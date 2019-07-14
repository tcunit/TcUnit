using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_CreateFourTestsWithSameName : TestFunctionBlockAssert
    {
        public FB_CreateFourTestsWithSameName(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            TestOne();
        }

        private void TestOne()
        {
            AssertMessageCount("Test with name 'TestOne' already exists in test suite 'PRG_TEST.CreateFourTestsWithSameName'", 1);
        }
    }
}
