using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_MultipleAssertWithSameParametersInSameCycleWithSameTest : TestFunctionBlockAssert
    {
        public FB_MultipleAssertWithSameParametersInSameCycleWithSameTest(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            Assert_SeveralTimes();
        }

        private void Assert_SeveralTimes()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimes", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 3);
        }
    }
}