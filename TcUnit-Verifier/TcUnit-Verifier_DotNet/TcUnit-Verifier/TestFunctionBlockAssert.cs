using EnvDTE80;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace TcUnit.Verifier
{
    class TestFunctionBlockAssert
    {
        private IEnumerable<ErrorList.Error> _errors;
        protected string _testFunctionBlockInstance;
        private static ILog log = LogManager.GetLogger("TcUnit-Verifier");

        private string DefaultFunctionBlockInstance
        {
            get
            {
                string className = this.GetType().Name;
                if (className.StartsWith("FB_"))
                    return className.Remove(0, 3);
                return className;
            }
        }

        public TestFunctionBlockAssert(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null)
        {
            _errors = errors;
            _testFunctionBlockInstance = testFunctionBlockInstance ?? DefaultFunctionBlockInstance;
        }

        protected string CreateFailedTestIdent(string method)
        {
            string returnString;
            returnString = "FAILED TEST 'PRG_TEST." + _testFunctionBlockInstance + "@" + method + "'";
            return returnString;
        }

        protected string CreateFailedTestMessage(string method, string expected, string actual, string message)
        {
            string returnString;
            returnString = CreateFailedTestIdent(method) + ", " + "EXP: " + expected + ", " + "ACT: " + actual + ", " + "MSG: " + message;
            return returnString;
        }

        protected string CreateFailedTestMessageNoAssertionMessage(string method, string expected, string actual)
        {
            string returnString;
            returnString = CreateFailedTestIdent(method) + ", " + "EXP: " + expected + ", " + "ACT: " + actual;
            return returnString;
        }

        private bool AreErrorItemsContainingTestMessage(string testMessage, vsBuildErrorLevel errorLevel)
        {
            return _errors.Any(e => (e.Description.Contains(testMessage.ToUpper())) && e.ErrorLevel.Equals(errorLevel));
        }
        private bool AreErrorItemsContainingTestMessage(string testIdentText, string testMessage, vsBuildErrorLevel errorLevel)
        {
            return _errors.Any(e => (e.Description.Contains(testIdentText.ToUpper())) && (e.Description.Contains(testMessage.ToUpper())) && e.ErrorLevel.Equals(errorLevel));
        }

        private int CountErrorItemsContainingTestMessage(string testMessage, vsBuildErrorLevel errorLevel)
        {
            return _errors.Count(s => (s.Description.Contains(testMessage.ToUpper())) && s.ErrorLevel.Equals(errorLevel));
        }

        protected void AssertMessageCount(string message, int messageCount, vsBuildErrorLevel errorLevel)
        {
            int actualCount = CountErrorItemsContainingTestMessage(message, errorLevel);
            if (actualCount != messageCount)
            {
                log.Info("Test suite " + _testFunctionBlockInstance +" reports message " +message + " " + actualCount + " times");
            }
        }

        /// <summary>
        /// Asserts that at least one of the messages in the array exists the messageCount number of times. Note that if the messageCount will
        /// increase also if both messages exist.
        /// </summary>
        protected void AssertAtLeastOneMessageCount(string[] messages, int messageCount, vsBuildErrorLevel errorLevel)
        {
            int actualCount = 0;
            string print;
            foreach (string s in messages)
            {
                int count = CountErrorItemsContainingTestMessage(s, errorLevel);
                actualCount = actualCount + count;
            }

            if (actualCount != messageCount)
            {
                print = "Test suite " + _testFunctionBlockInstance + " reports the messages [";

                foreach (string s in messages)
                {
                    print = print + s + ",";
                }
                print = print + "] ";
                print = print + actualCount + " times";
                log.Info(print);
            }
        }

        protected void AssertContainsMessage(string message, vsBuildErrorLevel errorLevel)
        {
            if (!AreErrorItemsContainingTestMessage(message, errorLevel))
            {
                log.Info("Test suite " +_testFunctionBlockInstance + " does not report: " + message);
            }
        }

        /// <summary>
        /// Asserts that at least one message in the array exists
        /// </summary>
        protected void AssertContainsAtLeastOneMessage(string[] messages, vsBuildErrorLevel errorLevel)
        {
            bool foundMessage = false;
            foreach (string s in messages)
            {
                if (AreErrorItemsContainingTestMessage(s, errorLevel)) { 
                    foundMessage = true;
                    break;
                }
            }
            if (!foundMessage)
            {
                string print;
                print = "Test suite " + _testFunctionBlockInstance + " does not report any of the messages: [";
                
                foreach (string s in messages)
                {
                    print = print + s + ",";
                }
                print = print + "]";
                log.Info(print);
            }
        }

        protected void AssertDoesNotContainMessage(string message, vsBuildErrorLevel errorLevel)
        {
            if (AreErrorItemsContainingTestMessage(message, errorLevel))
            {
                log.Info("Test suite " + _testFunctionBlockInstance + " reports: " + message);
            }
        }
        protected void AssertDoesNotContainMessage(string testName, string message, vsBuildErrorLevel errorLevel)
        {
            string testIdentText = CreateFailedTestIdent(testName);
            if (AreErrorItemsContainingTestMessage(testIdentText, message, errorLevel))
            {
                log.Info("Test " + _testFunctionBlockInstance + "." + testName + " reports: " + message);
            }
        }
    }
}
