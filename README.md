# TcUnit-Verifier
This project is can be used to verify the different functions of [TcUnit](https://www.github.com/tcunit/TcUnit).
It has various test suites to test different functions in TcUnit. It consists of two separate programs:
- TcUnit-Verifier_TwinCAT
- TcUnit-Verifier_DotNet

When verifying that TcUnit works as expected a developer needs to add tests to both of these programs.

![TcUnit-Verifier concept](https://github.com/tcunit/TcUnit-Verifier/blob/master/img/TcUnit-Verifier_Concept_1280.png)

## TcUnit-Verifier_TwinCAT
TcUnit-Verifier_TwinCAT (TcVT) is as the name implies a TwinCAT project. It holds a reference to and instantiates
the TcUnit framework. It is defined by several test suites (function blocks) that make use of TcUnit and its different
functionalitites. An example of a test is to verify that TcUnit prints a message if an assertion of an
INT has failed (expected value not equals to asserted value). This means per definition that the verifier
will have failed tests as results, because that is what we want to test. Everytime a test suite is created
in TcVT that tests the functionality of TcUnit it is expected that either:
- Some specific output is created by TcUnit (error-log), for example asserting a value that differs from the expected value
- Some specific output is **not** created by TcUnit (error-log), for example asserting a value that is equal to the expected value

Creating a test suite for TcUnit thus requires that the developer needs to know what TcUnit should print (or not print)
This means that running this TwinCAT project is not enough to know whether TcUnit behaves as expected, as it would
require to manually check all the output from TcVT. Because we don't manually want to check that the output provided by
TcUnit-Verifier_TwinCAT everytime it's running, another program is necessary that verifies that the output of the TcVT
is as expected.

## TcUnit-Verifier_DotNet
The TcUnit-Verifier_DotNet (TcVD) is a C# program that opens and runs the TcUnit-Verifier_TwinCAT project by the usage
of the TwinCAT automation interface. It basically does the following:
- Starts Visual Studio (using the same version that was used developing TcVT)
- Opens TcVT
- Does a clean/build of TcVT
- Generates a boot project for TcVT on local machine
- Activates TcVT on local machine
- Restarts TwinCAT on local machine
- Waits for all tests in TcVT to be finished
- Collects all results from the error list in Visual Studio
- Checks that the output is as defined/expected


