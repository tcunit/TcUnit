using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_WriteProtectedFunctions : TestFunctionBlockAssert
    {
        public FB_WriteProtectedFunctions(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_WRITE_PROTECTED_BOOL();
            Test_WRITE_PROTECTED_BYTE();
            Test_WRITE_PROTECTED_DATE();
            Test_WRITE_PROTECTED_DATE_AND_TIME();
            Test_WRITE_PROTECTED_DINT();
            Test_WRITE_PROTECTED_DWORD();
            Test_WRITE_PROTECTED_INT();
            TEST_WRITE_PROTECTED_LINT();
            Test_WRITE_PROTECTED_LREAL();
            Test_WRITE_PROTECTED_LWORD();
            Test_WRITE_PROTECTED_REAL();
            Test_WRITE_PROTECTED_SINT();
            Test_WRITE_PROTECTED_STRING();
            Test_WRITE_PROTECTED_TIME();
            Test_WRITE_PROTECTED_TIME_OF_DAY();
            Test_WRITE_PROTECTED_UDINT();
            Test_WRITE_PROTECTED_UINT();
            Test_WRITE_PROTECTED_USINT();
            Test_WRITE_PROTECTED_ULINT();
            Test_WRITE_PROTECTED_WORD();
            Test_WRITE_PROTECTED_WSTRING();
        }

        private void Test_WRITE_PROTECTED_BOOL()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_BOOL";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_BYTE()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_BYTE";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_DATE()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_DATE";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_DATE_AND_TIME()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_DATE_AND_TIME";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_DINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_DINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_DWORD() {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_DWORD";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_INT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_INT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void TEST_WRITE_PROTECTED_LINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@TEST_WRITE_PROTECTED_LINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_LREAL()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_LREAL";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_LWORD()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_LWORD";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_REAL()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_REAL";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_SINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_SINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_STRING()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_STRING";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_TIME()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_TIME";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_TIME_OF_DAY()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_TIME_OF_DAY";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_UDINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_UDINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_UINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_UINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_USINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_USINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_ULINT()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_ULINT";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_WORD()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_WORD";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WRITE_PROTECTED_WSTRING()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_WRITE_PROTECTED_WSTRING";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
