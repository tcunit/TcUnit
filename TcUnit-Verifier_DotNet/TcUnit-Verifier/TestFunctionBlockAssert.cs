using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class TestFunctionBlockAssert
    {
        private IEnumerable<ErrorList.Error> _errors;
        protected string _testFunctionBlockInstance;

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

        protected string CreateFailedTestMessage(string method, string expected, string actual, string message)
        {
            string returnString;
            returnString = "FAILED TEST 'PRG_TEST." + _testFunctionBlockInstance + "@" +method +"', " 
                +"EXP: " +expected + ", " +"ACT: " +actual + ", " + "MSG: " +message;
            return returnString;
        }

        private bool AreErrorItemsContainingTestMessage(string testMessage)
        {
            return _errors.Any(e => e.Description.Contains(testMessage.ToUpper()));
        }

        private int CountErrorItemsContainingTestMessage(string testMessage)
        {
            return _errors.Count(s => s.Description.Contains(testMessage.ToUpper()));
        }

        protected void AssertMessageCount(string message, int messageCount)
        {
            int actualCount = CountErrorItemsContainingTestMessage(message);
            if (actualCount != messageCount)
            {
                Console.WriteLine("Test suite " + _testFunctionBlockInstance +" reports message " +message + " " + actualCount + " times");
            }
        }

        protected void AssertContainsMessage(string message)
        {
            if (!AreErrorItemsContainingTestMessage(message))
            {
                Console.WriteLine("Test suite " +_testFunctionBlockInstance + " does not report: " + message);
            }
        }

        protected void AssertDoesNotContainMessage(string message)
        {
            if (AreErrorItemsContainingTestMessage(message))
            {
                Console.WriteLine("Test suite " + _testFunctionBlockInstance + " reports: " + message);
            }
        }
    }
}
