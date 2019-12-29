using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_CreateFourTestsWithSameName : TestFunctionBlockAssert
    {
        public FB_CreateFourTestsWithSameName(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            TestOne();
        }

        private void TestOne()
        {
            AssertMessageCount("Test with name 'TestOne' already exists in test suite 'PRG_TEST.CreateFourTestsWithSameName'", 1, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
