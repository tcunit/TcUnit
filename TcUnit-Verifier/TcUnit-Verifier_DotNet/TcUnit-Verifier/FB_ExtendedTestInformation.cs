using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_ExtendedTestInformation : TestFunctionBlockAssert
    {
        public FB_ExtendedTestInformation(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Check_TestSuite_Statistics();
            AssertTestClassName();
            Test_BOOL_AssertFailed();
            Test_BOOL_AssertSuccess();
            Test_BYTE_TwoFailedAsserts();
            Test_LINT_AssertFailed();
            Test_LINT_AssertSuccess();
        }

        private void Check_TestSuite_Statistics()
        {
            string testSuiteFinishedMessage = "Test suite ID=1 'PRG_TEST.ExtendedTestInformation'";
            string testSuiteStatistics = "ID=1 number of tests=5, number of failed tests=3";
            AssertContainsMessage(testSuiteFinishedMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertContainsMessage(testSuiteStatistics, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void AssertTestClassName()
        {
            string className = "Test class name=PRG_TEST.ExtendedTestInformation";
            // One for every each of the test
            // 1. Test_BOOL_AssertFailed
            // 2. Test_BOOL_AssertSuccess
            // 3. Test_BYTE_TwoFailedAsserts
            // 4. Test_LINT_AssertFailed
            // 5. Test_LINT_AssertSuccess
            AssertMessageCount(className, 5, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }


        private void Test_BOOL_AssertFailed()
        {
            string testMessage = CreateFailedTestMessage("Test_ExtendedTestInformation_BOOL_AssertFailed", "TRUE", "FALSE", "Extendedinformation values differ BOOL");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            // Extended information
            AssertContainsMessage("Test name=Test_ExtendedTestInformation_BOOL_AssertFailed", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertContainsMessage("Test assert message=Extendedinformation values differ BOOL failure", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test name=Test_ExtendedTestInformation_BOOL_AssertFailed", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test assert message=Extendedinformation values differ BOOL failure", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void Test_BOOL_AssertSuccess()
        {
            string testMessage = CreateFailedTestMessage("Test_ExtendedTestInformation_BOOL_AssertSuccess", "TRUE", "TRUE", "Extendedinformation values differ BOOL");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            // Extended information
            AssertContainsMessage("Test name=Test_ExtendedTestInformation_BOOL_AssertSuccess", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertDoesNotContainMessage("Test assert message=Extendedinformation values differ BOOL success", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void Test_BYTE_TwoFailedAsserts()
        {
            string testMessage = CreateFailedTestMessage("Test_ExtendedTestInformation_BYTE_TwoFailedAsserts", "0xAB", "0xCD", "Extendedinformation values differ BYTE1");
            string testMessage2 = CreateFailedTestMessage("Test_ExtendedTestInformation_BYTE_TwoFailedAsserts", "0xEF", "0x01", "Extendedinformation values differ BYTE2");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
            AssertContainsMessage(testMessage2, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            // Check that we only get extended information for the first assert
            AssertMessageCount("Test name=Test_ExtendedTestInformation_BYTE_TwoFailedAsserts", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test assert message=Extendedinformation values differ BYTE1", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test assert message=Extendedinformation values differ BYTE2", 0, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void Test_LINT_AssertFailed()
        {
            string testMessage = CreateFailedTestMessage("Test_ExtendedTestInformation_LINT_AssertFailed", "-451416345", "589532453", "Extendedinformation values differ LINT");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            // Extended information
            AssertContainsMessage("Test name=Test_ExtendedTestInformation_LINT_AssertFailed", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertContainsMessage("Test assert message=Extendedinformation values differ LINT failure", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test name=Test_ExtendedTestInformation_LINT_AssertFailed", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertMessageCount("Test assert message=Extendedinformation values differ LINT failure", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void Test_LINT_AssertSuccess()
        {
            string testMessage = CreateFailedTestMessage("Test_ExtendedTestInformation_LINT_AssertSuccess", "-123456789", "-123456789", "Extendedinformation values differ LINT");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            // Extended information
            AssertContainsMessage("Test name=Test_ExtendedTestInformation_LINT_AssertSuccess", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
            AssertDoesNotContainMessage("Test assert message=Extendedinformation values differ LINT success", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }
    }
}
