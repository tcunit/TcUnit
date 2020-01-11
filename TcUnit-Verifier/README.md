# TcUnit-Verifier
This project is can be used to verify the different functions of
[TcUnit](https://www.github.com/tcunit/TcUnit). It has various test suites to
test different functions in TcUnit. It consists of two separate programs:
- TcUnit-Verifier_TwinCAT
- TcUnit-Verifier_DotNet

When verifying that TcUnit works as expected a developer needs to add tests to
both of these programs.

![TcUnit-Verifier concept](https://github.com/tcunit/TcUnit/blob/master/TcUnit-Verifier/img/TcUnit-Verifier_Concept_1280.png)

## TcUnit-Verifier_TwinCAT
TcUnit-Verifier_TwinCAT (TcVT) is as the name implies a TwinCAT project.
It holds a reference to and instantiates the TcUnit framework. It is defined by
several test suites (function blocks) that make use of TcUnit and its different
functionalitites. An example of a test is to verify that TcUnit prints a message
if an assertion of an INT has failed (expected value not equals to
asserted value). This means per definition that the verifier will have failed
tests as results, because that is what we want to test. Everytime a test suite
is created in TcVT that tests the functionality of TcUnit it is expected
that either:
- Some specific output is created by TcUnit (error-log), for example asserting a
  value that differs from the expected value
- Some specific output is **not** created by TcUnit (error-log), for example
  asserting a value that is equal to the expected value

Creating a test suite for TcUnit thus requires that the developer needs to know
what TcUnit should print (or not print). This means that running this TwinCAT
project is not enough to know whether TcUnit behaves as expected, as it would
require to manually check all the output from TcVT. Because we don't manually
want to check that the output provided by TcUnit-Verifier_TwinCAT everytime it's
running, another program is necessary that verifies that the output of the TcVT
is as expected.

## TcUnit-Verifier_DotNet
The TcUnit-Verifier_DotNet (TcVD) is a (Visual Studio 2013) C# program that opens
and runs the TcUnit-Verifier_TwinCAT project by the usage of the TwinCAT automation
interface. It basically does the following:
- Starts Visual Studio (using the same version that was used developing TcVT)
- Opens TcVT
- Does a clean/build of TcVT
- Generates a boot project for TcVT on local machine
- Activates TcVT on local machine
- Restarts TwinCAT on local machine
- Waits for all tests in TcVT to be finished
- Collects all results from the error list in Visual Studio
- Checks that the output is as defined/expected

The different tests in TcVD follow the same naming-schema as for the TcVT tests
so for example you can find a structured text (ST) function block (FB)
`FB_PrimitiveTypes.TcPOU` in TcVT and the accompanying C#-class
`FB_PrimitiveTypes.cs`. The C# classes verify and test that the output from the
ST function blocks is correct. Thus a complete test of a specific function in
TcUnit needs to be developed in pair, both a FB of tests needs to be added to
TcVT as well a class of tests needs to be added to TcVD.

All test classes are instantiated in the class `Program.cs` starting from the
lines:
```
/* Insert the test classes here */
new FB_PrimitiveTypes(errors);
new FB_AssertTrueFalse(errors);
new FB_AssertEveryFailedTestTwice(errors);
...
...
...
```

To create a new test class and make sure that it will be running all that is
necessary is to make sure to instantiate it with the argument `errors`
(just as above). If you have added a test in TcVT that is supposed to fail, and
thus adding an additional failed test to the output, you need to increment the
variable `expectedNumberOfFailedTests` in TcVD by one for every failed test
that you have added. 

For example, if we in the PRG_TEST-program of TcVT have a function block
instantiated in this way:
```
PROGRAM PRG_TEST
VAR
    AssertEveryFailedTestTwiceArrayVersion : FB_AssertEveryFailedTestTwiceArrayVersion;
END_VAR
```
The equivalent test class in TcVD needs to be instantiated in the following way:
```
new FB_AssertEveryFailedTestTwiceArrayVersion(errors);
```

This is an example of how it can look running the TcUnit-Verifier_DotNet:

```
C:\Code\GitHub_TcUnit\TcUnit\TcUnit-Verifier\TcUnit-Verifier_DotNet\TcUnit-Verifier\bin\Debug>TcUnit-Verifier.exe -v "C:\Code\GitHub_TcUnit\TcUnit\TcUnit-Verifier\TcUnit-Verifier_TwinCAT\TcUnit-Verifier_TwinCAT.sln"
2020-01-11 21:26:52 - Starting TcUnit-Verifier...
2020-01-11 21:26:52 - Loading the Visual Studio Development Tools Environment (DTE)...
2020-01-11 21:27:02 - Cleaning and building TcUnit-Verifier_TwinCAT solution...
2020-01-11 21:27:02 - Generating TcUnit-Verifier_TwinCAT boot project...
2020-01-11 21:27:08 - Activating TcUnit-Verifier_TwinCAT configuration...
2020-01-11 21:27:10 - Restarting TwinCAT...
2020-01-11 21:27:10 - Waiting for TcUnit-Verifier_TwinCAT to finish running tests...
2020-01-11 21:27:11 - ... got 5 report lines so far.
2020-01-11 21:27:12 - ... got 5 report lines so far.
2020-01-11 21:27:13 - ... got 29 report lines so far.
2020-01-11 21:27:14 - ... got 79 report lines so far.
2020-01-11 21:27:15 - ... got 129 report lines so far.
2020-01-11 21:27:16 - ... got 179 report lines so far.
2020-01-11 21:27:17 - ... got 229 report lines so far.
2020-01-11 21:27:18 - ... got 279 report lines so far.
2020-01-11 21:27:19 - ... got 354 report lines so far.
2020-01-11 21:27:21 - ... got 404 report lines so far.
2020-01-11 21:27:22 - ... got 454 report lines so far.
2020-01-11 21:27:23 - ... got 504 report lines so far.
2020-01-11 21:27:24 - ... got 554 report lines so far.
2020-01-11 21:27:25 - ... got 604 report lines so far.
2020-01-11 21:27:26 - ... got 654 report lines so far.
2020-01-11 21:27:27 - ... got 704 report lines so far.
2020-01-11 21:27:28 - ... got 779 report lines so far.
2020-01-11 21:27:29 - ... got 829 report lines so far.
2020-01-11 21:27:30 - ... got 879 report lines so far.
2020-01-11 21:27:31 - ... got 929 report lines so far.
2020-01-11 21:27:32 - ... got 979 report lines so far.
2020-01-11 21:27:33 - ... got 1029 report lines so far.
2020-01-11 21:27:34 - ... got 1079 report lines so far.
2020-01-11 21:27:35 - ... got 1154 report lines so far.
2020-01-11 21:27:37 - ... got 1180 report lines so far.
2020-01-11 21:27:37 - Asserting results...
2020-01-11 21:27:37 - Done.
2020-01-11 21:27:37 - Closing the Visual Studio Development Tools Environment (DTE), please wait...
2020-01-11 21:27:59 - Exiting application...
```
If there was an error in the TcUnit framework this would be shown between the
lines `Asserting results...` and `Done.`. If nothing is shown between these
lines (like in the example) it means that TcUnit behaves according to the tests
defined in TcUnit-Verifier. As TcVD starts the TwinCAT runtime it is required 
that a (trial) runtime license for TwinCAT is activated on the PC of where TcVD
and TcVT will be running.