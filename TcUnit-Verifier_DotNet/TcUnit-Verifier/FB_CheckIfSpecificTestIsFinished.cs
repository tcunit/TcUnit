using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_CheckIfSpecificTestIsFinished : TestFunctionBlockAssert
    {
        public FB_CheckIfSpecificTestIsFinished(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            TestThatInstantlyFinishes();
        }

        private void TestThatInstantlyFinishes()
        {
            string testMessage1 = CreateFailedTestMessage("CheckBeforeAndAfterFinishing", "FALSE", "TRUE", "Values differ before finishing");
            AssertDoesNotContainMessage(testMessage1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            string testMessage2 = CreateFailedTestMessage("CheckBeforeAndAfterFinishing", "TRUE", "FALSE", "Values differ after finishing");
            AssertDoesNotContainMessage(testMessage2, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
