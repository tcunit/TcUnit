using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_TestFileControl : TestFunctionBlockAssert
    {
        public FB_TestFileControl(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_Read();
            Test_Open();
            Test_Write();
            Test_Close();
            Test_Delete();
        }

        private void Test_Read()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Read";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
  
        private void Test_Open()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Open";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Write()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Write";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Close()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Close";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Delete()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Delete";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
