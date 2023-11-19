# Test cases - Part two

<p align="center">
  <img width="1024" src="./img/tc3_banner4.jpg">
</p>

## FB_DiagnosticMessageFlagsParser_Test

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

![Flags layout](img/flagslayout.png)

Let's write four tests and call them:

- `WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`
- `WhenInfoMessageExpectInfoMessageGlobalTimestampAndZeroParameters`
- `WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters`
- `WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters`  

It's a good idea to have descriptive names about what the test is testing and what the expected result is.
In this way any developer reading the tests can clearly understand the goal of the tests.
With all this information other developers get a lot of documentation for free!

**Test `WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`**

```body
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

**Test `WhenErrorMessageExpectErrorMessageLocalTimestampAndFourParameters`**

```body
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

**Test `WhenReservedForFutureUseMessageExpectReservedForFutureUseMessageLocalTimestampAnd33Parameters`**

```body
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

**Test `WhenWarningMessageExpectWarningMessageLocalTimestampAndTwoParameters`**

```body
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

```delcaration
FUNCTION_BLOCK FB_DiagnosticMessageFlagsParser_Test EXTENDS TcUnit.FB_TestSuite
```

And as usual, we need to add a call to all the test-methods in the body of the test suite.

```body
TestWithEmergencyMessage();
TestWithManufacturerSpecificMessage();
TestWithUnspecifiedMessageMessage();
TestWithUnspecifiedMessageMessage_ParameterVariant();
```

What we've got left is to create test cases for the parsing of the text identity and the timestamp of the diagnostic event.
Then we also want to have a few tests that closes the loop and verifies the parsing of a complete diagnosis history message.

## FB_DiagnosticMessageTextIdentityParser_Test

The only input for the text identity are two bytes that together make up an unsigned integer (0-65535), which is the result (output) of this parser.
It's enough to make three test cases; one for low/medium/max.
We accomplish to test the three values by changing the two bytes that make up the unsigned integer.
The header of the test suite:

```declaration
FUNCTION_BLOCK FB_DiagnosticMessageTextIdentityParser_Test EXTENDS TcUnit.FB_TestSuite
```

We'll write the tests Low/Med/High, testing for the different inputs 0, 34500 and 65535.

**Test `WhenTextIdentityLowExpectTextIdentity0`**

```body
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

**Test `WhenTextIdentityMedExpectTextIdentity34500`**

```body
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

**Test `WhenTextIdentityHighExpectTextIdentity65535`**

```body
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
```

As can be seen the only thing that varies between the tests (other than name) is the different inputs and expected output.

### FB_DiagnosticMessageTimeStampParser_Test

The eight bytes that make up the timestamp can be either the distributed clock (DC) from EtherCAT, or a local clock in the device itself.
In the global case we want to parse the DC-time, while in the local case we just want to take the DC from the current task time (the local clock could be extracted from the EtherCAT-slave, but for the sake of simplicity we'll use the task DC).
Because the local/global-flag is read from the "Flags"-FB, this information needs to be provided into the timestamp-FB, and is therefore an input to the FB.
What this means is that if the timestamp is local, the eight bytes don't matter as we'll get the time from the task.
For the timestamp-FB it's enough with two test cases, one testing it with a local timestamp and the other with a global timestamp.
The local timestamp unit test result has to be created in runtime.

```declaration
FUNCTION_BLOCK FB_DiagnosticMessageTimeStampParser_Test EXTENDS TcUnit.FB_TestSuite
```

Let's create our tests, and start with the test * **`TestWithTimestampZeroTimeExpectCurrentTime`**.

```body
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

**Test `TestWithValidTimestampExpectSameTimestamp`**

```body
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

## FB_DiagnosticMessageParser_Test

The final test-FB that we need is the one that ties the bag together and uses all the other four.
The `FB_DiagnosticMessageParser` function block will be the one where we send in all the bytes that we receive from the IO-Link master, and that will output the struct that we can present to the operator or send further up in the chain.
One could argue that because we already have unit tests for the other four function blocks, we don't need to have unit tests for this one.
By having unit tests for this "umbrella" function block, we add an additional level of confidence that our code is working properly.
Additionally, we can also make sure that combinations of different diagnosis messages are parsed correctly.

To have maximum variation we want to try to vary all parameters as much as possible.
Because it's quite a lot of code, it's highly recommended to look at the code in your development environment in parallell.
The code is available on [GitHub](https://github.com/tcunit/ExampleProjects/tree/master/AdvancedExampleProject).
We'll go through all the details, thus it should thus be easy for you to add any test cases that you find necessary.
As usual, header first:

```declaration
FUNCTION_BLOCK FB_DiagnosticMessageParser_Test EXTENDS TcUnit.FB_TestSuite
```

The function block ´FB_DiagnosticMessageParser is a very simple FB that compares every data element of the struct ´ST_DIAGNOSTICMESSAGE´, which we'll later use when we're doing the assertion.
We want to make sure to test various types of diagnostic messages, with their complete content. We'll write four tests:

- One test with an emergency message
- One test with a manufacturer specific message
- Two tests with unspecified messages (two variants)

Because this function uses the other four function blocks, we need to create a complete structure for every test with the complete content of a diagnosis message, making the tests prerequisites for every test quite large.
We'll start with the test **`TestWithEmergencyMessage`**.

```body
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

```body
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

```body
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

```body
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

Every byte has an accompanying comment so that it's obvious what information is being stored in every byte.
Note that the test-result is a structured representation of all the diagnostic history message bytes.
These 16 bytes are the sum of all the other bytes used for the other four function block parsers.

## That's our tests

We're finished with the unit tests. Notice that we still have not written a single line of code for the implementing part.
We've defined the inputs/outputs and the accompanying data structures for our function blocks.
We've also created all the test cases.
All of the code you've written so far is **excellent documentation** for any other developer that would try to understand the implementing code in the future.
Not only that, all your test cases also form the **acceptance criteria** for the implementation code.
You've basically said "I require my code to pass these tests, and these tests must pass for my code to do what I want it to do".
Note that all the tests above can be executed at anytime.
Done any change to your code?
Just re-run the tests and make sure you haven't broken anything.
Fantastic, isn't it?

We've written a total of 17 tests, so now let's build the project and run the tests.

![TcUnit many fails](img/TcUnitManyFails.png)

This is just a selection of all the failed asserts.
For every failed assert, we can see the expected value we should have got in case the implementing code did what it is supposed to do and also the actual value as well as the message that we provided to the assert.
The statistics are printed a little bit further down:

![TcUnit 16 of 17 failed](img/TcUnit16Of17Failed_2.png)

First we can see that we have had five test suites running, in where each had a certain amount of tests defined.
Every test suite is responsible to test a specific function block.
We can see that all tests except one has failed.
But how come that we've had a successful test even though we haven't yet written a single line of code?
This is usually related to tests that test some zero-values, where the default return value of the function block under test is zero.
In this case it is this test:

```body
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
