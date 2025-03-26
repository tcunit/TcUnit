using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_TestDurationMeasurement : TestFunctionBlockAssert
    {
        public FB_TestDurationMeasurement(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            TestNamedTest20msDurationMeasuredCorrectly();
            TestOrderedTest30msDurationMeasuredCorrectly();
            TestRegularTestDurationMeasuredCorrectly();
        }

        private void TestNamedTest20msDurationMeasuredCorrectly()
        {
            AssertContainsResultSet("TestNamedTest20msDurationMeasuredCorrectly", "PRG_TEST.TestDurationMeasurement", "FAIL", 1, 0.02, 0.1);
        }

        private void TestOrderedTest30msDurationMeasuredCorrectly()
        {
            AssertContainsResultSet("TestOrderedTest30msDurationMeasuredCorrectly", "PRG_TEST.TestDurationMeasurement", "FAIL", 1, 0.03, 0.1);
        }

        private void TestRegularTestDurationMeasuredCorrectly()
        {
            AssertContainsResultSet("TestRegularTestDurationMeasuredCorrectly", "PRG_TEST.TestDurationMeasurement", "FAIL", 2, 0.0003, 0.001);
        }
    }
}
