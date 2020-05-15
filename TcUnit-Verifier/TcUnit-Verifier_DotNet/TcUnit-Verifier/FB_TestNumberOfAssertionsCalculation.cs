using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_TestNumberOfAssertionsCalculation : TestFunctionBlockAssert
    {
        public FB_TestNumberOfAssertionsCalculation(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            TestMixed33SuccessulAnd9FailedAssertions();
            TestWith43SuccessfulAssertions();
            TestWith44FailedAssertions();
            TestWith45SuccessfulArrayAssertions();
            TestWith46FailedArrayAssertions();
        }

        private void TestMixed33SuccessulAnd9FailedAssertions()
        {
            AssertContainsMessage("| Test status=FAIL, number of asserts=42", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void TestWith43SuccessfulAssertions()
        {
            AssertContainsMessage("| Test status=PASS, number of asserts=43", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void TestWith44FailedAssertions()
        {
            AssertContainsMessage("| Test status=FAIL, number of asserts=44", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void TestWith45SuccessfulArrayAssertions()
        {
            AssertContainsMessage("| Test status=PASS, number of asserts=45", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

        private void TestWith46FailedArrayAssertions()
        {
            AssertContainsMessage("| Test status=FAIL, number of asserts=46", EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow);
        }

    }
}
