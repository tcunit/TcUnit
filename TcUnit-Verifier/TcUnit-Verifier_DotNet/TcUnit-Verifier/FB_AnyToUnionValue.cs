using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_AnyToUnionValue : TestFunctionBlockAssert
    {
        public FB_AnyToUnionValue(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            Test_BOOL();
            Test_BIT();
            Test_BYTE();
            Test_WORD();
            Test_DWORD();
            Test_LWORD();
            Test_SINT();
            Test_INT();
            Test_DINT();
            Test_LINT();
            Test_USINT();
            Test_UINT();
            Test_UDINT();
            Test_ULINT();
            Test_REAL();
            Test_LREAL();
            Test_STRING();
            Test_STRING_2();
            Test_WSTRING();
            Test_TIME();
            Test_DATE();
            Test_DATE_AND_TIME();
            Test_TIME_OF_DAY();
            Test_LTIME();
        }

        private void Test_BOOL()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BOOL", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_BIT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BIT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_BYTE()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BYTE", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_WORD", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_DWORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DWORD", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_LWORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LWORD", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_SINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_SINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_INT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_INT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_DINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_LINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_USINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_USINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_UINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_UINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_UDINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_UDINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ULINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_ULINT", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_REAL()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_REAL", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_LREAL()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LREAL", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_STRING()
        {
            AssertDoesNotContainMessage("'PRG_TEST." + _testFunctionBlockInstance + "@Test_STRING'", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_STRING_2()
        {
            AssertDoesNotContainMessage("'PRG_TEST." + _testFunctionBlockInstance + "@Test_STRING_2'", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_WSTRING()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_WSTRING", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_TIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_TIME", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_DATE()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DATE", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_DATE_AND_TIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DATE_AND_TIME", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_TIME_OF_DAY()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_TIME_OF_DAY", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_LTIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LTIME", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
