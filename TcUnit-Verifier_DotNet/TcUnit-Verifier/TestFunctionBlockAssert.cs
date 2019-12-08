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

        /// <summary>
        /// Asserts that at least one of the messages in the array exists the messageCount amount of times. Note that if the messageCount will
        /// increase also if both messages exist.
        /// </summary>
        protected void AssertAtLeastOneMessageCount(string[] messages, int messageCount)
        {
            int actualCount = 0;
            foreach (string s in messages)
            {
                int count = CountErrorItemsContainingTestMessage(s);
                actualCount = actualCount + count;
            }

            if (actualCount != messageCount)
            {
                Console.Write("Test suite " + _testFunctionBlockInstance + " reports the messages [");
                foreach (string s in messages)
                {
                    Console.Write(s + ",");
                }
                Console.Write("] ");
                Console.Write(actualCount + " times" + Environment.NewLine);
            }
        }

        protected void AssertContainsMessage(string message)
        {
            if (!AreErrorItemsContainingTestMessage(message))
            {
                Console.WriteLine("Test suite " +_testFunctionBlockInstance + " does not report: " + message);
            }
        }

        /// <summary>
        /// Asserts that at least one message in the array exists
        /// </summary>
        protected void AssertContainsAtLeastOneMessage(string[] messages)
        {
            bool foundMessage = false;
            foreach (string s in messages)
            {
                if (AreErrorItemsContainingTestMessage(s)) { 
                    foundMessage = true;
                    break;
                }
            }
            if (!foundMessage)
            {
                Console.Write("Test suite " + _testFunctionBlockInstance + " does not report any of the messages: [");
                foreach (string s in messages)
                {
                    Console.Write(s + ",");
                }
                Console.Write("]" + Environment.NewLine);
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
