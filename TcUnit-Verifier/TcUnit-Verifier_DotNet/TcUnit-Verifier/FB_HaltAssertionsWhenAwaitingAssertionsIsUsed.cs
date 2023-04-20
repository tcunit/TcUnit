using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_HaltAssertionsWhenAwaitingAssertionsIsUsed : TestFunctionBlockAssert
    {
        public FB_HaltAssertionsWhenAwaitingAssertionsIsUsed(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_AssertImmediatelyAfterTest();
            Test_ShortHalt();
            Test_LongHalt();
        }

        private void Test_AssertImmediatelyAfterTest()
        {
            string testMessage = CreateFailedTestMessage("Test_AssertImmediatelyAfterTest", 
                                                         "TRUE", 
                                                         "FALSE", 
                                                         "this assertion should fail because test needs 2 second to have the real result");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_ShortHalt()
        {
            string testMessage = CreateFailedTestMessage("Test_ShortHalt",
                                                         "TRUE",
                                                         "FALSE",
                                                         "this assertion should fail because test needs 2 second to have the real result but assertion halted for 1 second");
            AssertContainsMessage("Test_ShortHalt", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_LongHalt()
        {
            AssertDoesNotContainMessage("Test_LongHalt", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}