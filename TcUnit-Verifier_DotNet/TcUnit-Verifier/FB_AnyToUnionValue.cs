using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AnyToUnionValue : TestFunctionBlockAssert
    {
        public FB_AnyToUnionValue(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
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
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BOOL");
        }

        private void Test_BIT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BIT");
        }

        private void Test_BYTE()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_BYTE");
        }

        private void Test_WORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_WORD");
        }

        private void Test_DWORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DWORD");
        }

        private void Test_LWORD()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LWORD");
        }

        private void Test_SINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_SINT");
        }

        private void Test_INT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_INT");
        }

        private void Test_DINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DINT");
        }

        private void Test_LINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LINT");
        }

        private void Test_USINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_USINT");
        }

        private void Test_UINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_UINT");
        }

        private void Test_UDINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_UDINT");
        }

        private void Test_ULINT()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_ULINT");
        }

        private void Test_REAL()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_REAL");
        }

        private void Test_LREAL()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LREAL");
        }

        private void Test_STRING()
        {
            string testMessage = CreateFailedTestMessage("Test_STRING", "(Data size = 81)", "(Data size = 256)", "Values differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_STRING_2()
        {
            AssertDoesNotContainMessage("'PRG_TEST." + _testFunctionBlockInstance + "@Test_STRING_2'");
        }

        private void Test_WSTRING()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_WSTRING");
        }

        private void Test_TIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_TIME");
        }

        private void Test_DATE()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DATE");
        }

        private void Test_DATE_AND_TIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_DATE_AND_TIME");
        }

        private void Test_TIME_OF_DAY()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_TIME_OF_DAY");
        }

        private void Test_LTIME()
        {
            AssertDoesNotContainMessage("PRG_TEST." + _testFunctionBlockInstance + "@Test_LTIME");
        }
    }
}
