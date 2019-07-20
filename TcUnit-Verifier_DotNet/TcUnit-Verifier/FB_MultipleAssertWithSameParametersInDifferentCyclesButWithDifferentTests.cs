using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests : TestFunctionBlockAssert
    {
        public FB_MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            Assert_SeveralTimes();
            Assert_SeveralTimesAgain();
            Assert_SeveralTimesAgainAgain();
        }

        private void Assert_SeveralTimes()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimes", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 3);
        }

        private void Assert_SeveralTimesAgain()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimesAgain", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 3);
        }

        private void Assert_SeveralTimesAgainAgain()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimesAgainAgain", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 3);
        }
    }
}