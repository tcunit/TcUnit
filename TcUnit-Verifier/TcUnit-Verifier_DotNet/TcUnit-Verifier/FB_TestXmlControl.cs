using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_TestXmlControl : TestFunctionBlockAssert
    {
        public FB_TestXmlControl(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_NewTag();
            Test_NewTagNested();
            Test_NewParameter();
            Test_NewComment();
            Test_CloseOpenTag();
            Test_CloseTag();
            Test_NewTagData();
            Test_NextTagFlat();
            Test_NextTagNested();
        }

        private void Test_NewTag()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NewTag";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
  
        private void Test_NewTagNested()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NewTagNested";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_NewParameter()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NewParameter";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_NewComment()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NewComment";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_CloseOpenTag()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_CloseOpenTag";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_CloseTag()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_CloseTag";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_NewTagData()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NewTagData";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_NextTagFlat()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NextTagFlat";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }

        private void Test_NextTagNested()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_NextTagNested";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
