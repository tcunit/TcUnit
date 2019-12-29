using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest : TestFunctionBlockAssert
    {
        public FB_MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Assert_SeveralTimes();
        }

        private void Assert_SeveralTimes()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimes", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 9, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}