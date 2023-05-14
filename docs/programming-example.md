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

```
TYPE ST_DIAGNOSTICCODE :
STRUCT
    eDiagnosticCodeType : E_DIAGNOSTICCODETYPE;
    nCode : UINT;
END_STRUCT
END_TYPE
```

where the `E_DIAGNOSTICCODETYPE` is

```
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

```
TYPE ST_FLAGS :
STRUCT
    eDiagnostisType : E_DIAGNOSISTYPE;
    eTimeStampType : E_TIMESTAMPTYPE;
    nNumberOfParametersInDiagnosisMessage : USINT;
END_STRUCT
END_TYPE
```

Where `E_DiagnosisType` and `E_TimeStampType` are respectively:

```
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

```
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

```
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
```
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
```
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
```
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
```
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
```
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

```
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

```
FUNCTION_BLOCK FB_DiagnosticMessageDiagnosticCodeParser_Test EXTENDS TcUnit.FB_TestSuite
```

This is the required layout of the header of a test suite just as described in the [introduction user guide](introduction-user-guide.md).

Our next step is setting up all test fixtures and the expected test results.
Here we need to think about the different test cases that we would like to run.
We want to make sure our function block correctly parses the three different diagnostic code types, but we also want to make sure our code correctly handles the case when the IO-Link master for instance outputs a diagnosis code type that’s not valid (Reserved for future use for instance). In this case we want to make sure our function block sets it to “Unspecified”. To make sure that our code handles all these different cases, we want four good tests.

The first test will represent an emergency code, and the header of the method defining the test will look like follows:

```
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

```
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
```
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

```
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
```
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

```
WhenEmergencyErrorCodeExpectEmergencyErrorCode();
WhenManufacturerSpecificExpectManufacturerSpecific();
WhenProfileSpecificExpectProfileSpecific();
WhenReservedForFutureUseExpectReservedForFutureUse();
```