using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_TestFinishedNamed : TestFunctionBlockAssert
    {
        public FB_TestFinishedNamed(ErrorItems errorItems, string testFunctionBlockInstance = null)
            : base(errorItems, testFunctionBlockInstance)
        {
            Test_FinishedNamed();
        }

        private void Test_FinishedNamed()
        {
            
        }
    }
}
