using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AssertEveryFailedTestTwiceArrayVersion : TestFunctionBlockAssert
    {
        public FB_AssertEveryFailedTestTwiceArrayVersion(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            TwiceAssertCall_Arrays();
        }

        private void TwiceAssertCall_Arrays()
        {
            string testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[2] = FALSE", "ARRAY[2] = TRUE", "Not equal BOOL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1] = 0xAA", "ARRAY[1] = 0xCD", "Not equal BYTE");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[4] = -2147483645", "ARRAY[4] = -2147483641", "Not equal DINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[-1] = 0xEFAA2346", "ARRAY[-1] = 0xEF012345", "Not equal DWORD");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[-7] = -23", "ARRAY[2] = 24", "Not equal INT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[0] = -9223372036853775808", "ARRAY[5] = -9223372036854775808", "Not equal LINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[-1] = 7.88", "ARRAY[0] = 7.99", "Not equal LREAL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1,0] = 6.96", "ARRAY[1,0] = 6.68", "Not equal LREAL2D");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1,0,1] = 6.0", "ARRAY[1,0,1] = 6.4", "Not equal LREAL3D");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1] = 0xEDCBA09876543210", "ARRAY[1] = 0x01234567890ABCDE", "Not equal LWORD");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[2] = 2.44001", "ARRAY[2] = 2.44003", "Not equal REAL");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1,1] = 7.7701", "ARRAY[1,1] = 7.7703", "Not equal REAL2D");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[0,1,1] = 4.01", "ARRAY[0,1,1] = 4.021", "Not equal REAL3D");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[0] = -128", "ARRAY[0] = 127", "Not equal SINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[-4] = 5", "ARRAY[1] = 4", "Not equal UDINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[3] = 99", "ARRAY[3] = 12", "Not equal UINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[1] = 9400000000000", "ARRAY[1] = 18446744073709551615", "Not equal ULINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[4] = 4", "ARRAY[4] = 5", "Not equal USINT");
            AssertMessageCount(testMessage, 1);

            testMessage = CreateFailedTestMessage("TwiceAssertCall_Arrays", "ARRAY[7] = 0x1133", "ARRAY[7] = 0x1122", "Not equal WORD");
            AssertMessageCount(testMessage, 1);
        }
        
    }
}
