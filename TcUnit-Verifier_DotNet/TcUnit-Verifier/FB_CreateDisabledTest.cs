using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_CreateDisabledTest : TestFunctionBlockAssert
    {
        public FB_CreateDisabledTest(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            TestEnabled();
            TestDisabled();
        }

        private void TestEnabled()
        {
            string testMessage = CreateFailedTestMessage("TestEnabled", "TRUE", "FALSE", "A does not equal B");
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void TestDisabled()
        {
            string testMessage = CreateFailedTestMessage("DISABLED_ThisShouldNotExecute", "FALSE", "TRUE", "A does not equal B");
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
        
    }
}