﻿using EnvDTE80;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        protected string CreateFailedTestMessage(string method, string expected, string actual, string message)
        {
            string returnString;
            returnString = CreateFailedTestCommonString(method) + ", " + "EXP: " + expected + ", " + "ACT: " + actual + ", " + "MSG: " + message;
            return returnString;
        }

        protected string CreateFailedTestMessageNoAssertionMessage(string method, string expected, string actual)
        {
            string returnString;
            returnString = CreateFailedTestCommonString(method) + ", " + "EXP: " + expected + ", " + "ACT: " + actual;
            return returnString;
        }

        private bool AreErrorItemsContainingTestMessage(string testMessage, vsBuildErrorLevel errorLevel)
        {
            // no regex needed, do a fast check
            if(!testMessage.Contains("%f"))
                return _errors.Any(e => e.Description.Contains(testMessage.ToUpper()) && e.ErrorLevel.Equals(errorLevel));

            // convert number placeholders (%f) to a regex that matches floating point values
            testMessage = @".*?" + Regex.Escape(testMessage).Replace("%f", @"[+-]?(\d+([.]\d*)?([eE][+-]?\d+)?|[.]\d+([eE][+-]?\d+)?)") + @".*?";
            return _errors.Any(e => Regex.Match(e.Description, testMessage, RegexOptions.IgnoreCase).Success && e.ErrorLevel.Equals(errorLevel));
        }

        private bool AreErrorItemsContainingTestMessage(string testIdentText, string testMessage, vsBuildErrorLevel errorLevel)
        {
            return _errors.Any(e => (e.Description.Contains(testIdentText.ToUpper())) && (e.Description.Contains(testMessage.ToUpper())) && e.ErrorLevel.Equals(errorLevel));
        }

        private int CountErrorItemsContainingTestMessage(string testMessage, vsBuildErrorLevel errorLevel)
        {
            // no regex needed, do a fast check
            if (!testMessage.Contains("%f"))
                return _errors.Count(e => e.Description.Contains(testMessage.ToUpper()) && e.ErrorLevel.Equals(errorLevel));

            // convert number placeholders (%f) to a regex that matches floating point values
            testMessage = @".*?" + Regex.Escape(testMessage).Replace("%f", @"[+-]?(\d+([.]\d*)?([eE][+-]?\d+)?|[.]\d+([eE][+-]?\d+)?)") + @".*?";
            return _errors.Count(e => Regex.Match(e.Description, testMessage, RegexOptions.IgnoreCase).Success && e.ErrorLevel.Equals(errorLevel));
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

            string testIdentText = CreateFailedTestCommonString(testName);
            if (AreErrorItemsContainingTestMessage(testIdentText, message, errorLevel))
            {
                log.Info("Test " + _testFunctionBlockInstance + "." + testName + " reports: " + message);
            }
        }

        protected void AssertContainsResultSet(string testName, string className, string status, int numberOfAsserts, double expectedDuration, double expectedDurationTolerance)
        {
            var errorMessagePrefix = $"Test suite {_testFunctionBlockInstance} test {testName} ";
            try
            {
                var results = _errors
                    .Select((e, index) => new { Error = e, Index = index })
                    .Where(item =>
                        item.Error.Description.Contains($"| Test name={testName}".ToUpper()) &&
                        item.Error.ErrorLevel.Equals(vsBuildErrorLevel.vsBuildErrorLevelLow))
                    .Select(item => _errors.Skip(item.Index).Take(3))
                    .FirstOrDefault();


                if (!results.ElementAt(1).Description.Contains($"| Test class name={className}".ToUpper()))
                {
                    log.Info($"{errorMessagePrefix} does not list class name: {className}");
                    return;
                }

                string pattern = @"Test status=(?<Status>\w+), number of asserts=(?<Asserts>\d+), duration=(?<Duration>[\d\.e-]+)";
                Match match = Regex.Match(results.ElementAt(2).Description, pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    log.Info($"{errorMessagePrefix} does not contain expected result set");
                    return;
                }

                var actualStatus = match.Groups["Status"].Value;
                if (actualStatus != status)
                {
                    log.Info($"{errorMessagePrefix} does not have expected status: {actualStatus} != {status}");
                    return;
                }

                var actualNumberOfAsserts = int.Parse(match.Groups["Asserts"].Value);
                if (actualNumberOfAsserts != numberOfAsserts)
                {
                    log.Info($"{errorMessagePrefix} does not have expected number of asserts: {actualNumberOfAsserts} != {numberOfAsserts}");
                    return;
                }

                var actualDuration = double.Parse(match.Groups["Duration"].Value, System.Globalization.CultureInfo.InvariantCulture);
                if (System.Math.Abs(actualDuration - expectedDuration) > expectedDurationTolerance)
                {
                    log.Info($"{errorMessagePrefix} does not have expected duration: abs({actualDuration} - {expectedDuration}) > {expectedDurationTolerance}");
                    return;
                }
            } 
            catch
            {
                log.Info($"{errorMessagePrefix} does not contain expected results");
            }
        }

        protected string CreateFailedTestCommonString(string method)
        {
            string returnString;
            returnString = "FAILED TEST 'PRG_TEST." + _testFunctionBlockInstance + "@" + method + "'";
            return returnString;
        }
    }
}
