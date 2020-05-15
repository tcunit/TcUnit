using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_MultipleAssertWithSameParametersInSameCycleWithSameTest : TestFunctionBlockAssert
    {
        public FB_MultipleAssertWithSameParametersInSameCycleWithSameTest(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Assert_SeveralTimes();
        }

        private void Assert_SeveralTimes()
        {
            string testMessage = CreateFailedTestMessage("Assert_SeveralTimes", "0x12345678", "0x90ABCDEF", "Values differ");
            AssertMessageCount(testMessage, 3, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}