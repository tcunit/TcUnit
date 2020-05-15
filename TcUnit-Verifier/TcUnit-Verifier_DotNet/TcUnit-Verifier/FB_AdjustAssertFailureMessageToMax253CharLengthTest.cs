using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_AdjustAssertFailureMessageToMax253CharLengthTest : TestFunctionBlockAssert
    {
        public FB_AdjustAssertFailureMessageToMax253CharLengthTest(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            TestInstancePath253CharsExpectTooLongTestInstancePath();
            TestInstancePath221CharsExpectShortenedTestInstancePath();
            TestInstancePath255CharsExpectTooLongTestInstancePath();
        }

        private void TestInstancePath253CharsExpectTooLongTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath253CharsExpectTooLongTestInstancePath", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void TestInstancePath221CharsExpectShortenedTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath221CharsExpectShortenedTestInstancePath", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void TestInstancePath255CharsExpectTooLongTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath255CharsExpectTooLongTestInstancePath", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}