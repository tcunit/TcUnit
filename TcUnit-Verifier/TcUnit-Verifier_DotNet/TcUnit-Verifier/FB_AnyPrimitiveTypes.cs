using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AnyPrimitiveTypes : TestFunctionBlockAssert
    {
        public FB_AnyPrimitiveTypes(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            Test_ANY_BYTE_Equals();
            Test_ANY_BYTE_Differ();
            Test_ANY_BOOL_Equals();
            Test_ANY_BOOL_Differ();
            Test_ANY_DATE_Equals();
            Test_ANY_DATE_Differ();
            Test_ANY_DATE_AND_TIME_Equals();
            Test_ANY_DATE_AND_TIME_Differ();
            Test_ANY_DINT_Equals();
            Test_ANY_DINT_Differ();
            Test_ANY_DWORD_Equals();
            Test_ANY_DWORD_Differ();
            Test_ANY_INT_Equals();
            Test_ANY_INT_Differ();
            Test_ANY_LINT_Equals();
            Test_ANY_LINT_Differ();
            Test_ANY_LREAL_Equals();
            Test_ANY_LREAL_Differ();
            Test_ANY_LTIME_Equals();
            Test_ANY_LTIME_Differ();
            Test_ANY_LWORD_Equals();
            Test_ANY_LWORD_Differ();
            Test_ANY_REAL_Equals();
            Test_ANY_REAL_Differ();
            Test_ANY_SINT_Equals();
            Test_ANY_SINT_Differ();
            Test_ANY_STRING_Equals();
            Test_ANY_STRING_Differ();
            Test_ANY_TIME_Equals();
            Test_ANY_TIME_Differ();
            Test_ANY_TIME_OF_DAY_Equals();
            Test_ANY_TIME_OF_DAY_Differ();
            Test_ANY_UDINT_Equals();
            Test_ANY_UDINT_Differ();
            Test_ANY_UINT_Equals();
            Test_ANY_UINT_Differ();
            Test_ANY_ULINT_Equals();
            Test_ANY_ULINT_Differ();
            Test_ANY_USINT_Equals();
            Test_ANY_USINT_Differ();
            Test_ANY_WORD_Equals();
            Test_ANY_WORD_Differ();
        }

       

        private void Test_ANY_BOOL_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_BOOL_Equals", "TRUE", "TRUE", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_BOOL_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_BOOL_Differ", "TRUE", "FALSE", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_BYTE_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_BYTE_Equals", "0xCD", "0xCD", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_BYTE_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_BYTE_Differ", "0xAB", "0xCD", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DATE_AND_TIME_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DATE_AND_TIME_Equals", "DT#2019-01-20-13:54:30", "DT#2019-01-20-13:54:30", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DATE_AND_TIME_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DATE_AND_TIME_Differ", "DT#1996-05-06-15:36:30", "DT#1972-03-29-00:00:00", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DATE_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DATE_Equals", "D#1996-05-06", "D#1996-05-06", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DATE_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DATE_Differ", "D#1996-05-06", "D#2019-01-20", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DINT_Equals", "-80000", "-80000", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DINT_Differ", "-55555", "70000", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DWORD_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DWORD_Equals", "0x7890ABCD", "0x7890ABCD", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_DWORD_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_DWORD_Differ", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_INT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_INT_Equals", "-12345", "-12345", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_INT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_INT_Differ", "-32000", "15423", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LINT_Equals", "-123456789", "-123456789", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LINT_Differ", "-451416345", "589532453", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LREAL_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LREAL_Equals", "1234567.89", "1234567.76", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LREAL_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LREAL_Differ", "1234567.89", "1234567.76", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LTIME_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LTIME_Equals", "LTIME#213503d23h34m33s709ms551us615ns", "LTIME#213503d23h34m33s709ms551us615ns", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LTIME_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LTIME_Differ", "LTIME#213503d23h34m33s709ms551us615ns", "LTIME#1000d15h23m12s34ms2us44ns", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LWORD_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LWORD_Equals", "0x0123456789ABCDEF", "0x0123456789ABCDEF", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_LWORD_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_LWORD_Differ", "0x0123656789ABCBEC", "0x0123256789ABCAEE", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_REAL_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_REAL_Equals", "1234.5", "1234.5", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_REAL_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_REAL_Differ", "1234.5", "1234.4", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_SINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_SINT_Equals", "-128", "-128", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_SINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_SINT_Differ", "127", "-30", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_STRING_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_STRING_Equals", "Hello there", "Hello there", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_STRING_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_STRING_Differ", "This is a string", "This is another string", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_TIME_Equals()
        {
            // TwinCAT 3.1.4020 & 3.1.4022
            string testMessage = CreateFailedTestMessage("Test_ANY_TIME_Equals", "T#694m13s244ms", "T#694m13s244ms", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
            // TwinCAT 3.1.4024 and newer
            testMessage = CreateFailedTestMessage("Test_ANY_TIME_Equals", "T#12h34m15s10ms", "T#11h34m13s244ms", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_TIME_Differ()
        {
            // Differ between TwinCAT 3.1.4020/4022 and 4024 (or newer)
            string testMessage4022 = CreateFailedTestMessage("Test_ANY_TIME_Differ", "T#754m15s10ms", "T#694m13s244ms", "Values differ");
            string testMessage4024AndNewer = CreateFailedTestMessage("Test_ANY_TIME_Differ", "T#12h34m15s10ms", "T#11h34m13s244ms", "Values differ");
            string[] messages = new String[] { testMessage4022, testMessage4024AndNewer };
            AssertContainsAtLeastOneMessage(messages, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_TIME_OF_DAY_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_TIME_OF_DAY_Equals", "TOD#06:21:11.492", "TOD#06:21:11.492", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_TIME_OF_DAY_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_TIME_OF_DAY_Differ", "TOD#15:36:30.123", "TOD#06:21:11.492", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_UDINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_UDINT_Equals", "21845123", "21845123", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_UDINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_UDINT_Differ", "34124214", "52343244", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_UINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_UINT_Equals", "65535", "65535", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_UINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_UINT_Differ", "64322", "32312", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_ULINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_ULINT_Equals", "45683838383", "45683838383", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_ULINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_ULINT_Differ", "10000", "53685437234", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_USINT_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_USINT_Equals", "5", "5", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_USINT_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_USINT_Differ", "3", "7", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_WORD_Equals()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_WORD_Equals", "0xABCD", "0xABCD", "Values differ");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ANY_WORD_Differ()
        {
            string testMessage = CreateFailedTestMessage("Test_ANY_WORD_Differ", "0xEF01", "0x2345", "Values differ");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
