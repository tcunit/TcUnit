using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_SkipAssertionsWhenFinished : TestFunctionBlockAssert
    {
        public FB_SkipAssertionsWhenFinished(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_LongTest();
            Test_ShortTest();
            Test_AssertImmediatelyAfterFinished();
        }

        private void Test_LongTest()
        {
            AssertDoesNotContainMessage("Test_LongTest");
        }

        private void Test_ShortTest()
        {
            AssertDoesNotContainMessage("Test_ShortTest");
        }

        private void Test_AssertImmediatelyAfterFinished()
        {
            AssertDoesNotContainMessage("Test_AssertImmediatelyAfterFinished");
        }
    }
}