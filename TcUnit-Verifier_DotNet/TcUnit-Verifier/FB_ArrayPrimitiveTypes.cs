using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class FB_ArrayPrimitiveTypes : TestFunctionBlockAssert
    {
        public FB_ArrayPrimitiveTypes(IEnumerable<ErrorList.Error> errors, string testFunctionBlockInstance = null) : base(errors, testFunctionBlockInstance)
        {
            Test_BOOL_Array_Equals();
            Test_BOOL_Array_DifferInSize();
            Test_BOOL_Array_DifferInContent();
            Test_BYTE_Array_Equals();
            Test_BYTE_Array_DifferInSize();
            Test_BYTE_Array_DifferInContent();
            Test_DINT_Array_Equals();
            Test_DINT_Array_DifferInSize();
            Test_DINT_Array_DifferInContent();
            Test_DWORD_Array_Equals();
            Test_DWORD_Array_DifferInSize();
            Test_DWORD_Array_DifferInContent();
            Test_INT_Array_Equals();
            Test_INT_Array_DifferInSize();
            Test_INT_Array_DifferInContent();
            Test_LINT_Array_Equals();
            Test_LINT_Array_DifferInSize();
            Test_LINT_Array_DifferInContent();
            Test_LREAL_Array_Equals();
            Test_LREAL_Array_DifferInSize();
            Test_LREAL_Array_DifferInContent();
            Test_LREAL_Array2d_Equals();
            Test_LREAL_Array2d_DifferInSize_D1();
            Test_LREAL_Array2d_DifferInSize_D2();
            Test_LREAL_Array2d_DifferInContent_LBOUND();
            Test_LREAL_Array2d_DifferInContent_Middle();
            Test_LREAL_Array2d_DifferInContent_UBOUND();
            Test_LREAL_Array3d_Equals();
            Test_LREAL_Array3d_DifferInSize_D1();
            Test_LREAL_Array3d_DifferInSize_D2();
            Test_LREAL_Array3d_DifferInSize_D3();
            Test_LREAL_Array3d_DifferInContent_LBOUND();
            Test_LREAL_Array3d_DifferInContent_Middle();
            Test_LREAL_Array3d_DifferInContent_UBOUND();
            Test_LWORD_Array_Equals();
            Test_LWORD_Array_DifferInSize();
            Test_LWORD_Array_DifferInContent();
            Test_REAL_Array_Equals();
            Test_REAL_Array_DifferInSize();
            Test_REAL_Array_DifferInContent();
            Test_REAL_Array2d_Equals();
            Test_REAL_Array2d_DifferInSize_D1();
            Test_REAL_Array2d_DifferInSize_D2();
            Test_REAL_Array2d_DifferInContent_LBOUND();
            Test_REAL_Array2d_DifferInContent_Middle();
            Test_REAL_Array2d_DifferInContent_UBOUND();
            Test_REAL_Array3d_Equals();
            Test_REAL_Array3d_DifferInSize_D1();
            Test_REAL_Array3d_DifferInSize_D2();
            Test_REAL_Array3d_DifferInSize_D3();
            Test_REAL_Array3d_DifferInContent_LBOUND();
            Test_REAL_Array3d_DifferInContent_Middle();
            Test_REAL_Array3d_DifferInContent_UBOUND();
            Test_SINT_Array_Equals();
            Test_SINT_Array_DifferInSize();
            Test_SINT_Array_DifferInContent();
            Test_UDINT_Array_Equals();
            Test_UDINT_Array_DifferInSize();
            Test_UDINT_Array_DifferInContent();
            Test_UINT_Array_Equals();
            Test_UINT_Array_DifferInSize();
            Test_UINT_Array_DifferInContent();
            Test_ULINT_Array_Equals();
            Test_ULINT_Array_DifferInSize();
            Test_ULINT_Array_DifferInContent();
            Test_USINT_Array_Equals();
            Test_USINT_Array_DifferInSize();
            Test_USINT_Array_DifferInContent();
            Test_WORD_Array_Equals();
            Test_WORD_Array_DifferInSize();
            Test_WORD_Array_DifferInContent();
        }

        private void Test_BOOL_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_BOOL_Array_Equals");
        }

        private void Test_BOOL_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_BOOL_Array_DifferInSize", "SIZE = 6", "SIZE = 4", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_BOOL_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_BOOL_Array_DifferInContent", "ARRAY[2] = FALSE", "ARRAY[2] = TRUE", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_BYTE_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_BYTE_Array_Equals");
        }

        private void Test_BYTE_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_BYTE_Array_DifferInSize", "SIZE = 2", "SIZE = 5", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_BYTE_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_BYTE_Array_DifferInContent", "ARRAY[1] = 0xAA", "ARRAY[1] = 0xCD", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_DINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_DINT_Array_Equals");
        }

        private void Test_DINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_DINT_Array_DifferInSize", "SIZE = 2", "SIZE = 3", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_DINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_DINT_Array_DifferInContent", "ARRAY[4] = -2147483645", "ARRAY[4] = -2147483641", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_DWORD_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_DWORD_Array_Equals");
        }

        private void Test_DWORD_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_DWORD_Array_DifferInSize", "SIZE = 4", "SIZE = 2", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_DWORD_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_DWORD_Array_DifferInContent", "ARRAY[-1] = 0xEFAA2346", "ARRAY[-1] = 0xEF012345", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_INT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_INT_Array_Equals");
        }

        private void Test_INT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_INT_Array_DifferInSize", "SIZE = 8", "SIZE = 5", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_INT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_INT_Array_DifferInContent", "ARRAY[-7] = -23", "ARRAY[2] = 24", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_LINT_Array_Equals");
        }

        private void Test_LINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_LINT_Array_DifferInSize", "SIZE = 3", "SIZE = 2", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_LINT_Array_DifferInContent", "ARRAY[0] = -9223372036853775808", "ARRAY[5] = -9223372036854775808", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_LREAL_Array_Equals");
        }

        private void Test_LREAL_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array_DifferInSize", "SIZE = 8", "SIZE = 5", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array_DifferInContent", "ARRAY[-7] = -23.0", "ARRAY[2] = 24.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array2d_Equals()
        {
            AssertDoesNotContainMessage("Test_LREAL_Array2d_Equals");
        }

        private void Test_LREAL_Array2d_DifferInSize_D1()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array2d_DifferInSize_D1", "SIZE = [-4..-2,-1..0] (3x2)", "SIZE = [1..2,0..1] (2x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array2d_DifferInSize_D2()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array2d_DifferInSize_D2", "SIZE = [-4..-3,-1..0] (2x2)", "SIZE = [1..2,0..2] (2x3)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array2d_DifferInContent_LBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array2d_DifferInContent_LBOUND", "ARRAY[-5,-1] = 0.0", "ARRAY[0,3] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array2d_DifferInContent_Middle()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array2d_DifferInContent_Middle", "ARRAY[-4,0] = 0.0", "ARRAY[1,4] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_LREAL_Array2d_DifferInContent_UBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array2d_DifferInContent_UBOUND", "ARRAY[-3,1] = 0.0", "ARRAY[2,5] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array3d_Equals()
        {
            AssertDoesNotContainMessage("Test_LREAL_Array3d_Equals");
        }

        private void Test_LREAL_Array3d_DifferInSize_D1()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInSize_D1", "SIZE = [-5..-4,0..2,-1..0] (2x3x2)", "SIZE = [0..2,3..5,6..7] (3x3x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array3d_DifferInSize_D2()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInSize_D2", "SIZE = [-5..-4,0..2,-1..0] (2x3x2)", "SIZE = [0..1,3..4,6..7] (2x2x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }
        private void Test_LREAL_Array3d_DifferInSize_D3()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInSize_D3", "SIZE = [-5..-4,0..2,-1..0] (2x3x2)", "SIZE = [0..1,3..5,6..6] (2x3x1)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LREAL_Array3d_DifferInContent_LBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInContent_LBOUND", "ARRAY[-5,-1,1] = 0.0", "ARRAY[0,3,5] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_LREAL_Array3d_DifferInContent_Middle()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInContent_Middle", "ARRAY[-4,0,2] = 0.0", "ARRAY[1,4,6] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_LREAL_Array3d_DifferInContent_UBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_LREAL_Array3d_DifferInContent_UBOUND", "ARRAY[-3,1,3] = 0.0", "ARRAY[2,5,7] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_LWORD_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_LWORD_Array_Equals");
        }

        private void Test_LWORD_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_LWORD_Array_DifferInSize", "SIZE = 1", "SIZE = 2", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_LWORD_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_LWORD_Array_DifferInContent", "ARRAY[1] = 0xEDCBA09876543210", "ARRAY[1] = 0x01234567890ABCDE", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_REAL_Array_Equals");
        }

        private void Test_REAL_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array_DifferInSize", "SIZE = 8", "SIZE = 5", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array_DifferInContent", "ARRAY[-7] = -23.0", "ARRAY[2] = 24.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array2d_Equals()
        {
            AssertDoesNotContainMessage("Test_REAL_Array2d_Equals");
        }

        private void Test_REAL_Array2d_DifferInSize_D1()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array2d_DifferInSize_D1", "SIZE = [-4..-2,-1..0] (3x2)", "SIZE = [1..2,0..1] (2x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array2d_DifferInSize_D2()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array2d_DifferInSize_D2", "SIZE = [-4..-3,-1..0] (2x2)", "SIZE = [1..2,0..2] (2x3)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array2d_DifferInContent_LBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array2d_DifferInContent_LBOUND", "ARRAY[-5,-1] = 0.0", "ARRAY[1,0] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array2d_DifferInContent_Middle()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array2d_DifferInContent_Middle", "ARRAY[-4,0] = 0.0", "ARRAY[2,1] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array2d_DifferInContent_UBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array2d_DifferInContent_UBOUND", "ARRAY[-3,1] = 0.0", "ARRAY[3,2] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        
        private void Test_REAL_Array3d_Equals()
        {
            AssertDoesNotContainMessage("Test_REAL_Array3d_Equals");
        }

        private void Test_REAL_Array3d_DifferInSize_D1()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInSize_D1", "SIZE = [-5..-4,1..3,-2..-1] (2x3x2)", "SIZE = [1..1,4..6,6..7] (1x3x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array3d_DifferInSize_D2()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInSize_D2", "SIZE = [-5..-4,1..3,-2..-1] (2x3x2)", "SIZE = [1..2,4..5,6..7] (2x2x2)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array3d_DifferInSize_D3()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInSize_D3", "SIZE = [-5..-4,1..3,-2..-1] (2x3x2)", "SIZE = [1..2,4..6,6..6] (2x3x1)", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_REAL_Array3d_DifferInContent_LBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInContent_LBOUND", "ARRAY[-5,-1,0] = 0.0", "ARRAY[1,3,6] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array3d_DifferInContent_Middle()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInContent_Middle", "ARRAY[-4,0,1] = 0.0", "ARRAY[2,4,7] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }
        private void Test_REAL_Array3d_DifferInContent_UBOUND()
        {
            string testMessage = CreateFailedTestMessage("Test_REAL_Array3d_DifferInContent_UBOUND", "ARRAY[-3,1,2] = 0.0", "ARRAY[3,5,8] = 1.0", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_SINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_SINT_Array_Equals");
        }

        private void Test_SINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_SINT_Array_DifferInSize", "SIZE = 1", "SIZE = 2", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_SINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_SINT_Array_DifferInContent", "ARRAY[0] = -128", "ARRAY[0] = 127", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_UDINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_UDINT_Array_Equals");
        }

        private void Test_UDINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_UDINT_Array_DifferInSize", "SIZE = 2", "SIZE = 3", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_UDINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_UDINT_Array_DifferInContent", "ARRAY[-4] = 5", "ARRAY[1] = 4", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_UINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_UINT_Array_Equals");
        }

        private void Test_UINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_UINT_Array_DifferInSize", "SIZE = 3", "SIZE = 4", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_UINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_UINT_Array_DifferInContent", "ARRAY[3] = 99", "ARRAY[3] = 12", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_ULINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_ULINT_Array_Equals");
        }

        private void Test_ULINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_ULINT_Array_DifferInSize", "SIZE = 2", "SIZE = 1", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_ULINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_ULINT_Array_DifferInContent", "ARRAY[1] = 9400000000000", "ARRAY[1] = 18446744073709551615", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_USINT_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_USINT_Array_Equals");
        }

        private void Test_USINT_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_USINT_Array_DifferInSize", "SIZE = 101", "SIZE = 71", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_USINT_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_USINT_Array_DifferInContent", "ARRAY[4] = 4", "ARRAY[4] = 5", "Arrays differ");
            AssertContainsMessage(testMessage);
        }

        private void Test_WORD_Array_Equals()
        {
            AssertDoesNotContainMessage("Test_WORD_Array_Equals");
        }

        private void Test_WORD_Array_DifferInSize()
        {
            string testMessage = CreateFailedTestMessage("Test_WORD_Array_DifferInSize", "SIZE = 5", "SIZE = 7", "Arrays differ, size of arrays not matching.");
            AssertContainsMessage(testMessage);
        }

        private void Test_WORD_Array_DifferInContent()
        {
            string testMessage = CreateFailedTestMessage("Test_WORD_Array_DifferInContent", "ARRAY[7] = 0x1133", "ARRAY[7] = 0x1122", "Arrays differ");
            AssertContainsMessage(testMessage);
        }


    }
}
