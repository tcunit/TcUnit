using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class FB_TestFinishedNamed : TestFunctionBlockAssert
    {
        public FB_TestFinishedNamed(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_FinishedNamed();
        }

        private void Test_FinishedNamed()
        {
            
        }
    }
}
