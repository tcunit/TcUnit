using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class TestFunctionBlockAssert
    {
        protected ErrorItems _errorItems;
        protected string _testFunctionBlockInstance;

        public TestFunctionBlockAssert(ErrorItems errorItems, string testFunctionBlockInstance)
        {
            _errorItems = errorItems;
            _testFunctionBlockInstance = testFunctionBlockInstance;
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
            bool result = false;
            for (int i = 1; i <= _errorItems.Count; i++)
            {
                ErrorItem item = _errorItems.Item(i);
                if (item.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh)
                {
                    if (item.Description.ToUpper().Contains(testMessage.ToUpper()))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private int CountErrorItemsContainingTestMessage(string testMessage)
        {
            int count = 0;
            for (int i = 1; i <= _errorItems.Count; i++)
            {
                ErrorItem item = _errorItems.Item(i);
                if (item.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh)
                {
                    if (item.Description.ToUpper().Contains(testMessage.ToUpper()))
                    {
                        count++;
                    }
                }
            }
            return count;
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
