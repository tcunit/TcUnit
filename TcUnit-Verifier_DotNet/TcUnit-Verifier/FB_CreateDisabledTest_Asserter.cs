using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_CreateDisabledTest_Asserter : TestFunctionBlockAssert
    {
        public FB_CreateDisabledTest_Asserter(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            TestEnabled();
            TestDisabled();
        }

        private void TestEnabled()
        {
            string testMessage = CreateFailedTestMessage("TestEnabled", "TRUE", "FALSE", "A does not equal B");
            AssertContainsMessage(testMessage);
        }

        private void TestDisabled()
        {
            string testMessage = CreateFailedTestMessage("DISABLED_ThisShouldNotExecute", "FALSE", "TRUE", "A does not equal B");
            AssertDoesNotContainMessage(testMessage);
        }
        
    }
}
