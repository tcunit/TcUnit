using System;
using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_AssertCountExceedsMaxNumber : TestFunctionBlockAssert
    {
        public FB_AssertCountExceedsMaxNumber(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            string testName;
            string testMessage;

            testName = "Assert_SameEntryInOneCycle";
            testMessage = "AssertResults.TotalAsserts invalid";
            AssertDoesNotContainMessage(testName, testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);

            testName = "Assert_SameArrayEntryInOneCycle";
            testMessage = "AssertArrayResults.TotalArrayAsserts invalid";
            AssertDoesNotContainMessage(testName, testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

    }
}
