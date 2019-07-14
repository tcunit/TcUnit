using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AssertEveryFailedTestTwice_Asserter : TestFunctionBlockAssert
    {
        public FB_AssertEveryFailedTestTwice_Asserter(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            TwiceAssertCall();
        }

        private void TwiceAssertCall()
        {
            string testMessage = CreateFailedTestMessage("TwiceAssertCall", "55", "77", "Not equal ANY");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "TRUE", "FALSE", "Not equal BOOL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "0xAB", "0xBA", "Not equal BYTE");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "D#1996-05-06", "D#2019-01-20", "Not equal DATE");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "DT#1996-05-06-15:36:30", "DT#1972-03-29-00:00:00", "Not equal DATE_AND_TIME");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "4444", "3333", "Not equal DINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "0xAAAAAAAA", "0xBBBBBBBB", "Not equal DWORD");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "30000", "32000", "Not equal INT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "50000", "50001", "Not equal LINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "33.3", "44.4", "Not equal LREAL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "LTIME#213503d23h34m33s709ms551us615ns", "LTIME#1000d15h23m12s34ms2us44ns", "Not equal LTIME");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "0xAAAAAAAAAAAAAAAA", "0xBBBBBBBBBBBBBBBB", "Not equal LWORD");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "44.4", "22.2", "Not equal REAL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "99", "10", "Not equal SINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "Hello world", "Hey there", "Not equal STRING");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "249494994", "1223", "Not equal UDINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "3444", "3445", "Not equal UINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "789234475", "34523327234", "Not equal ULINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "34", "36", "Not equal USINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "T#754m15s10ms", "T#694m13s244ms", "Not equal TIME");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "TOD#15:36:30.123", "TOD#06:21:11.492", "Not equal TIME_OF_DAY");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall", "0xABCD", "0x89EF", "Not equal WORD");
            AssertMessageCount(testMessage, 1);
        }
        
    }
}
