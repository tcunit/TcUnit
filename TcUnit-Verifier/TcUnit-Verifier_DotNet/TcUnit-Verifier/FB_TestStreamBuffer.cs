using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_TestStreamBuffer : TestFunctionBlockAssert
    {
        public FB_TestStreamBuffer(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_BufferSizeDiffers();
            Test_Length();
            Test_Append();
            Test_Clear();
            Test_Find();
            Test_Copy();
            Test_CutOff();
        }

        private void Test_BufferSizeDiffers()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_BufferSizeDiffers";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Append()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Append";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Clear()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Clear";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_CutOff()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_CutOff";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Copy()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Copy";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Find()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Find";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_Length()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_Length";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
