using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_AssertTrueFalse : TestFunctionBlockAssert
    {
        public FB_AssertTrueFalse(ErrorItems errorItems, string testFunctionBlockInstance) : base(errorItems, testFunctionBlockInstance)
        {
            AssertThatINTsAreEqual();
            AssertThatINTsAreNotEqual();
            AssertThatWORDsAreEqual();
            AssertThatWORDsAreNotEqual();
        }

        private void AssertThatINTsAreEqual()
        {
            string testMessage = CreateFailedTestMessage("AssertThatINTsAreEqual", "FALSE", "TRUE", "INTs are equal");
            AssertContainsMessage(testMessage);
        }

        private void AssertThatINTsAreNotEqual()
        {
            string testMessage = CreateFailedTestMessage("AssertThatINTsAreNotEqual", "FALSE", "TRUE", "INTs are equal");
            AssertDoesNotContainMessage(testMessage);
        }

        private void AssertThatWORDsAreEqual()
        {
            string testMessage = CreateFailedTestMessage("AssertThatWORDsAreEqual", "TRUE", "FALSE", "WORDs are not equal");
            AssertContainsMessage(testMessage);
        }

        private void AssertThatWORDsAreNotEqual()
        {
            string testMessage = CreateFailedTestMessage("AssertThatWORDsAreNotEqual", "TRUE", "FALSE", "WORDs are equal");
            AssertDoesNotContainMessage(testMessage);
        }
    }
}
