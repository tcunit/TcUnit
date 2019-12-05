using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AdjustAssertFailureMessageToMax252CharLengthTest : TestFunctionBlockAssert
    {
        public FB_AdjustAssertFailureMessageToMax252CharLengthTest(ErrorItems errorItems, string testFunctionBlockInstance = null) : base(errorItems, testFunctionBlockInstance)
        {
            TestInstancePath252CharsExpectTooLongTestInstancePath();
            TestInstancePath220CharsExpectShortenedTestInstancePath();
            TestInstancePath255CharsExpectTooLongTestInstancePath();
        }

        private void TestInstancePath252CharsExpectTooLongTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath252CharsExpectTooLongTestInstancePath");
        }

        private void TestInstancePath220CharsExpectShortenedTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath220CharsExpectShortenedTestInstancePath");
        }

        private void TestInstancePath255CharsExpectTooLongTestInstancePath()
        {
            AssertDoesNotContainMessage("TestInstancePath255CharsExpectTooLongTestInstancePath");
        }
    }
}