using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TcUnit.Verifier
{
    class FB_TestXUnitPublisher : TestFunctionBlockAssert
    {
        public FB_TestXUnitPublisher(IEnumerable<ErrorList.Error> errors, string xmlTestResultsFilePath, string testFunctionBlockInstance = null)
            : base(errors, testFunctionBlockInstance)
        {
            Test_ParceXml(xmlTestResultsFilePath);
            Test_EscapedFailedMessage();
            Test_EscapedFunctionName();
        }
        private void Test_ParceXml(string xmlTestResultsFilePath)
        {
            try
            {
                XDocument _ = XDocument.Load(xmlTestResultsFilePath);
            }
            catch (Exception)
            {
                log.Info("Test suite " + _testFunctionBlockInstance + " could not parse xml file: " + xmlTestResultsFilePath);
            }
            
        }
        private void Test_EscapedFailedMessage()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_EscapedFailedMessage";
            AssertContainsMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
  
        private void Test_EscapedFunctionName()
        {
            string testMessage = "PRG_TEST." + _testFunctionBlockInstance + "@Test_EscapedFunctionName";
            AssertDoesNotContainMessage(testMessage, EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh);
        }
    }
}
