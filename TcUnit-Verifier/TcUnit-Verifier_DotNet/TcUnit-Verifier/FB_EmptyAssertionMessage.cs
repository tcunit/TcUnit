using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_EmptyAssertionMessage : TestFunctionBlockAssert
    {
        public FB_EmptyAssertionMessage(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            IntegerEmptyAssertionMessage();
        }

        private void IntegerEmptyAssertionMessage()
        {
            string testMessage = CreateFailedTestMessageNoAssertionMessage("IntegerEmptyAssertionMessage", "-32000", "15423");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
