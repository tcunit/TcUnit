---
layout: page
title: Programming example
---

## Introduction

For this example we are going to develop a TwinCAT library with some well defined functionality using test driven development with the TcUnit framework.
The scope of the library will be developing functions to handle certain aspects of the IO-Link communication.
IO-Link is a digital point-to-point (master and slave) serial communication protocol.
It's used to extend or replace the standard analog (0..10V, 4..20mA, +/- 10V etc) sensors with a digital interface.
With IO-Link you get a significantly improved and extended integration with the lowest level sensors in your system.
We won't go too much into details specifically about IO-Link, instead it's highly recommended to read more about it on the [IO-Link website](https://io-link.com/en/index.php).

For this topic however, what is important to know is that IO-Link provides certain services.
One of the functionalities of IO-Link devices is that they can fire off events to the IO-Link master to notify that something has happened, for instance an alarm that something is wrong.
By using an IO-Link master that is using EtherCAT for fieldbus communications to the higher level system (PLC) we can receive the events by doing CoE reads.
When using an EtherCAT-capable IO-Link master such as:

- [Balluff BNI0077](https://www.balluff.com/en-us/products/BNI0077#?data=)
- [Beckhoff EL6224](https://www.beckhoff.com/en-en/products/i-o/ethercat-terminals/el6xxx-communication/el6224.html#:~:text=The%20EL6224%20IO%2DLink%20terminal,the%20terminal%20and%20the%20device.) or [EP6224](https://www.beckhoff.com/en-en/products/i-o/ethercat-box/epxxxx-industrial-housing/ep6xxx-communication/ep6224-0042.html)/[EP6228](https://www.beckhoff.com/en-en/products/i-o/ethercat-box/epxxxx-industrial-housing/ep6xxx-communication/ep6228-0022.html)
- [IFM AL1332](https://www.ifm.com/de/en/product/AL1332)
- [Omron GX-ILM08C](https://www.ia.omron.com/products/family/3541/)

which all support the CoE diagnosis history object (0x10F3), all IO-Link events are stored in the memory of the device.
It's important to note that the diagnosis history object (0x10F3) can be implemented by any EtherCAT slave to store diagnostic type of data, not only IO-Link events.
Note that the implementation of the diagnosis history object is optional by the manufacturer.
Whether the diagnosis history object is implemented or not is reported by a flag in the ESI-file of the EtherCAT slave.
According to EtherCAT Technology Group document "[ETG1020 – EtherCAT Protocol Enhancements](https://www.ethercat.org/en/downloads/downloads_00600A7B120E41D385DC0AF19C034434.htm)", each message logged in the diagnostic history object has the following data:

- Diagnosis code (4 bytes) – mandatory
- Flags (2 bytes) – mandatory
- Text ID (2 bytes) – mandatory
- Timestamp (8 bytes) – mandatory
- Optional parameters – optional

This is only a description of the data on a high level, for all the details on what's exactly included on a bit-level all information can be found in ETG1020.
The number of optional parameters can be varying (zero parameters as well) depending on the diagnosis message itself.

## Data to be parsed

What we will do here is to create test cases to parse each and one of the mandatory fields.
Each field will be parsed by its own function block that will provide the data above in a structured manner.
Looking at the diagnosis history object, the diagnosis messages themselves are an array of bytes that are read by SDO read. For this particular example, we assume we have the stream of bytes already prepared by the SDO read.
We will not focus on creating test cases for the SDO read, but rather focus on the parsing.
In one of the tested IO-Link masters, the diagnosis history message object is up to 28 bytes, which means that the IO-Link master on top of the above data also supports a certain number of parameters.
As said above however, we'll focus on the parsing of the first 4+2+2+8 = 16 bytes, as the first four parameters in the diagnosis message are mandatory whilst the parameters are optional.
What we need to do now is to create a data structure for each of the data fields above.

Before we start to dwell too deep into the code, it's good to know that all the source code for the complete example is available [on GitHub](https://github.com/tcunit/ExampleProjects/tree/master/AdvancedExampleProject), as it might be preferred to look at the code in the Visual Studio IDE rather than on a webpage.

### Diagnosis code

The diagnosis code looks like this:

| Bit 0-15 | Bit 16-31 |
|-|-|
|0x0000-0xDFFF|Not used|
|0xE000-0xE7FF|Can be used manufacturer specific|
|0xE800|Emergency Error Code as defined in DS301 or DS4xxx|
|0xE801-0xEDFF|Reserved for future standardization|
|0xEE00-0xEFFF|Profile specific|
|0xF000-0xFFFF|Not used|

We'll create a struct for it:

```StructuredText
TYPE ST_DIAGNOSTICCODE :
STRUCT
    eDiagnosticCodeType : E_DIAGNOSTICCODETYPE;
    nCode : UINT;
END_STRUCT
END_TYPE
```

where the `E_DIAGNOSTICCODETYPE` is

```StructuredText
TYPE E_DIAGNOSTICCODETYPE :
(
    ManufacturerSpecific := 0,
    EmergencyErrorCodeDS301 := 1,
    ProfileSpecific := 2,
    Unspecified := 3
) USINT;
END_TYPE
```

What we're basically doing here is to first look at the first 16 bits, and categorizing them into any of the four possibilities of the enumeration `E_DIAGNOSTICCODETYPE`.
All unknowns (reserved, not used) are set as `E_DIAGNOSTICCODETYPE.Unspecified`.
Then we convert bit 16-31 into nCode. These two together will create the struct `ST_DIAGNOSTICCODE`.

### Flags

The flags have three parameters: "Diagnosis type", "Time stamp type", "Number of parameters in the diagnosis message".
|Bit|Description|
|-|-|
|Bit 0-3|0: Info message, 1: Warning message, 2: Error message, 3-15: Reserved for future use|
|Bit 4|Time stamp is a local time stamp|
|Bit 5-7|Reserved for future use|
|Bit 8-15|Number of parameters in this diagnosis message|

We'll create a struct for it:

```StructuredText
TYPE ST_FLAGS :
STRUCT
    eDiagnostisType : E_DIAGNOSISTYPE;
    eTimeStampType : E_TIMESTAMPTYPE;
    nNumberOfParametersInDiagnosisMessage : USINT;
END_STRUCT
END_TYPE
```

Where `E_DiagnosisType` and `E_TimeStampType` are respectively:

```StructuredText
TYPE E_DIAGNOSISTYPE :
(
    InfoMessage := 0,
    WarningMessage := 1,
    ErrorMessage := 2,
    Unspecified := 3
) BYTE;
END_TYPE
```

Where the `Unspecified` value is there in case we would receive one of the values that are reserved for future standardization.

```StructuredText
TYPE E_TIMESTAMPTYPE :
(
    Local := 0,
    Global := 1
) USINT;
END_TYPE
```

The timestamp is obtained from the local clock of the terminal at the time of the event.
The difference between the global and local timestamp is that the global is based on the DC-clock of the reference clock (and is thus global on the whole DC network), whilst a local clock is only used internally in the EtherCAT slave.
It's interesting to store this information as you probably want to handle the reading of the timestamp differently depending on if it's a local or a global timestamp.

### Text identity

The text identity is just a simple unsigned integer (0-65535) value which is a reference to the diagnosis text file located in the ESI-xml file for the IO-Link master.
This information is valuable if you want to do further analysis on the event, as it will give you more details on the event in a textual format.

### Time stamp

The 64 bit timestamp is either the EtherCAT DC-clock timestamp or the local time stamp of the EtherCAT slave/IO-Link master, depending on whether DC is enabled and/or the IO-Link master supports DC.
The 64-bit value holds data with 1ns resolution.

### The complete diagnostic message

Now that we have all four diagnosis message parameters, we'll finish off by creating a structure for them that our parser will deliver as output once a new diagnosis event has occurred.
Based on the information provided above it will have the following layout:

```StructuredText
TYPE ST_DIAGNOSTICMESSAGE :
STRUCT
    stDiagnosticCode : ST_DIAGNOSTICCODE;
    stFlags : ST_FLAGS;
    nTextIdentityReferenceToESIFile : UINT;
    sTimeStamp : STRING(29);
END_STRUCT
END_TYPE
```

Here you can obviously have the timestamp as a `DC_Time64`-type instead of the `STRING`-type, as it's normally more interesting to store time/data as strings the closer you come to the operator.
But for the sake of showing the concept of writing unit test cases, we'll stick to strings.
The reason we went for a 29 byte is because this is the size of the string that is returned when doing a `DCTIME64_TO_STRING()` function call.

## The function blocks

Let's create the headers for all the function blocks that we will write unit tests for.
What we'll do is to create a function block for parsing each and one of the parameters (a total number of four), and an additional function block that uses all these four separate function blocks to deliver the result in the struct `ST_DIAGNOSTICMESSAGE`.
A rudimentary scheme for this looks like follows:
![Function block layout](img/function-block-layout.png)

Note that we at this stage are not implementing the function blocks, we're only stating what functionality they must provide, by declaring their interfaces.
As this example is quite simple, we'll solve that for every function block by making them provide a function block output.
The function blocks and their headers will have the following layout:

### Main diagnosis message event parser

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageParser
VAR_INPUT
    anDiagnosticMessageBuffer : ARRAY[1..28] OF BYTE;
END_VAR
VAR_OUTPUT
    stDiagnosticMessage : ST_DIAGNOSTICMESSAGE;
END_VAR
```

This takes the 28 bytes that we receive from the IO-Link master, and outputs the complete diagnostic message according to the layout of our struct `ST_DIAGNOSTICMESSAGE` described earlier.
Note that in this example we'll only make use of the first 16 (mandatory) bytes, and ignore the 12 (optional) bytes.

### Diagnostic code parser

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageDiagnosticCodeParser
VAR_INPUT
    anDiagnosticCodeBuffer : ARRAY[1..4] OF BYTE;
END_VAR
VAR_OUTPUT
    stDiagnosticCode : ST_DIAGNOSTICCODE;
END_VAR
```

This function block takes four of the 28 bytes as input and outputs the diagnostic code according to the layout of our struct `ST_DIAGNOSTICCODE` described earlier.

### Flags parser

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageFlagsParser
VAR_INPUT
    anFlagsBuffer : ARRAY[1..2] OF BYTE;
END_VAR
VAR_OUTPUT
    stFlags : ST_FLAGS;
END_VAR
```

This function block takes two of the 28 bytes as input and outputs the flags according to the layout of our struct `ST_FLAGS` described earlier.

### Text identity parser

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageTextIdentityParser
VAR_INPUT
    anTextIdentityBuffer : ARRAY[1..2] OF BYTE;
END_VAR
VAR_OUTPUT
    nTextIdentity : UINT;
END_VAR
```

This function block takes two of the 28 bytes as input and outputs the text identity as an unsigned integer according to the description earlier.

### Timestamp parser

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageTimeStampParser
VAR_INPUT
    anTimeStampBuffer : ARRAY[1..8] OF BYTE;
    bIsLocalTime : BOOL;
END_VAR
VAR_OUTPUT
    sTimeStamp : STRING(29);
END_VAR
```

This function block takes eight of the 28 bytes as input and outputs the timestamp as a human-readable string.
Note that we also have a bIsLocalTime input, as we want to have different handling on the parsing of the timestamp depending on whether the timestamp is a local or global time stamp.
This could be handled in many ways, but for the sake of this example we'll handle the timestamp as:

- If it's global, the timestamp is based on the EtherCAT distributed clock of the system and we'll make use of/parse the eight bytes
- If it's local, we ignore the eight bytes and instead return the current task distributed clock time

Although this text is about unit testing, it's worth pointing out that we could handle this differently in such a way that if the timestamp is local, we could read-out the local time of the EtherCAT slave/IO-Link master (if available, located in CoE object 0x10F8), and then calculate what the global time stamp is by calculating the age of the message, and subtracting this from the current DC-time.

And that's it!
Now we have prepared all the data types and function blocks that together form the functionality specification of our parser.
What's nice about this is that we now have formed the acceptance criteria for the expected functionality, which are all the outputs of the function blocks.
If we run our function blocks now with real input, they will of course not return the correct values.
Everything returned will just be with the default values of the different structures.
Our next step will be to write the unit tests that will make our tests fail, and once that is done (and not before!), we'll write the actual body (implementation) code for the function blocks.

## Test cases

As our IO-Link project is a library project and thus won’t have any runnable tasks to be running in any PLC, we still need to have a task and a program to run our unit tests.
This task will be running on our local development machine.
We need to create a task/program inside the library project which initiates all the unit tests, so we can run the unit tests which in turn initializes the library code that we want to test.
One suggestion is to put all unit tests in a folder called test next to the standard folder `POUs`, which are initialized by the program `PRG_TEST`. For every function block we've defined, we'll create an accompanying test function block (test suite).
One standard is to call this test suite the same as the function block that we want to test, but add "_test" in the name.
The framework does not enforce any naming rules, this is entirely up to the user to decide on a naming convention.

![Unit test structure](img/unit-test-structure-1.png)

As can be seen here every FB that we've defined a functionality for has an accompanying test-FB (test suite).
By structuring all your libraries in this way, all the function blocks and unit tests will always be available together.
Also, by having the program/task available in the library project, any developer can at anytime run the unit tests.
This is an excellent way to package everything nicely together.

Next, we instantiate every test suite in `PRG_MAIN`, and every test suite in turn instantiates the FB that it is supposed to test.
Every test suite can do as many tests for the FB under test as you want, i.e. make calls to the FBs with different inputs to test various scenarios.
One example for a parser FB could be to test min/mid/max values as input, totaling to three different tests.

![IO-Link unit tests](img/iolink-tests-tcunit.png)

This figure shows:

1. We create a total of five test suites, one for every real function block
2. We instantiate the test suites in `PRG_MAIN`
3. We create several test cases for every function block. Each test case tests the function block for certain inputs (test fixture) and expects certain outputs (test result)
4. `PRG_MAIN` calls each test suite

Every test that we are going to run requires some sort of test fixture, which are the prerequisites for the test.
In our case, this will be setting up the data bytes used as input for the function block under test.
For every test that we will run we will have to do an assertion, checking whether the result (output) we get from our function block is equal to the expected result.

Let's start by creating the five test suites (orange above), and instantiating them in `PRG_TEST` and running them.

```StructuredText
PROGRAM PRG_TEST
VAR
    fbDiagnosticMessageDiagnosticCodeParser_Test : FB_DiagnosticMessageDiagnosticCodeParser_Test;
    fbDiagnosticMessageFlagsParser_Test : FB_DiagnosticMessageFlagsParser_Test;
    fbDiagnosticMessageParser_Test : FB_DiagnosticMessageParser_Test;
    fbDiagnosticMessageTextIdentityParser_Test : FB_DiagnosticMessageTextIdentityParser_Test;
    fbDiagnosticMessageTimeStampParser_Test : FB_DiagnosticMessageTimeStampParser_Test;
END_VAR
---------------------------------------
TcUnit.RUN();
```

What we need to do now is to implement each unit test-FB with some tests that we think should be included for each parser.

### FB_DiagnosticMessageDiagnosticCodeParser_Test

The function block `FB_DiagnosticMessageDiagnosticCodeParser` is responsible for parsing a diagnostic code type (ManufacturerSpecific, EmergencyErrorCode or ProfileSpecific) together with the code itself.

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageDiagnosticCodeParser_Test EXTENDS TcUnit.FB_TestSuite
```

This is the required layout of the header of a test suite just as described in the [introduction user guide](introduction-user-guide.md).

Our next step is setting up all test fixtures and the expected test results.
Here we need to think about the different test cases that we would like to run.
We want to make sure our function block correctly parses the three different diagnostic code types, but we also want to make sure our code correctly handles the case when the IO-Link master for instance outputs a diagnosis code type that’s not valid (Reserved for future use for instance). In this case we want to make sure our function block sets it to “Unspecified”. To make sure that our code handles all these different cases, we want four good tests.

The first test will represent an emergency code, and the header of the method defining the test will look like follows:

```StructuredText
METHOD PRIVATE WhenEmergencyErrorCodeExpectEmergencyErrorCode
VAR
    fbDiagnosticMessageDiagnosticCodeParser : FB_DiagnosticMessageDiagnosticCodeParser;
    stDiagnosticCode : ST_DIAGNOSTICCODE;
 
    // @TEST-FIXTURE EmergencyErrorCode
    cnDiagnosticCodeBufferByte1_EmergencyErrorCode : BYTE := 16#00; // 16#E800
    cnDiagnosticCodeBufferByte2_EmergencyErrorCode : BYTE := 16#E8;
    cnDiagnosticCodeBufferByte3_EmergencyErrorCode : BYTE := 16#30; // 16#7530 = 10#30000
    cnDiagnosticCodeBufferByte4_EmergencyErrorCode : BYTE := 16#75;
    canDiagnosticCodeBuffer_EmergencyErrorCode : ARRAY[1..4] OF BYTE := [
                                                cnDiagnosticCodeBufferByte1_EmergencyErrorCode,
                                                cnDiagnosticCodeBufferByte2_EmergencyErrorCode,
                                                cnDiagnosticCodeBufferByte3_EmergencyErrorCode,
                                                cnDiagnosticCodeBufferByte4_EmergencyErrorCode];
    // @TEST-RESULT EmergencyErrorCode
    ceDiagnosticCodeType_EmergencyErrorCode : E_DIAGNOSTICCODETYPE := E_DIAGNOSTICCODETYPE.EmergencyErrorCodeDS301;
    cnDiagnosticCode_EmergencyErrorCode : UINT := 10#30000;
END_VAR
```

The two variables are the declaration of the function block under test (`fbDiagnosticMessageDiagnosticCodeParser`) and the structure where the result will be stored (`stDiagnosticCode`).
The `stDiagnosticCode` variable is the output of the FB that we'll be testing, and this is thus the one that we will compare the expected result with.
The variables starting with "c" are the input variables for our test (test fixture) and the expected result.
The four bytes for our test fixture represents:

|Bit 0-15|Bit 16-31|
|-|-|
|0x0000-0xDFFF|not used|
|0xE000-0xE7FF|can be used manufacturer specific|
|0xE800|Emergency Error Code as defined in DS301 or DS4xxx|
|0xE801-0xEDFF|reserved for future standardization|
|0xEE00-0xEFFF|Profile specific|
|0xF000-0xFFFF|not used|

We have two bytes for the type of diagnosis code, and two bytes for the code itself.
Emergency error code is defined as `0xE800`, and we set the code itself to `0x7530` (representing a decimal value of 30000).

![Diagnosis code bytes](img/diagnosis-code-bytes.png)

We put these four bytes into an array, which we’ll be using as an input to the FB under test once we are going to run our tests.
The test result for the struct that the function block outputs should be `EmergencyErrorCodeDS301` and 30000 (decimal), which is the result defined under the `@TEST-RESULT`.

Next we move to the body of the test suite:

```StructuredText
TEST('WhenEmergencyErrorCodeExpectEmergencyErrorCode');
 
// @TEST-RUN
fbDiagnosticMessageDiagnosticCodeParser(anDiagnosticCodeBuffer := canDiagnosticCodeBuffer_EmergencyErrorCode,
                                        stDiagnosticCode => stDiagnosticCode);
 
// @TEST-ASSERT
AssertEquals(Expected := ceDiagnosticCodeType_EmergencyErrorCode,
             Actual := stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'emergency error code$' failed at $'diagnostic code type$'');
AssertEquals(Expected := cnDiagnosticCode_EmergencyErrorCode,
             Actual := stDiagnosticCode.nCode,
             Message := 'Test $'emergency error code$' failed at $'diagnostic code$'');
 
TEST_FINISHED();
```

What we're doing here is to run the test by calling the function block with our test fixture (defined as an array) as input, and checking (by asserting) whether the result we get in our output is according to our expected test-result.
And that's basically everything there is to it!
If we were to run our code now, the test would fail simply because our function block doesn't do anything as it has not yet been implemented!
Note that we need to finish the method with a call to `TEST_FINISHED()` to indicate to the testing framework that this is the end of the tests.
This gives the possibility to have tests that span over multiple PLC-cycles, as it’s possible to have a condition that needs to be reached before calling `TEST_FINISHED()`.
Before doing any implementation code, we need to finish our different test cases so we cover as much scenarios as possible.
What follows are three additional test fixtures and expected test results.

**Test "`WhenManufacturerSpecificExpectManufacturerSpecific`"**

```StructuredText
METHOD PRIVATE WhenManufacturerSpecificExpectManufacturerSpecific
VAR
    fbDiagnosticMessageDiagnosticCodeParser : FB_DiagnosticMessageDiagnosticCodeParser;
    stDiagnosticCode : ST_DIAGNOSTICCODE;
 
    // @TEST-FIXTURE ManuFacturerSpecific
    cnDiagnosticCodeBufferByte1_ManufacturerSpecific : BYTE := 16#00; // 16#E000 (in range of 0xE000 - 0xE7FF)
    cnDiagnosticCodeBufferByte2_ManufacturerSpecific : BYTE := 16#E0;
    cnDiagnosticCodeBufferByte3_ManufacturerSpecific : BYTE := 16#E8; // 16#03E8 = 10#1000
    cnDiagnosticCodeBufferByte4_ManufacturerSpecific : BYTE := 16#03;
    canDiagnosticCodeBuffer_ManufacturerSpecific : ARRAY[1..4] OF BYTE := [
                                                cnDiagnosticCodeBufferByte1_ManufacturerSpecific,
                                                cnDiagnosticCodeBufferByte2_ManufacturerSpecific,
                                                cnDiagnosticCodeBufferByte3_ManufacturerSpecific,
                                                cnDiagnosticCodeBufferByte4_ManufacturerSpecific];
                                                 
    // @TEST-RESULT ManuFacturerSpecific
    ceDiagnosticCodeType_ManufacturerSpecific : E_DIAGNOSTICCODETYPE := E_DIAGNOSTICCODETYPE.ManufacturerSpecific;
    cnDiagnosticCode_ManufacturerSpecific : UINT := 10#1000;
END_VAR
-----------------------------------------------------------
TEST('WhenManufacturerSpecificExpectManufacturerSpecific');
 
// @TEST-RUN
fbDiagnosticMessageDiagnosticCodeParser(anDiagnosticCodeBuffer := canDiagnosticCodeBuffer_ManufacturerSpecific,
                                        stDiagnosticCode => stDiagnosticCode);
 
// @TEST-ASSERT
AssertEquals(Expected := ceDiagnosticCodeType_ManufacturerSpecific,
             Actual := stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'manufacturer specific$' failed at $'diagnostic code type$'');
AssertEquals(Expected := cnDiagnosticCode_ManufacturerSpecific,
             Actual := stDiagnosticCode.nCode,
             Message := 'Test $'manufacturer specific$' failed at $'diagnostic code$'');
 
TEST_FINISHED();

```

**Test "`WhenProfileSpecificExpectProfileSpecific`"**

```StructuredText
METHOD PRIVATE WhenProfileSpecificExpectProfileSpecific
VAR
    fbDiagnosticMessageDiagnosticCodeParser : FB_DiagnosticMessageDiagnosticCodeParser;
    stDiagnosticCode : ST_DIAGNOSTICCODE;
 
    // @TEST-FIXTURE ProfileSpecific
    cnDiagnosticCodeBufferByte1_ProfileSpecific : BYTE := 16#10; // 16#EF10 (in range of 0xEE00 - 0xEFFF)
    cnDiagnosticCodeBufferByte2_ProfileSpecific : BYTE := 16#EF;
    cnDiagnosticCodeBufferByte3_ProfileSpecific : BYTE := 16#FF; // 16#FFFF = 10#65535
    cnDiagnosticCodeBufferByte4_ProfileSpecific : BYTE := 16#FF;
    canDiagnosticCodeBuffer_ProfileSpecific : ARRAY[1..4] OF BYTE := [
                                                cnDiagnosticCodeBufferByte1_ProfileSpecific,
                                                cnDiagnosticCodeBufferByte2_ProfileSpecific,
                                                cnDiagnosticCodeBufferByte3_ProfileSpecific,
                                                cnDiagnosticCodeBufferByte4_ProfileSpecific];
 
    // @TEST-RESULT ProfileSpecific
    ceDiagnosticCodeType_ProfileSpecific : E_DIAGNOSTICCODETYPE := E_DIAGNOSTICCODETYPE.ProfileSpecific;
    cnDiagnosticCode_ProfileSpecific : UINT := 10#65535;
END_VAR
-------------------------------------------------
TEST('WhenProfileSpecificExpectProfileSpecific');
 
// @TEST-RUN
fbDiagnosticMessageDiagnosticCodeParser(anDiagnosticCodeBuffer := canDiagnosticCodeBuffer_ProfileSpecific,
                                        stDiagnosticCode => stDiagnosticCode);
 
// @TEST-ASSERT
AssertEquals(Expected := ceDiagnosticCodeType_ProfileSpecific,
             Actual := stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'profile specific$' failed at $'diagnostic code type$'');
AssertEquals(Expected := cnDiagnosticCode_ProfileSpecific,
             Actual := stDiagnosticCode.nCode,
             Message := 'Test $'profile specific$' failed at $'diagnostic code$'');
 
TEST_FINISHED();
```

**Test "`WhenReservedForFutureUseExpectReservedForFutureUse`"**

```StructuredText
METHOD PRIVATE WhenReservedForFutureUseExpectReservedForFutureUse
VAR
    fbDiagnosticMessageDiagnosticCodeParser : FB_DiagnosticMessageDiagnosticCodeParser;
    stDiagnosticCode : ST_DIAGNOSTICCODE;
 
    // @TEST-FIXTURE ReservedForFutureUse
    cnDiagnosticCodeBufferByte1_ReservedForFutureUse : BYTE := 16#01; // 16#E801 (in range of 0xE801 - 0xEDFF)
    cnDiagnosticCodeBufferByte2_ReservedForFutureUse : BYTE := 16#E8;
    cnDiagnosticCodeBufferByte3_ReservedForFutureUse : BYTE := 16#D9; // 16#3BD9 = 10#15321
    cnDiagnosticCodeBufferByte4_ReservedForFutureUse : BYTE := 16#3B;
    canDiagnosticCodeBuffer_ReservedForFutureUse : ARRAY[1..4] OF BYTE := [
                                                cnDiagnosticCodeBufferByte1_ReservedForFutureUse,
                                                cnDiagnosticCodeBufferByte2_ReservedForFutureUse,
                                                cnDiagnosticCodeBufferByte3_ReservedForFutureUse,
                                                cnDiagnosticCodeBufferByte4_ReservedForFutureUse];
 
    // @TEST-RESULT ReservedForFutureUse
    ceDiagnosticCodeType_ReservedForFutureUse : E_DIAGNOSTICCODETYPE := E_DIAGNOSTICCODETYPE.Unspecified;
    cnDiagnosticCode_ReservedForFutureUse : UINT := 10#15321;
END_VAR
 
TEST('WhenReservedForFutureUseExpectReservedForFutureUse');
 
// @TEST-RUN
fbDiagnosticMessageDiagnosticCodeParser(anDiagnosticCodeBuffer := canDiagnosticCodeBuffer_ReservedForFutureUse,
                                        stDiagnosticCode => stDiagnosticCode);
 
// @TEST-ASSERT
AssertEquals(Expected := ceDiagnosticCodeType_ReservedForFutureUse,
             Actual := stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'reserved for future use$' failed at $'diagnostic code type$'');
AssertEquals(Expected := cnDiagnosticCode_ReservedForFutureUse,
             Actual := stDiagnosticCode.nCode,
             Message := 'Test $'reserved for future use$' failed at $'diagnostic code$'');
 
TEST_FINISHED();
```

We basically cover all the different use cases for the type of diagnosis code.
Notice that the test fixture is a code that according to the ETG1020 specification is "reserved for future use", while our result should be "unspecified".
We should handle all reserved/unknown as "Unspecified", and thus this is what the output of our function block should be.
Every time we run the function block under test, we assert that the output (`stDiagnosticCode`) is equal to our expected `@TEST-RESULT`.

Next we need to make sure to call all the tests in the body of the test suite.

```StructuredText
WhenEmergencyErrorCodeExpectEmergencyErrorCode();
WhenManufacturerSpecificExpectManufacturerSpecific();
WhenProfileSpecificExpectProfileSpecific();
WhenReservedForFutureUseExpectReservedForFutureUse();
```

### FB_DiagnosticMessageFlagsParser_Test

The next function block that we want to write tests for is the one that parses the different flags in the event message.
The tests will follow the same layout as for the previous function block, where we:

- Instantiate the function block under test
- Declare test-fixtures for our tests
- Declare the test-results for the test-fixtures
- Run the tests  

The layout of the two bytes for the flags looks like this:
|Bit|Description|
|-|-|
|0-3|0: Info message, 1: Warning message, 2: Error message, 3-15: Reserved for future use|
|4|Time stamp is a local time stamp|
|5-7|Reserved for future use|
|8-15|Number of parameters in this diagnosis message|

A couple of good tests would be to try every code type (info, warning, error) and with some different combinations of timestamp and amount of parameters.

`TODO: INSERT IMAGE HERE`

Let's write four tests and call them:

- `WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`
- `WhenInfoMessageExpectInfoMessageGlobalTimestampAndZeroParameters`
- `WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters`
- `WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters`  

It's a good idea to have descriptive names about what the test is testing and what the expected result is.
In this way any developer reading the tests can clearly understand the goal of the tests.
With all this information other developers get a lot of documentation for free!

**Test "`WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`"**

```StructuredText
METHOD PRIVATE WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters
VAR
    fbDiagnosticMessageFlagsParser : FB_DiagnosticMessageFlagsParser;
    stFlags : ST_FLAGS;
 
    // @TEST-FIXTURE ErrorMessage
    cnFlagsBufferByte1_ErrorMessage : BYTE := 2#0001_0010; // Error message and local time stamp
    cnFlagsBufferByte2_ErrorMessage : BYTE := 2#0000_0100; // Four parameters in the diagnosis message
    canFlagsBuffer_ErrorMessage : ARRAY[1..2] OF BYTE := [cnFlagsBufferByte1_ErrorMessage, 
                                                          cnFlagsBufferByte2_ErrorMessage];
    // @TEST-RESULT ErrorMessage
    ceFlags_DiagnosisTypeErrorMessage : E_DIAGNOSISTYPE := E_DIAGNOSISTYPE.ErrorMessage;
    ceFlags_TimeStampTypeLocal : E_TIMESTAMPTYPE := E_TIMESTAMPTYPE.Local;
    cnFlags_NumberOfParametersInDiagnosisMessageFour : USINT := 4;
END_VAR
--------------------------------------------------------------------------
TEST('WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters');
 
// @TEST-RUN ErrorMessage
fbDiagnosticMessageFlagsParser(anFlagsBuffer := canFlagsBuffer_ErrorMessage,
                               stFlags => stFlags);
 
// @TEST-ASSERT ErrorMessage
AssertEquals(Expected := ceFlags_DiagnosisTypeErrorMessage,
             Actual := stFlags.eDiagnosisType,
             Message :='Test $'Error message$' failed at $'diagnosis type$'');
AssertEquals(Expected := ceFlags_TimeStampTypeLocal,
             Actual := stFlags.eTimeStampType,
             Message := 'Test $'Error message$' failed at $'timestamp type$'');
AssertEquals(Expected := cnFlags_NumberOfParametersInDiagnosisMessageFour,
             Actual := stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'Error message$' failed at $'number of parameters$'');
 
TEST_FINISHED();
```

**Test "`WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`"**

```StructuredText
METHOD PRIVATE WhenInfoMessageExpectInfoMessageGlobalTimestampAndZeroParameters
VAR
    fbDiagnosticMessageFlagsParser : FB_DiagnosticMessageFlagsParser;
    stFlags : ST_FLAGS;
 
    // @TEST-FIXTURE InfoMessage
    cnFlagsBufferByte1_InfoMessage : BYTE := 2#0000_0000; // Info message and global time stamp
    cnFlagsBufferByte2_InfoMessage : BYTE := 2#0000_0000; // Zero parameters in the diagnosis message
    canFlagsBuffer_InfoMessage : ARRAY[1..2] OF BYTE := [cnFlagsBufferByte1_InfoMessage, 
                                                         cnFlagsBufferByte2_InfoMessage];
    // @TEST-RESULT InfoMessage
    ceFlags_DiagnosisTypeInfoMessage : E_DIAGNOSISTYPE := E_DIAGNOSISTYPE.InfoMessage;
    ceFlags_TimeStampTypeGlobal : E_TIMESTAMPTYPE := E_TIMESTAMPTYPE.Global;
    cnFlags_NumberOfParametersInDiagnosisMessageZero : USINT := 0;
END_VAR
-------------------------------------------------------------------------
TEST('WhenInfoMessageExpectInfoMessageGlobalTimestampAndZeroParameters');
 
// @TEST-RUN
fbDiagnosticMessageFlagsParser(anFlagsBuffer := canFlagsBuffer_InfoMessage,
                               stFlags => stFlags);
 
// @TEST-ASSERT
AssertEquals(Expected := ceFlags_DiagnosisTypeInfoMessage,
             Actual := stFlags.eDiagnosisType,
             Message := 'Test $'Info message$' failed at $'diagnosis type$'');
AssertEquals(Expected := ceFlags_TimeStampTypeGlobal,
             Actual := stFlags.eTimeStampType,
             Message := 'Test $'Info message$' failed at $'timestamp type$'');
AssertEquals(Expected := cnFlags_NumberOfParametersInDiagnosisMessageZero,
             Actual := stFlags.nNumberOfParametersInDiagnosisMessage,
             Message :='Test $'Info message$' failed at $'number of parameters$'');
 
TEST_FINISHED();
```

**Test "`WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters`"**

```StructuredText
METHOD PRIVATE WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters
VAR
    fbDiagnosticMessageFlagsParser : FB_DiagnosticMessageFlagsParser;
    stFlags : ST_FLAGS;
 
    // @TEST-FIXTURE ReservedForFutureUseMessage
    cnFlagsBufferByte1_ReservedForFutureUseMessage : BYTE := 2#0001_0011; // ReservedForFutureUse message and local time stamp
    cnFlagsBufferByte2_ReservedForFutureUseMessage : BYTE := 2#0010_0001; // 33 parameters in the diagnosis message
    canFlagsBuffer_ReservedForFutureUseMessage : ARRAY[1..2] OF BYTE := [cnFlagsBufferByte1_ReservedForFutureUseMessage, 
                                                                         cnFlagsBufferByte2_ReservedForFutureUseMessage];
    // @TEST-RESULT ReservedForFutureUseMessage
    ceFlags_DiagnosisTypeReservedForFutureUseMessage : E_DIAGNOSISTYPE := E_DIAGNOSISTYPE.Unspecified;
    cnFlags_NumberOfParametersInDiagnosisMessage33 : USINT := 33;
    ceFlags_TimeStampTypeLocal : E_TIMESTAMPTYPE := E_TIMESTAMPTYPE.Local;
END_VAR
------------------------------------------------------------------------------------------------------
TEST('WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters');
 
// @TEST-RUN ReservedForFutureUseMessage
fbDiagnosticMessageFlagsParser(anFlagsBuffer := canFlagsBuffer_ReservedForFutureUseMessage,
                               stFlags => stFlags);
 
// @TEST-ASSERT ReservedForFutureUseMessage
AssertEquals(Expected := ceFlags_DiagnosisTypeReservedForFutureUseMessage,
             Actual := stFlags.eDiagnosisType,
             Message := 'Test $'Reserved for future use message$' failed at $'diagnosis type$'');
AssertEquals(Expected := ceFlags_TimeStampTypeLocal,
             Actual := stFlags.eTimeStampType,
             Message := 'Test $'Reserved for future use message$' failed at $'timestamp type$'');
AssertEquals(Expected := cnFlags_NumberOfParametersInDiagnosisMessage33,
             Actual := stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'Reserved for future use message$' failed at $'number of parameters$'');
 
TEST_FINISHED();
```

**Test "`WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters`"**

```StructuredText
METHOD PRIVATE WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters
VAR
    fbDiagnosticMessageFlagsParser : FB_DiagnosticMessageFlagsParser;
    stFlags : ST_FLAGS;
 
    // @TEST-FIXTURE WarningMessage
    cnFlagsBufferByte1_WarningMessage : BYTE := 2#0001_0001; // Warning message and local time stamp
    cnFlagsBufferByte2_WarningMessage : BYTE := 2#0000_0010; // Two parameters in the diagnosis message
    canFlagsBuffer_WarningMessage : ARRAY[1..2] OF BYTE := [cnFlagsBufferByte1_WarningMessage, 
                                                            cnFlagsBufferByte2_WarningMessage];
    // @TEST-RESULT WarningMessage
    ceFlags_DiagnosisTypeWarningMessage : E_DIAGNOSISTYPE := E_DIAGNOSISTYPE.WarningMessage;
    ceFlags_TimeStampTypeLocal : E_TIMESTAMPTYPE := E_TIMESTAMPTYPE.Local;
    cnFlags_NumberOfParametersInDiagnosisMessageTwo : USINT := 2;
END_VAR
-----------------------------------------------------------------------------
TEST('WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters');
 
// @TEST-RUN WarningMessage
fbDiagnosticMessageFlagsParser(anFlagsBuffer := canFlagsBuffer_WarningMessage,
                               stFlags => stFlags);
 
// @TEST-ASSERT WarningMessage
AssertEquals(Expected := ceFlags_DiagnosisTypeWarningMessage,
             Actual := stFlags.eDiagnosisType,
             Message := 'Test $'Warning message$' failed at $'diagnosis type$'');
AssertEquals(Expected := ceFlags_TimeStampTypeLocal,
             Actual := stFlags.eTimeStampType,
             Message := 'Test $'Warning message$' failed at $'timestamp type$'');
AssertEquals(Expected := cnFlags_NumberOfParametersInDiagnosisMessageTwo,
             Actual := stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'Warning message$' failed at $'number of parameters$'');
 
TEST_FINISHED();
```

We differentiate between the different tests by changing the contents of the two bytes defining the flags-parameter.
By changing the first four bits of the first byte, we change the diagnosis type (info, warning, error, unspecified).
To verify that our code outputs a diagnosis type of unspecified, we need to make sure that the first four bits of the first byte have a value of 4-15 (decimal), which is reserved for future use.
This is what is done in the fourth text fixture.
Finally we need to call the function block under test with all the test fixtures and assert the result for each and one of them, just like we did for the diagnosis code function block previously.

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageFlagsParser_Test EXTENDS TcUnit.FB_TestSuite
```

And as usual, we need to add a call to all the test-methods in the body of the test suite.

```StructuredText
TestWithEmergencyMessage();
TestWithManufacturerSpecificMessage();
TestWithUnspecifiedMessageMessage();
TestWithUnspecifiedMessageMessage_ParameterVariant();
```

What we've got left is to create test cases for the parsing of the text identity and the timestamp of the diagnostic event.
Then we also want to have a few tests that closes the loop and verifies the parsing of a complete diagnosis history message.

### FB_DiagnosticMessageTextIdentityParser_Test

The only input for the text identity are two bytes that together make up an unsigned integer (0-65535), which is the result (output) of this parser.
It's enough to make three test cases; one for low/medium/max.
We accomplish to test the three values by changing the two bytes that make up the unsigned integer.
The header of the test suite:

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageTextIdentityParser_Test EXTENDS TcUnit.FB_TestSuite
```

We'll write the tests Low/Med/High, testing for the different inputs 0, 34500 and 65535.

**Test "`WhenTextIdentityLowExpectTextIdentity0`"**

```StructuredText
METHOD PRIVATE WhenTextIdentityLowExpectTextIdentity0
VAR
    fbDiagnosticMessageTextIdentityParser : FB_DiagnosticMessageTextIdentityParser;
    nTextIdentity : UINT;
 
    // @TEST-FIXTURE TextIdentity#Low
    cnTextIdentityBufferByte1_IdentityLow : BYTE := 16#00; // 0 = no text identity
    cnTextIdentityBufferByte2_IdentityLow : BYTE := 16#00;
    canTextIdentityBuffer_IdentityLow : ARRAY[1..2] OF BYTE := [cnTextIdentityBufferByte1_IdentityLow,
                                                                cnTextIdentityBufferByte2_IdentityLow];
    // @TEST-RESULT TextIdentity#Low
    cnTextIdentity_IdentityLow : UINT := 0;
END_VAR
----------------------------------------------- 
TEST('WhenTextIdentityLowExpectTextIdentity0');
 
// @TEST-RUN
fbDiagnosticMessageTextIdentityParser(anTextIdentityBuffer := canTextIdentityBuffer_IdentityLow,
                                      nTextIdentity => nTextIdentity);
// @TEST-ASSERT
Assert.AssertEquals(Expected := cnTextIdentity_IdentityLow,
                    Actual := nTextIdentity,
                    Message := 'Test $'TextIdentity#Low$' failed');
 
TEST_FINISHED();
```

**Test "`WhenTextIdentityMedExpectTextIdentity34500`"**

```StructuredText
METHOD PRIVATE WhenTextIdentityMedExpectTextIdentity34500
VAR
    fbDiagnosticMessageTextIdentityParser : FB_DiagnosticMessageTextIdentityParser;
    nTextIdentity : UINT;
 
    // @TEST-FIXTURE TextIdentity#Med
    cnTextIdentityBufferByte1_IdentityMed : BYTE := 16#C4; // 0x86C4 = 34500
    cnTextIdentityBufferByte2_IdentityMed : BYTE := 16#86;
    canTextIdentityBuffer_IdentityMed : ARRAY[1..2] OF BYTE := [cnTextIdentityBufferByte1_IdentityMed,
                                                                cnTextIdentityBufferByte2_IdentityMed];
    // @TEST-RESULT TextIdentity#Med
    cnTextIdentity_IdentityMed : UINT := 34500;
END_VAR
---------------------------------------------------
TEST('WhenTextIdentityMedExpectTextIdentity34500');
 
// @TEST-RUN
fbDiagnosticMessageTextIdentityParser(anTextIdentityBuffer := canTextIdentityBuffer_IdentityMed,
                                      nTextIdentity => nTextIdentity);
// @TEST-ASSERT
AssertEquals(Expected := cnTextIdentity_IdentityMed,
             Actual := nTextIdentity,
             Message := 'Test $'TextIdentity#Med$' failed');
 
TEST_FINISHED();
```

**Test "`WhenTextIdentityHighExpectTextIdentity65535`"**

```StructuredText
METHOD PRIVATE WhenTextIdentityHighExpectTextIdentity65535
VAR
    fbDiagnosticMessageTextIdentityParser : FB_DiagnosticMessageTextIdentityParser;
    nTextIdentity : UINT;
 
    // @TEST-FIXTURE TextIdentity#High
    cnTextIdentityBufferByte1_IdentityHigh : BYTE := 16#FF; // 0xFFFF = 65535
    cnTextIdentityBufferByte2_IdentityHigh : BYTE := 16#FF;
    canTextIdentityBuffer_IdentityHigh : ARRAY[1..2] OF BYTE := [cnTextIdentityBufferByte1_IdentityHigh,
                                                                 cnTextIdentityBufferByte2_IdentityHigh];
    // @TEST-RESULT TextIdentity#High
    cnTextIdentity_IdentityHigh : UINT := 65535;
END_VAR
----------------------------------------------------
TEST('WhenTextIdentityHighExpectTextIdentity65535');
 
// @TEST-RUN
fbDiagnosticMessageTextIdentityParser(anTextIdentityBuffer := canTextIdentityBuffer_IdentityHigh,
                                      nTextIdentity => nTextIdentity);
// @TEST-ASSERT
AssertEquals(Expected := cnTextIdentity_IdentityHigh,
             Actual := nTextIdentity,
             Message := 'Test $'TextIdentity#High$' failed');
 
TEST_FINISHED();

```StructuredText
As can be seen the only thing that varies between the tests (other than name) is the different inputs and expected output.

### FB_DiagnosticMessageTimeStampParser_Test

The eight bytes that make up the timestamp can be either the distributed clock (DC) from EtherCAT, or a local clock in the device itself.
In the global case we want to parse the DC-time, while in the local case we just want to take the DC from the current task time (the local clock could be extracted from the EtherCAT-slave, but for the sake of simplicity we'll use the task DC).
Because the local/global-flag is read from the "Flags"-FB, this information needs to be provided into the timestamp-FB, and is therefore an input to the FB.
What this means is that if the timestamp is local, the eight bytes don't matter as we'll get the time from the task.
For the timestamp-FB it's enough with two test cases, one testing it with a local timestamp and the other with a global timestamp.
The local timestamp unit test result has to be created in runtime.

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageTimeStampParser_Test EXTENDS TcUnit.FB_TestSuite
```

Let's create our tests, and start with the test * **"`TestWithTimestampZeroTimeExpectCurrentTime`"**.

```StructuredText
METHOD PRIVATE TestWithTimestampZeroTimeExpectCurrentTime
VAR
    fbDiagnosticMessageTimeStampParser : FB_DiagnosticMessageTimeStampParser;
    sTimeStamp : STRING(29);
 
    nCurrentDcTaskTime : Tc2_EtherCAT.T_DCTIME64;
    sCurrentDcTaskTimeString : STRING(29);
 
    // @TEST-FIXTURE time stamp zero time
    canTimeStampBuffer_TimeStampZeroTime : ARRAY[1..8] OF BYTE := [8(16#00)];
END_VAR
---------------------------------------------------
TEST('TestWithTimestampZeroTimeExpectCurrentTime');
 
// @TEST-RUN
fbDiagnosticMessageTimeStampParser(anTimeStampBuffer := canTimeStampBuffer_TimeStampZeroTime,
                                   sTimeStamp => sTimeStamp);
nCurrentDcTaskTime := Tc2_EtherCAT.F_GetCurDcTaskTime64();
sCurrentDcTaskTimeString := DCTIME64_TO_STRING(in := nCurrentDcTaskTime);
 
// @TEST-ASSERT
AssertEquals(Expected := sCurrentDcTaskTimeString,
             Actual := sTimeStamp,
             Message := 'Test $'TimeStamp zero time$' failed');
 
TEST_FINISHED();
```

**Test "`TestWithValidTimestampExpectSameTimestamp`"**

```StructuredText
METHOD PRIVATE TestWithValidTimestampExpectSameTimestamp
VAR
    fbDiagnosticMessageTimeStampParser : FB_DiagnosticMessageTimeStampParser;
    sTimeStamp : STRING(29);
 
    // @TEST-FIXTURE TimeStamp valid time
    cnTimeStampBufferByte1_TimeStampValidTime : BYTE := 16#C0; // 0x07C76560A71025C0 = '2017-10-05-14:15:44.425035200'
    cnTimeStampBufferByte2_TimeStampValidTime : BYTE := 16#25;
    cnTimeStampBufferByte3_TimeStampValidTime : BYTE := 16#10;
    cnTimeStampBufferByte4_TimeStampValidTime : BYTE := 16#A7;
    cnTimeStampBufferByte5_TimeStampValidTime : BYTE := 16#60;
    cnTimeStampBufferByte6_TimeStampValidTime : BYTE := 16#65;
    cnTimeStampBufferByte7_TimeStampValidTime : BYTE := 16#C7;
    cnTimeStampBufferByte8_TimeStampValidTime : BYTE := 16#07;
    canTimeStampBuffer_TimeStampValidTime : ARRAY[1..8] OF BYTE := [cnTimeStampBufferByte1_TimeStampValidTime,
                                                                    cnTimeStampBufferByte2_TimeStampValidTime,
                                                                    cnTimeStampBufferByte3_TimeStampValidTime,
                                                                    cnTimeStampBufferByte4_TimeStampValidTime,
                                                                    cnTimeStampBufferByte5_TimeStampValidTime,
                                                                    cnTimeStampBufferByte6_TimeStampValidTime,
                                                                    cnTimeStampBufferByte7_TimeStampValidTime,
                                                                    cnTimeStampBufferByte8_TimeStampValidTime];
    // @TEST-RESULT TimeStamp valid time
    csTimeStamp_TimeStampValidTime : STRING(29) := '2017-10-05-14:15:44.425035200'; // T_DCTime64 = 16#07C76560A71025C0
END_VAR
--------------------------------------------------
TEST('TestWithValidTimestampExpectSameTimestamp');
 
// @TEST-RUN
fbDiagnosticMessageTimeStampParser(anTimeStampBuffer := canTimeStampBuffer_TimeStampValidTime,
                                   sTimeStamp => sTimeStamp);
 
// @TEST-ASSERT
AssertEquals(Expected := csTimeStamp_TimeStampValidTime,
             Actual := sTimeStamp,
             Message := 'Test $'TimeStamp zero time$' failed');
 
TEST_FINISHED();
```

For the local timestamp case, we can see that we setup the test-fixture for the eight bytes to zeros, as this data is not necessary for the local timestamp case.
For the global timestamp test-fixture, we created eight bytes of data representing the date/time “2017-10-05-14:15:44.425035200”.
As our timestamp-FB returns a string, this is exactly the string that we expect to get as a test-result.
You might be asking yourself "how on earth is it possible to know that 0x07C76560A71025C0 equals 2017-10-05-14:15:44.425035200"?
This can be accomplished by creating a little program that just prints the current actual DC-time by using [`F_GetActualDCTime64()`](https://infosys.beckhoff.com/english.php?content=../content/1033/tcplclib_tc2_ethercat/2268416395.html&id=) in combination with [`DCTIME64_TO_STRING()`](https://infosys.beckhoff.com/english.php?content=../content/1033/tcplclib_tc2_ethercat/2267406347.html&id=6169424304031718909).
Because the `T_DCTIME64`-type that is returned from `F_GetActualDcTime64()` is an alias for a primitive type, it's easy to convert it into a byte-array.
Note that the assertion of the local time stamp is based on getting the current DC-task time by utilizing the [`F_GetCurDcTaskTime64()`](https://infosys.beckhoff.com/index.php?content=../content/1031/tcplclib_tc2_ethercat/2268414091.html&id=), thus we're making sure that if the diagnosis message tells us that the timestamp is a local clock, we check that our FB returns this.

### FB_DiagnosticMessageParser_Test

The final test-FB that we need is the one that ties the bag together and uses all the other four.
The `FB_DiagnosticMessageParser` function block will be the one where we send in all the bytes that we receive from the IO-Link master, and that will output the struct that we can present to the operator or send further up in the chain.
One could argue that because we already have unit tests for the other four function blocks, we don’t need to have unit tests for this one.
By having unit tests for this "umbrella" function block, we add an additional level of confidence that our code is working properly.
Additionally, we can also make sure that combinations of different diagnosis messages are parsed correctly.

To have maximum variation we want to try to vary all parameters as much as possible.
Because it's quite a lot of code, it's highly recommended to look at the code in your development environment in parallell.
The code is available on [GitHub](https://github.com/tcunit/ExampleProjects/tree/master/AdvancedExampleProject).
We'll go through all the details, thus it should thus be easy for you to add any test cases that you find necessary.
As usual, header first:

```StructuredText
FUNCTION_BLOCK FB_DiagnosticMessageParser_Test EXTENDS TcUnit.FB_TestSuite
```

The function block ´FB_DiagnosticMessageParser is a very simple FB that compares every data element of the struct ´ST_DIAGNOSTICMESSAGE´, which we'll later use when we're doing the assertion.
We want to make sure to test various types of diagnostic messages, with their complete content. We'll write four tests:

- One test with an emergency message
- One test with a manufacturer specific message
- Two tests with unspecified messages (two variants)

Because this function uses the other four function blocks, we need to create a complete structure for every test with the complete content of a diagnosis message, making the tests prerequisites for every test quite large.
We'll start with the test **`TestWithEmergencyMessage`**.

```StructuredText
METHOD PRIVATE TestWithEmergencyMessage
VAR  
    fbDiagnosticMessageParser : FB_DiagnosticMessageParser;
    aDiagnosticMessageBuffer : ARRAY[1..28] OF BYTE;
    stDiagnosticMessage : ST_DIAGNOSTICMESSAGE;
 
    // @TEST-RESULT EmergencyMessage
    stDiagnosticMessage_EmergencyMessage : ST_DIAGNOSTICMESSAGE :=
        (stDiagnosticCode := (eDiagnosticCodeType := E_DIAGNOSTICCODETYPE.EmergencyErrorCodeDS301, nCode := 0),
        stFlags := (eDiagnosisType := E_DIAGNOSISTYPE.ErrorMessage, eTimeStampType := E_TIMESTAMPTYPE.Local,
                    nNumberOfParametersInDiagnosisMessage := 0),
        nTextIdentityReferenceToESIFile := 0,
        sTimeStamp := '');  // Local time stamp, will be updated in program call to current task time;
 
    // @TEST-FIXTURE EmergencyMessage
    cnDiagnosticBufferByte1_EmergencyMessage : BYTE := 16#00; // 0xE800 = Emergency code
    cnDiagnosticBufferByte2_EmergencyMessage : BYTE := 16#E8;
    cnDiagnosticBufferByte3_EmergencyMessage : BYTE := 16#00; // 0x0000 = Code 0
    cnDiagnosticBufferByte4_EmergencyMessage : BYTE := 16#00;
    cnDiagnosticBufferByte5_EmergencyMessage : BYTE := 2#0001_0010; // Local time stamp &amp;amp;amp; error message 
    cnDiagnosticBufferByte6_EmergencyMessage : BYTE := 16#00; // Number of parameters = 0
    cnDiagnosticBufferByte7_EmergencyMessage : BYTE := 16#00; // 0x0000, Text id as reference to ESI file = 0
    cnDiagnosticBufferByte8_EmergencyMessage : BYTE := 16#00;
    cnDiagnosticBufferByte9_16_EmergencyMessage : BYTE := 16#00; // Timestamp (none attached)
    cnDiagnosticBufferByte17_EmergencyMessage : BYTE := 2#0000_0010; // Param1 = Signed8
    cnDiagnosticBufferByte18_EmergencyMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte19_EmergencyMessage : BYTE := 2#0000_0011; // Port 3
    cnDiagnosticBufferByte20_EmergencyMessage : BYTE := 2#0000_0011; // Param2 = Signed16
    cnDiagnosticBufferByte21_EmergencyMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte22_EmergencyMessage : BYTE := 2#1110_1000; // EventCode = 10#1000
    cnDiagnosticBufferByte23_EmergencyMessage : BYTE := 2#0000_0011;
    cnDiagnosticBufferByte24_EmergencyMessage : BYTE := 2#0000_0010; // Param3 = Signed8
    cnDiagnosticBufferByte25_EmergencyMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte26_EmergencyMessage : BYTE := 2#0001_0111; // Qualifier: Instance=Reserved_7, Source=Device, Type=Notification, Mode=Reserved
    cnDiagnosticBufferByte27_EmergencyMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte28_EmergencyMessage : BYTE := 2#0000_0000;
 
    canDiagnosticBuffer_EmergencyMessage : ARRAY[1..28] OF BYTE := [cnDiagnosticBufferByte1_EmergencyMessage,
                                                                    cnDiagnosticBufferByte2_EmergencyMessage,
                                                                    cnDiagnosticBufferByte3_EmergencyMessage,
                                                                    cnDiagnosticBufferByte4_EmergencyMessage,
                                                                    cnDiagnosticBufferByte5_EmergencyMessage,
                                                                    cnDiagnosticBufferByte6_EmergencyMessage,
                                                                    cnDiagnosticBufferByte7_EmergencyMessage,
                                                                    cnDiagnosticBufferByte8_EmergencyMessage,
                                                                    8(cnDiagnosticBufferByte9_16_EmergencyMessage),
                                                                    cnDiagnosticBufferByte17_EmergencyMessage,
                                                                    cnDiagnosticBufferByte18_EmergencyMessage,
                                                                    cnDiagnosticBufferByte19_EmergencyMessage,
                                                                    cnDiagnosticBufferByte20_EmergencyMessage,
                                                                    cnDiagnosticBufferByte21_EmergencyMessage,
                                                                    cnDiagnosticBufferByte22_EmergencyMessage,
                                                                    cnDiagnosticBufferByte23_EmergencyMessage,
                                                                    cnDiagnosticBufferByte24_EmergencyMessage,
                                                                    cnDiagnosticBufferByte25_EmergencyMessage,
                                                                    cnDiagnosticBufferByte26_EmergencyMessage,
                                                                    cnDiagnosticBufferByte27_EmergencyMessage,
                                                                    cnDiagnosticBufferByte28_EmergencyMessage];
END_VAR
---------------------------------
TEST('TestWithEmergencyMessage');
 
// @TEST-RUN
stDiagnosticMessage_EmergencyMessage.sTimeStamp := DCTIME64_TO_STRING(in := F_GetCurDcTaskTime64());
 
fbDiagnosticMessageParser(anDiagnosticMessageBuffer := canDiagnosticBuffer_EmergencyMessage,
                          stDiagnosticMessage => stDiagnosticMessage);
 
// @TEST-ASSERT
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.stDiagnosticCode.eDiagnosticCodeType,
             Actual := stDiagnosticMessage.stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'EmergencyMessage$' failed at $'Diagnostic code type$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.stDiagnosticCode.nCode,
             Actual := stDiagnosticMessage.stDiagnosticCode.nCode,
             Message := 'Test $'EmergencyMessage$' failed at $'Diagnostic code$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.stFlags.eDiagnosisType,
             Actual := stDiagnosticMessage.stFlags.eDiagnosisType,
             Message := 'Test $'EmergencyMessage$' failed at $'Diagnosis type$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.stFlags.eTimeStampType,
             Actual := stDiagnosticMessage.stFlags.eTimeStampType,
             Message := 'Test $'EmergencyMessage$' failed at $'Timestamp type$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Actual := stDiagnosticMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'EmergencyMessage$' failed at $'Numbers of parameters in diagnosis message$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.nTextIdentityReferenceToESIFile,
             Actual := stDiagnosticMessage.nTextIdentityReferenceToESIFile,
             Message := 'Test $'EmergencyMessage$' failed at $'Text identity reference to ESI file$'');
AssertEquals(Expected := stDiagnosticMessage_EmergencyMessage.sTimeStamp,
             Actual := stDiagnosticMessage.sTimeStamp,
             Message := 'Test $'EmergencyMessage$' failed at $'Timestamp$'');
 
TEST_FINISHED();
```

Next up is testcase **`TestWithManufacturerSpecificMessage`**

```StructuredText
METHOD PRIVATE TestWithManufacturerSpecificMessage
VAR
    fbDiagnosticMessageParser : FB_DiagnosticMessageParser;
    aDiagnosticMessageBuffer : ARRAY[1..28] OF BYTE;
    stDiagnosticMessage : ST_DIAGNOSTICMESSAGE;
 
    // @TEST-FIXTURE ManufacturerSpecificMessage
    cnDiagnosticBufferByte1_ManufacturerSpecificMessage : BYTE := 16#90; // 0xE290 = Manufacturer Specific
    cnDiagnosticBufferByte2_ManufacturerSpecificMessage : BYTE := 16#E2;
    cnDiagnosticBufferByte3_ManufacturerSpecificMessage : BYTE := 16#30; // 0x0000 = Code 0
    cnDiagnosticBufferByte4_ManufacturerSpecificMessage : BYTE := 16#75;
    cnDiagnosticBufferByte5_ManufacturerSpecificMessage : BYTE := 2#0000_0000; // Global time stamp &amp;amp;amp; info message 
    cnDiagnosticBufferByte6_ManufacturerSpecificMessage : BYTE := 16#02; // Number of parameters = 2
    cnDiagnosticBufferByte7_ManufacturerSpecificMessage : BYTE := 16#A8; // 0x61A8, Text id as reference to ESI file = 10#25000
    cnDiagnosticBufferByte8_ManufacturerSpecificMessage : BYTE := 16#61;
    cnDiagnosticBufferByte9_ManufacturerSpecificMessage : BYTE := 16#C8; // Timestamp from DC clock, 16#07C8D11492616FC8 = '2017-10-10-05:20:39.893037000'
    cnDiagnosticBufferByte10_ManufacturerSpecificMessage : BYTE := 16#6F;
    cnDiagnosticBufferByte11_ManufacturerSpecificMessage : BYTE := 16#61;
    cnDiagnosticBufferByte12_ManufacturerSpecificMessage : BYTE := 16#92;
    cnDiagnosticBufferByte13_ManufacturerSpecificMessage : BYTE := 16#14;
    cnDiagnosticBufferByte14_ManufacturerSpecificMessage : BYTE := 16#D1;
    cnDiagnosticBufferByte15_ManufacturerSpecificMessage : BYTE := 16#C8;
    cnDiagnosticBufferByte16_ManufacturerSpecificMessage : BYTE := 16#07;
    cnDiagnosticBufferByte17_ManufacturerSpecificMessage : BYTE := 2#0000_0010; // Param1 = Signed8
    cnDiagnosticBufferByte18_ManufacturerSpecificMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte19_ManufacturerSpecificMessage : BYTE := 2#0000_0110; // Port 6
    cnDiagnosticBufferByte20_ManufacturerSpecificMessage : BYTE := 2#0000_0011; // Param2 = Signed16
    cnDiagnosticBufferByte21_ManufacturerSpecificMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte22_ManufacturerSpecificMessage : BYTE := 2#1101_0010; // EventCode = 10#1234
    cnDiagnosticBufferByte23_ManufacturerSpecificMessage : BYTE := 2#0000_0100;
    cnDiagnosticBufferByte24_ManufacturerSpecificMessage : BYTE := 2#0000_0010; // Param3 = Signed8
    cnDiagnosticBufferByte25_ManufacturerSpecificMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte26_ManufacturerSpecificMessage : BYTE := 2#0100_0110; // Qualifier: Instance=Reserved_6, Source=Device, Type=Reserved, Mode=EventSingleShot
    cnDiagnosticBufferByte27_ManufacturerSpecificMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte28_ManufacturerSpecificMessage : BYTE := 2#0000_0000;
 
    canDiagnosticBuffer_ManufacturerSpecificMessage : ARRAY[1..28] OF BYTE := [
                                                               cnDiagnosticBufferByte1_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte2_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte3_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte4_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte5_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte6_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte7_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte8_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte9_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte10_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte11_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte12_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte13_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte14_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte15_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte16_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte17_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte18_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte19_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte20_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte21_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte22_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte23_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte24_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte25_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte26_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte27_ManufacturerSpecificMessage,
                                                               cnDiagnosticBufferByte28_ManufacturerSpecificMessage];
 
    // @TEST-RESULT ManufacturerSpecificMessage
    cstDiagnosticMessage_ManufacturerSpecificMessage : ST_DIAGNOSTICMESSAGE :=
        (stDiagnosticCode := (eDiagnosticCodeType := E_DIAGNOSTICCODETYPE.ManufacturerSpecific, nCode := 30000),
        stFlags := (eDiagnosisType := E_DIAGNOSISTYPE.InfoMessage, eTimeStampType := E_TIMESTAMPTYPE.Global,
                    nNumberOfParametersInDiagnosisMessage := 2),
        nTextIdentityReferenceToESIFile := 25000,
        sTimeStamp := '2017-10-10-05:20:39.893037000');
END_VAR
--------------------------------------------
TEST('TestWithManufacturerSpecificMessage');
 
// @TEST-RUN
fbDiagnosticMessageParser(anDiagnosticMessageBuffer := canDiagnosticBuffer_ManufacturerSpecificMessage,
                          stDiagnosticMessage => stDiagnosticMessage);
 
// @TEST-ASSERT
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.stDiagnosticCode.eDiagnosticCodeType,
             Actual := stDiagnosticMessage.stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Diagnostic code type$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.stDiagnosticCode.nCode,
             Actual := stDiagnosticMessage.stDiagnosticCode.nCode,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Diagnostic code$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.stFlags.eDiagnosisType,
             Actual := stDiagnosticMessage.stFlags.eDiagnosisType,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Diagnosis type$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.stFlags.eTimeStampType,
             Actual := stDiagnosticMessage.stFlags.eTimeStampType,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Timestamp type$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Actual := stDiagnosticMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Numbers of parameters in diagnosis message$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.nTextIdentityReferenceToESIFile,
             Actual := stDiagnosticMessage.nTextIdentityReferenceToESIFile,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Text identity reference to ESI file$'');
AssertEquals(Expected := cstDiagnosticMessage_ManufacturerSpecificMessage.sTimeStamp,
             Actual := stDiagnosticMessage.sTimeStamp,
             Message := 'Test $'ManufacturerSpecificMessage$' failed at $'Timestamp$'');
 
TEST_FINISHED();
```

And finally two test cases where the diagnosis type is unspecified.

```StructuredText
METHOD PRIVATE TestWithUnspecifiedMessageMessage
VAR
    fbDiagnosticMessageParser : FB_DiagnosticMessageParser;
    aDiagnosticMessageBuffer : ARRAY[1..28] OF BYTE;
    stDiagnosticMessage : ST_DIAGNOSTICMESSAGE;
 
    // @TEST-FIXTURE UnspecifiedMessage
    cnDiagnosticBufferByte1_UnspecifiedMessage : BYTE := 16#01; // 0xE801 = Reserved for future use
    cnDiagnosticBufferByte2_UnspecifiedMessage : BYTE := 16#E8;
    cnDiagnosticBufferByte3_UnspecifiedMessage : BYTE := 16#FF; // 0xFFFF = Code 65535
    cnDiagnosticBufferByte4_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte5_UnspecifiedMessage : BYTE := 2#0000_0001; // Global time stamp &amp;amp;amp; warning message 
    cnDiagnosticBufferByte6_UnspecifiedMessage : BYTE := 16#FF; // Number of parameters = 255
    cnDiagnosticBufferByte7_UnspecifiedMessage : BYTE := 16#FF; // 0x61A8, Text id as reference to ESI file = 10#65535
    cnDiagnosticBufferByte8_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte9_UnspecifiedMessage : BYTE := 16#FF; // Timestamp from DC clock, 16#FFFFFFFFFFFFFFFF = '2584-07-20-23:34:33.709551615'
    cnDiagnosticBufferByte10_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte11_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte12_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte13_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte14_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte15_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte16_UnspecifiedMessage : BYTE := 16#FF;
    cnDiagnosticBufferByte17_UnspecifiedMessage : BYTE := 2#0000_0010; // Param1 = Signed8
    cnDiagnosticBufferByte18_UnspecifiedMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte19_UnspecifiedMessage : BYTE := 2#0000_0101; // Port 5
    cnDiagnosticBufferByte20_UnspecifiedMessage : BYTE := 2#0000_0011; // Param2 = Signed16
    cnDiagnosticBufferByte21_UnspecifiedMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte22_UnspecifiedMessage : BYTE := 2#0100_0100; // EventCode = 10#65092
    cnDiagnosticBufferByte23_UnspecifiedMessage : BYTE := 2#1111_1110;
    cnDiagnosticBufferByte24_UnspecifiedMessage : BYTE := 2#0000_0010; // Param3 = Signed8
    cnDiagnosticBufferByte25_UnspecifiedMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte26_UnspecifiedMessage : BYTE := 2#1101_1111; // Qualifier: Instance=Reserved_7, Source=Master, Type=Notification, Mode=EventAppears
    cnDiagnosticBufferByte27_UnspecifiedMessage : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte28_UnspecifiedMessage : BYTE := 2#0000_0000;
 
    canDiagnosticBuffer_UnspecifiedMessage : ARRAY[1..28] OF BYTE := [
                                                               cnDiagnosticBufferByte1_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte2_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte3_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte4_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte5_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte6_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte7_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte8_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte9_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte10_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte11_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte12_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte13_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte14_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte15_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte16_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte17_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte18_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte19_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte20_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte21_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte22_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte23_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte24_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte25_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte26_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte27_UnspecifiedMessage,
                                                               cnDiagnosticBufferByte28_UnspecifiedMessage];
 
    // @TEST-RESULT UnspecifiedMessage
    cstDiagnosticMessage_UnspecifiedMessage : ST_DIAGNOSTICMESSAGE :=
        (stDiagnosticCode := (eDiagnosticCodeType := E_DIAGNOSTICCODETYPE.Unspecified, nCode := 65535),
        stFlags := (eDiagnosisType := E_DIAGNOSISTYPE.WarningMessage, eTimeStampType := E_TIMESTAMPTYPE.Global,
                    nNumberOfParametersInDiagnosisMessage := 255),
        nTextIdentityReferenceToESIFile := 65535,
        sTimeStamp := '2584-07-20-23:34:33.709551615');
END_VAR
------------------------------------------
TEST('TestWithUnspecifiedMessageMessage');
 
// @TEST-RUN
fbDiagnosticMessageParser(anDiagnosticMessageBuffer := canDiagnosticBuffer_UnspecifiedMessage,
                          stDiagnosticMessage => stDiagnosticMessage);
 
// @TEST-ASSERT
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.stDiagnosticCode.eDiagnosticCodeType,
             Actual := stDiagnosticMessage.stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Diagnostic code type$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.stDiagnosticCode.nCode,
             Actual := stDiagnosticMessage.stDiagnosticCode.nCode,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Diagnostic code$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.stFlags.eDiagnosisType,
             Actual := stDiagnosticMessage.stFlags.eDiagnosisType,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Diagnosis type$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.stFlags.eTimeStampType,
             Actual := stDiagnosticMessage.stFlags.eTimeStampType,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Timestamp type$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Actual := stDiagnosticMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Numbers of parameters in diagnosis message$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.nTextIdentityReferenceToESIFile,
             Actual := stDiagnosticMessage.nTextIdentityReferenceToESIFile,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Text identity reference to ESI file$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage.sTimeStamp,
             Actual := stDiagnosticMessage.sTimeStamp,
             Message := 'Test $'UnspecifiedMessageMessage$' failed at $'Timestamp$'');
 
TEST_FINISHED();
```

And a second variant with some different input parameters.

```StructuredText
METHOD PRIVATE TestWithUnspecifiedMessageMessage_ParameterVariant
VAR
    fbDiagnosticMessageParser : FB_DiagnosticMessageParser;
    aDiagnosticMessageBuffer : ARRAY[1..28] OF BYTE;
    stDiagnosticMessage : ST_DIAGNOSTICMESSAGE;
 
    // @TEST-FIXTURE UnspecifiedMessage_ParameterVariant
    cnDiagnosticBufferByte1_UnspecifiedMessage_ParameterVariant : BYTE := 16#01; // 0xE801 = Reserved for future use
    cnDiagnosticBufferByte2_UnspecifiedMessage_ParameterVariant : BYTE := 16#E8;
    cnDiagnosticBufferByte3_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF; // 0xFFFF = Code 65535
    cnDiagnosticBufferByte4_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte5_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0001; // Global time stamp &amp;amp;amp; warning message 
    cnDiagnosticBufferByte6_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF; // Number of parameters = 255
    cnDiagnosticBufferByte7_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF; // 0x61A8, Text id as reference to ESI file = 10#65535
    cnDiagnosticBufferByte8_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte9_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF; // Timestamp from DC clock, 16#FFFFFFFFFFFFFFFF = '2584-07-20-23:34:33.709551615'
    cnDiagnosticBufferByte10_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte11_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte12_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte13_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte14_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte15_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte16_UnspecifiedMessage_ParameterVariant : BYTE := 16#FF;
    cnDiagnosticBufferByte17_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0010; // Param1 = Signed8
    cnDiagnosticBufferByte18_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte19_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0101; // Port 5
    cnDiagnosticBufferByte20_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0100; // Param2 = Signed32
    cnDiagnosticBufferByte21_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte22_UnspecifiedMessage_ParameterVariant : BYTE := 2#1111_1111; // EventCode = 10#‭4294967295‬ (though will be interpreted as maximum 16 bits = 65535)
    cnDiagnosticBufferByte23_UnspecifiedMessage_ParameterVariant : BYTE := 2#1111_1111;
    cnDiagnosticBufferByte24_UnspecifiedMessage_ParameterVariant : BYTE := 2#1111_1111;
    cnDiagnosticBufferByte25_UnspecifiedMessage_ParameterVariant : BYTE := 2#1111_1111;
    cnDiagnosticBufferByte26_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0010; // Param3 = Signed8
    cnDiagnosticBufferByte27_UnspecifiedMessage_ParameterVariant : BYTE := 2#0000_0000;
    cnDiagnosticBufferByte28_UnspecifiedMessage_ParameterVariant : BYTE := 2#1101_1111; // Qualifier: Instance=Reserved_7, Source=Master, Type=Notification, Mode=EventAppears
 
    canDiagnosticBuffer_UnspecifiedMessage_ParameterVariant : ARRAY[1..28] OF BYTE := [
                                                        cnDiagnosticBufferByte1_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte2_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte3_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte4_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte5_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte6_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte7_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte8_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte9_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte10_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte11_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte12_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte13_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte14_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte15_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte16_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte17_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte18_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte19_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte20_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte21_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte22_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte23_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte24_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte25_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte26_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte27_UnspecifiedMessage_ParameterVariant,
                                                        cnDiagnosticBufferByte28_UnspecifiedMessage_ParameterVariant];
 
    // @TEST-RESULT UnspecifiedMessageParameterVariant
    cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant : ST_DIAGNOSTICMESSAGE :=
        (stDiagnosticCode := (eDiagnosticCodeType := E_DIAGNOSTICCODETYPE.Unspecified, nCode := 65535),
        stFlags := (eDiagnosisType := E_DIAGNOSISTYPE.WarningMessage, eTimeStampType := E_TIMESTAMPTYPE.Global,
                    nNumberOfParametersInDiagnosisMessage := 255),
        nTextIdentityReferenceToESIFile := 65535,
        sTimeStamp := '2584-07-20-23:34:33.709551615');
END_VAR
-----------------------------------------------------------
TEST('TestWithUnspecifiedMessageMessage_ParameterVariant');
 
// @TEST-RUN
fbDiagnosticMessageParser(anDiagnosticMessageBuffer := canDiagnosticBuffer_UnspecifiedMessage_ParameterVariant,
                          stDiagnosticMessage => stDiagnosticMessage);
 
// @TEST-ASSERT
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.stDiagnosticCode.eDiagnosticCodeType,
             Actual := stDiagnosticMessage.stDiagnosticCode.eDiagnosticCodeType,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Diagnostic code type$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.stDiagnosticCode.nCode,
             Actual := stDiagnosticMessage.stDiagnosticCode.nCode,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Diagnostic code$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.stFlags.eDiagnosisType,
             Actual := stDiagnosticMessage.stFlags.eDiagnosisType,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Diagnosis type$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.stFlags.eTimeStampType,
             Actual := stDiagnosticMessage.stFlags.eTimeStampType,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Timestamp type$'');
fAssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.stFlags.nNumberOfParametersInDiagnosisMessage,
              Actual := stDiagnosticMessage.stFlags.nNumberOfParametersInDiagnosisMessage,
              Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Numbers of parameters in diagnosis message$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.nTextIdentityReferenceToESIFile,
             Actual := stDiagnosticMessage.nTextIdentityReferenceToESIFile,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Text identity reference to ESI file$'');
AssertEquals(Expected := cstDiagnosticMessage_UnspecifiedMessage_ParameterVariant.sTimeStamp,
             Actual := stDiagnosticMessage.sTimeStamp,
             Message := 'Test $'UnspecifiedMessageMessage_ParameterVariant$' failed at $'Timestamp$'');
 
TEST_FINISHED();
```

Every byte has an accompanying comment so that it'’'s obvious what information is being stored in every byte.
Note that the test-result is a structured representation of all the diagnostic history message bytes.
These 16 bytes are the sum of all the other bytes used for the other four function block parsers.

**That's our tests!**  
We’re finished with the unit tests. Notice that we still have not written a single line of code for the implementing part.
We’ve defined the inputs/outputs and the accompanying data structures for our function blocks.
We’ve also created all the test cases.
All of the code you’ve written so far is **excellent documentation** for any other developer that would try to understand the implementing code in the future.
Not only that, all your test cases also form the **acceptance criteria** for the implementation code.
You’ve basically said "I require my code to pass these tests, and these tests must pass for my code to do what I want it to do".
Note that all the tests above can be executed at anytime.
Done any change to your code?
Just re-run the tests and make sure you haven’t broken anything.
Fantastic, isn’t it?

We've written a total of 17 tests, so now let’s build the project and run the tests.

![TcUnit many fails](img/TcUnitManyFails.png)

This is just a selection of all the failed asserts.
For every failed assert, we can see the expected value we should have got in case the implementing code did what it is supposed to do and also the actual value as well as the message that we provided to the assert.
The statistics are printed a little bit further down:

![TcUnit 16 of 17 failed](img/TcUnit16Of17Failed_2.png)

First we can see that we have had five test suites running, in where each had a certain amount of tests defined.
Every test suite is responsible to test a specific function block.
We can see that all tests except one has failed.
But how come that we’ve had a successful test even though we haven’t yet written a single line of code?
This is usually related to tests that test some zero-values, where the default return value of the function block under test is zero.
In this case it is this test:

```StructuredText
METHOD PRIVATE WhenTextIdentityLowExpectTextIdentity0
VAR
    fbDiagnosticMessageTextIdentityParser : FB_DiagnosticMessageTextIdentityParser;
    nTextIdentity : UINT;
 
    // @TEST-FIXTURE TextIdentity#Low
    cnTextIdentityBufferByte1_IdentityLow : BYTE := 16#00; // 0 = no text identity
    cnTextIdentityBufferByte2_IdentityLow : BYTE := 16#00;
    canTextIdentityBuffer_IdentityLow : ARRAY[1..2] OF BYTE := [cnTextIdentityBufferByte1_IdentityLow,
                                                                cnTextIdentityBufferByte2_IdentityLow];
    // @TEST-RESULT TextIdentity#Low
    cnTextIdentity_IdentityLow : UINT := 0;
END_VAR
```

We're testing the function block `FB_DiagnosticMessageTextIdentityParser` by providing it with a zero-value as input (two bytes, each holding `0x00`) and expecting the number 0 as result.
The default initialization of a number value in TwinCAT is always zero, and thus this is returned which makes our test succeed.
Tests that pass without implementing code generally don't provide much value.
