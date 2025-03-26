# API

<p align="center">
  <img width="1024" src="./img/tc3_banner5.jpg">
</p>

This is the application programming interface for TcUnit.

- [`FB_TestSuite`](#fb_testsuite)

## FB_TestSuite

This function block is responsible for holding the internal state of the test suite.
Every test suite can have one or more tests, and every test can do one or more asserts.
It's also responsible for providing all the assert-methods for asserting different data types.
Only failed assertions are recorded.

**Method summary:**

- [AssertArrayEquals_BOOL](#assertarrayequals_bool)`(Expecteds : ARRAY[*] OF BOOL, Actuals : ARRAY[*] OF BOOL, Message : T_MaxString)`  
*Asserts that two BOOL arrays are equal.*
- [AssertArrayEquals_BYTE](#assertarrayequals_byte)`(Expecteds : ARRAY[*] OF BYTE, Actuals : ARRAY[*] OF BYTE, Message : T_MaxString)`  
*Asserts that two BYTE arrays are equal.*
- [AssertArrayEquals_DINT](#assertarrayequals_dint)`(Expecteds : ARRAY[*] OF DINT, Actuals : ARRAY[*] OF DINT, Message : T_MaxString)`  
*Asserts that two DINT arrays are equal.*
- [AssertArrayEquals_DWORD](#assertarrayequals_dword)`(Expecteds : ARRAY[*] OF DWORD, Actuals : ARRAY[*] OF DWORD, Message : T_MaxString)`  
*Asserts that two DWORD arrays are equal.*
- [AssertArrayEquals_INT](#assertarrayequals_int)`(Expecteds : ARRAY[*] OF INT, Actuals : ARRAY[*] OF INT, Message : T_MaxString)`  
*Asserts that two INT arrays are equal.*
- [AssertArrayEquals_LINT](#assertarrayequals_lint)`(Expecteds : ARRAY[*] OF LINT, Actuals : ARRAY[*] OF LINT, Message : T_MaxString)`  
*Asserts that two LINT arrays are equal.*
- [AssertArrayEquals_LWORD](#assertarrayequals_lword)`(Expecteds : ARRAY[*] OF LWORD, Actuals : ARRAY[*] OF LWORD, Message : T_MaxString)`  
*Asserts that two LWORD arrays are equal.*
- [AssertArrayEquals_SINT](#assertarrayequals_sint)`(Expecteds : ARRAY[*] OF SINT, Actuals : ARRAY[*] OF SINT, Message : T_MaxString)`  
*Asserts that two SINT arrays are equal.*
- [AssertArrayEquals_UDINT](#assertarrayequals_udint)`(Expecteds : ARRAY[*] OF UDINT, Actuals : ARRAY[*] OF UDINT, Message : T_MaxString)`  
*Asserts that two UDINT arrays are equal.*
- [AssertArrayEquals_UINT](#assertarrayequals_uint)`(Expecteds : ARRAY[*] OF UINT, Actuals : ARRAY[*] OF UINT, Message : T_MaxString)`  
*Asserts that two UINT arrays are equal.*
- [AssertArrayEquals_ULINT](#assertarrayequals_ulint)`(Expecteds : ARRAY[*] OF ULINT, Actuals : ARRAY[*] OF ULINT, Message : T_MaxString)`  
*Asserts that two ULINT arrays are equal.*
- [AssertArrayEquals_USINT](#assertarrayequals_usint)`(Expecteds : ARRAY[*] OF USINT, Actuals : ARRAY[*] OF USINT, Message : T_MaxString)`  
*Asserts that two USINT arrays are equal.*
- [AssertArrayEquals_WORD](#assertarrayequals_word)`(Expecteds : ARRAY[*] OF WORD, Actuals : ARRAY[*] OF WORD, Message : T_MaxString)`  
*Asserts that two WORD arrays are equal.*
- [AssertEquals](#assertequals)`(Expected : ANY, Actual : ANY, Message : T_MaxString)`  
*Asserts that two objects (of any type) are equal.*
- [AssertEquals_BOOL](#assertequals_bool)`(Expected : BOOL, Actual : BOOL, Message : T_MaxString)`  
*Asserts that two BOOLs are equal.*
- [AssertEquals_BYTE](#assertequals_byte)`(Expected : BYTE, Actual : BYTE, Message : T_MaxString)`  
*Asserts that two BYTEs are equal.*
- [AssertEquals_DATE](#assertequals_date)`(Expected : DATE, Actual : DATE, Message : T_MaxString)`  
*Asserts that two DATEs are equal.*
- [AssertEquals_DATE_AND_TIME](#assertequals_date_and_time)`(Expected : DATE_AND_TIME, Actual : DATE_AND_TIME, Message : T_MaxString)`  
*Asserts that two DATE_AND_TIMEs are equal.*
- [AssertEquals_DINT](#assertequals_dint)`(Expected : DINT, Actual : DINT, Message : T_MaxString)`  
*Asserts that two DINTs are equal.*
- [AssertEquals_DWORD](#assertequals_dword)`(Expected : DWORD, Actual : DWORD, Message : T_MaxString)`  
*Asserts that two DWORDs are equal.*
- [AssertEquals_INT](#assertequals_int)`(Expected : INT, Actual : INT, Message : T_MaxString)`  
*Asserts that two INTs are equal.*
- [AssertEquals_LINT](#assertequals_lint)`(Expected : LINT, Actual : LINT, Message : T_MaxString)`  
*Asserts that two LINTs are equal.*
- [AssertEquals_LREAL](#assertequals_lreal)`(Expected : LREAL, Actual : LREAL, Delta : LREAL, Message : T_MaxString)`  
*Asserts that two LREALs are equal to within a positive delta.*
- [AssertEquals_LTIME](#assertequals_ltime)`(Expected : LTIME, Actual : LTIME, Message : T_MaxString)`  
*Asserts that two LTIMEs are equal.*
- [AssertEquals_LWORD](#assertequals_lword)`(Expected : LWORD, Actual : LWORD, Message : T_MaxString)`  
*Asserts that two LWORDs are equal.*
- [AssertEquals_REAL](#assertequals_real)`(Expected : REAL, Actual : REAL, Delta : REAL, Message : T_MaxString)`  
*Asserts that two REALs are equal to within a positive delta.*
- [AssertEquals_SINT](#assertequals_sint)`(Expected : SINT, Actual : SINT, Message : T_MaxString)`  
*Asserts that two SINTs are equal.*
- [AssertEquals_STRING](#assertequals_string)`(Expected : T_MaxString, Actual : T_MaxString, Message : T_MaxString)`  
*Asserts that two STRINGs are equal.*
- [AssertEquals_TIME](#assertequals_time)`(Expected : TIME, Actual : TIME, Message : T_MaxString)`  
*Asserts that two TIMEs are equal.*
- [AssertEquals_TIME_OF_DAY](#assertequals_time_of_day)`(Expected : TIME_OF_DAY, Actual : TIME_OF_DAY, Message : T_MaxString)`  
*Asserts that two TIME_OF_DAYs are equal.*
- [AssertEquals_UDINT](#assertequals_udint)`(Expected : UDINT, Actual : UDINT, Message : T_MaxString)`  
*Asserts that two UDINTs are equal.*
- [AssertEquals_UINT](#assertequals_uint)`(Expected : UINT, Actual : UINT, Message : T_MaxString)`  
*Asserts that two UINTs are equal.*
- [AssertEquals_ULINT](#assertequals_ulint)`(Expected : ULINT, Actual : ULINT, Message : T_MaxString)`  
*Asserts that two ULINTs are equal.*
- [AssertEquals_USINT](#assertequals_usint)`(Expected : USINT, Actual : USINT, Message : T_MaxString)`  
*Asserts that two USINTs are equal.*
- [AssertEquals_WORD](#assertequals_word)`(Expected : WORD, Actual : WORD, Message : T_MaxString)`  
*Asserts that two WORDs are equal.*
- [AssertEquals_WSTRING](#assertequals_wstring)`(Expected : WSTRING, Actual : WSTRING, Message : T_MaxString)`  
*Asserts that two WSTRINGs are equal.*
- [AssertFalse](#assertfalse)`(Condition : BOOL, Message : T_MaxString)`  
*Asserts that a condition is false.*
- [AssertTrue](#asserttrue)`(Condition : BOOL, Message : T_MaxString)`  
*Asserts that a condition is true.*

### AssertArrayEquals_BOOL

```declaration
METHOD PUBLIC AssertArrayEquals_BOOL
VAR_IN_OUT
    Expecteds : ARRAY[*] OF BOOL;
    Actuals : ARRAY[*] OF BOOL;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two BOOL arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - BOOL array with expected values
- `Actuals` - BOOL array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[1..5] OF BOOL := [TRUE, FALSE, TRUE, FALSE, TRUE];
    b : ARRAY[1..5] OF BOOL := [TRUE, FALSE, TRUE, FALSE, TRUE];
END_VAR
-------
TEST('Test_BOOL_Array_Equals');
AssertArrayEquals_BOOL(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**  

```example
VAR
    a : ARRAY[1..6] OF BOOL := [TRUE, TRUE, TRUE, TRUE, TRUE, TRUE];
    b : ARRAY[1..4] OF BOOL := [TRUE, TRUE, TRUE, TRUE];
END_VAR
-------
TEST('Test_BYTE_Array_DiffersInSize');
AssertArrayEquals_BOOL(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..5] OF BOOL := [TRUE, TRUE, FALSE, TRUE, FALSE, TRUE];
    b : ARRAY[0..5] OF BOOL := [TRUE, TRUE, TRUE, TRUE, FALSE, FALSE];
END_VAR
-------
TEST('Test_BYTE_Array_DiffersInContent');
AssertArrayEquals_BOOL(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_BYTE

```declaration
METHOD PUBLIC AssertArrayEquals_BYTE
VAR_IN_OUT
    Expecteds : ARRAY[*] OF BYTE;
    Actuals : ARRAY[*] OF BYTE;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two BYTE arrays are equal.
If they are not, an assertion error is created.

Parameters:

- `Expecteds` - BYTE array with expected values
- `Actuals` - BYTE array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[1..3] OF BYTE := [16#FD, 16#34, 16#9E];
    b : ARRAY[1..3] OF BYTE := [16#FD, 16#34, 16#9E];
END_VAR
-------
TEST('Test_BYTE_Array_Equals');
AssertArrayEquals_BYTE(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[1..2] OF BYTE := [16#AB, 16#CD];
    b : ARRAY[1..5] OF BYTE := [16#AB, 16#CD, 16#EF, 16#01, 16#23];
END_VAR
-------
TEST('Test_BYTE_Array_DiffersInSize');
AssertArrayEquals_BYTE(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..3] OF BYTE := [16#AB, 16#AA, 16#BB, 16#89];
    b : ARRAY[0..3] OF BYTE := [16#AB, 16#CD, 16#BB, 16#89];
END_VAR
-------
TEST('Test_BYTE_Array_DiffersInContent');
AssertArrayEquals_BYTE(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_DINT

```declaration
METHOD PUBLIC AssertArrayEquals_DINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF DINT;
    Actuals : ARRAY[*] OF DINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two DINT arrays are equal.
If they are not, an assertion error is created.

Parameters:

- `Expecteds` - DINT array with expected values
- `Actuals` - DINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[2..7] OF DINT := [64, 98, 2147483647, -2147483648, 0, -63987538];
    b : ARRAY[2..7] OF DINT := [64, 98, 2147483647, -2147483648, 0, -63987538];
END_VAR
-------
TEST('Test_DINT_Array_Equals');
AssertArrayEquals_DINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[3..4] OF DINT := [-11, 2147483647];
    b : ARRAY[4..6] OF DINT := [-11, 2147483647, 0];
END_VAR
-------
TEST('Test_DINT_Array_DifferInSize');
AssertArrayEquals_DINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[3..5] OF DINT := [-4746, -2147483645, 0];
    b : ARRAY[3..5] OF DINT := [-4746, -2147483641, 0];
END_VAR
-------
TEST('Test_DINT_Array_DifferInContent');
AssertArrayEquals_DINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_DWORD

```declaration
METHOD PUBLIC AssertArrayEquals_DWORD
VAR_IN_OUT
    Expecteds : ARRAY[*] OF DWORD;
    Actuals : ARRAY[*] OF DWORD;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two DWORD arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - DWORD array with expected values
- `Actuals` - DWORD array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[2..3] OF DWORD := [16#6789ABCD, 16#EF012345];
    b : ARRAY[1..2] OF DWORD := [16#6789ABCD, 16#EF012345];
END_VAR
-------
TEST('Test_DWORD_Array_Equals');
AssertArrayEquals_DWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[-2..1] OF DWORD := [16#6789ABCD, 16#EF012345, 16#67890ABC, 16#DDDDDDDD];
    b : ARRAY[-3..-2] OF DWORD := [16#6789ABCD, 16#EF012345];
END_VAR
-------
TEST('Test_DWORD_Array_DifferInSize');
AssertArrayEquals_DWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[-2..1] OF DWORD := [16#6789ABCD, 16#EFAA2346, 16#ABABABAB, 16#EEEEEEEE];
    b : ARRAY[-2..1] OF DWORD := [16#6789ABCD, 16#EF012345, 16#ABABABAB, 16#EEEEEEEE];
END_VAR
-------
TEST('Test_DWORD_Array_DifferInContent');
AssertArrayEquals_DWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_INT

```declaration
METHOD PUBLIC AssertArrayEquals_INT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF INT;
    Actuals : ARRAY[*] OF INT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two INT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - INT array with expected values
- `Actuals` - INT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[-5..1] OF INT := [64, 98, -32768, 32767, 5478, -378, 42];
    b : ARRAY[1..7] OF INT := [64, 98, -32768, 32767, 5478, -378, 42];
END_VAR
-------
TEST('Test_INT_Array_Equals');
AssertArrayEquals_INT(Expecteds := a,
                      Actuals := b,
                      Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[-4..3] OF INT := [64, 98, -32768, 32767, 5478, -378, 42, 6234];
    b : ARRAY[1..5] OF INT := [64, 98, -32768, 32767, 5478];
END_VAR
-------
TEST('Test_INT_Array_DifferInSize');
AssertArrayEquals_INT(Expecteds := a,
                      Actuals := b,
                      Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[-8..-6] OF INT := [42, -23, -32768];
    b : ARRAY[1..3] OF INT := [42, 24, -32768];
END_VAR
-------
TEST('Test_INT_Array_DifferInContent');
AssertArrayEquals_INT(Expecteds := a,
                      Actuals := b,
                      Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_LINT

```declaration
METHOD PUBLIC AssertArrayEquals_LINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF LINT;
    Actuals : ARRAY[*] OF LINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two LINT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - LINT array with expected values
- `Actuals` - LINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[-1..0] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_854_775_808];
    b : ARRAY[4..5] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_854_775_808];
END_VAR
-------
TEST('Test_LINT_Array_Equals');
AssertArrayEquals_LINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[-1..1] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_854_775_808, 55];
    b : ARRAY[4..5] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_854_775_808];
END_VAR
-------
TEST('Test_LINT_Array_DifferInSize');
AssertArrayEquals_LINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[-1..1] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_853_775_808, 55];
    b : ARRAY[4..6] OF LINT := [9_223_372_036_854_775_807, -9_223_372_036_854_775_808, 55];
END_VAR
-------
TEST('Test_LINT_Array_DifferInContent');
AssertArrayEquals_LINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_LWORD

```declaration
METHOD PUBLIC AssertArrayEquals_LWORD
VAR_IN_OUT
    Expecteds : ARRAY[*] OF LWORD;
    Actuals : ARRAY[*] OF LWORD;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two LWORD arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - LWORD array with expected values
- `Actuals` - LWORD array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[1..2] OF LWORD := [16#01234567890ABCDE, 16#EDCBA09876543210];
    b : ARRAY[1..2] OF LWORD := [16#01234567890ABCDE, 16#EDCBA09876543210];
END_VAR
-------
TEST('Test_LWORD_Array_Equals');
AssertArrayEquals_LWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[1..1] OF LWORD := [16#EDCBA09876543210];
    b : ARRAY[1..2] OF LWORD := [16#01234567890ABCDE, 16#EDCBA09876543210];
END_VAR
-------
TEST('Test_LWORD_Array_DifferInSize');
AssertArrayEquals_LWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[1..1] OF LWORD := [16#EDCBA09876543210];
    b : ARRAY[1..1] OF LWORD := [16#01234567890ABCDE];
END_VAR
-------
TEST('Test_LWORD_Array_DifferInContent');
AssertArrayEquals_LWORD(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_SINT

```declaration
METHOD PUBLIC AssertArrayEquals_SINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF SINT;
    Actuals : ARRAY[*] OF SINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two SINT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - SINT array with expected values
- `Actuals` - SINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[0..3] OF SINT := [-128, 127, -34, 62];
    b : ARRAY[0..3] OF SINT := [-128, 127, -34, 62];
END_VAR
-------
TEST('Test_SINT_Array_Equals');
AssertArrayEquals_SINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[0..0] OF SINT := [-128];
    b : ARRAY[0..1] OF SINT := [-128, 127];
END_VAR
-------
TEST('Test_SINT_Array_DifferInSize');
AssertArrayEquals_SINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..0] OF SINT := [-128];
    b : ARRAY[0..0] OF SINT := [127];
END_VAR
-------
TEST('Test_SINT_Array_DifferInContent');
AssertArrayEquals_SINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_UDINT

```declaration
METHOD PUBLIC AssertArrayEquals_UDINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF UDINT;
    Actuals : ARRAY[*] OF UDINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two UDINT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - UDINT array with expected values
- `Actuals` - UDINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[1..3] OF UDINT := [0, 4294967295, 5000];
    b : ARRAY[1..3] OF UDINT := [0, 4294967295, 5000];
END_VAR
-------
TEST('Test_UDINT_Array_Equals');
AssertArrayEquals_UDINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[-5..-4] OF UDINT := [4294967295, 0];
    b : ARRAY[0..2] OF UDINT := [4294967295, 0, 5000];
END_VAR
-------
TEST('Test_UDINT_Array_DifferInSize');
AssertArrayEquals_UDINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[-5..-4] OF UDINT := [4294967295, 5];
    b : ARRAY[0..1] OF UDINT := [4294967295, 4];
END_VAR
-------
TEST('Test_UDINT_Array_DifferInContent');
AssertArrayEquals_UDINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_UINT

```declaration
METHOD PUBLIC AssertArrayEquals_UINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF UINT;
    Actuals : ARRAY[*] OF UINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two UINT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - UINT array with expected values
- `Actuals` - UINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[0..4] OF UINT := [0, 65535, 2000, 34123, 59];
    b : ARRAY[0..4] OF UINT := [0, 65535, 2000, 34123, 59];
END_VAR
-------
TEST('Test_UINT_Array_Equals');
AssertArrayEquals_UINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[0..2] OF UINT := [0, 4, 8];
    b : ARRAY[0..3] OF UINT := [0, 4, 8, 12];
END_VAR
-------
TEST('Test_UINT_Array_DifferInSize');
AssertArrayEquals_UINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..3] OF UINT := [0, 4, 8, 99];
    b : ARRAY[0..3] OF UINT := [0, 4, 8, 12];
END_VAR
-------
TEST('Test_UINT_Array_DifferInContent');
AssertArrayEquals_UINT(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_ULINT

```declaration
METHOD PUBLIC AssertArrayEquals_ULINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF ULINT;
    Actuals : ARRAY[*] OF ULINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two ULINT arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - ULINT array with expected values
- `Actuals` - ULINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[0..3] OF ULINT := [0, 18_446_744_073_709_551_615, 9_400_000_000_000, 3_213_000_444_000];
    b : ARRAY[0..3] OF ULINT := [0, 18_446_744_073_709_551_615, 9_400_000_000_000, 3_213_000_444_000];
END_VAR
-------
TEST('Test_ULINT_Array_Equals');
AssertArrayEquals_ULINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[0..1] OF ULINT := [0, 9_400_000_000_000];
    b : ARRAY[0..0] OF ULINT := [0];
END_VAR
-------
TEST('Test_ULINT_Array_DifferInSize');
AssertArrayEquals_ULINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..1] OF ULINT := [3_213_000_444_000, 9_400_000_000_000];
    b : ARRAY[0..1] OF ULINT := [3_213_000_444_000, 18_446_744_073_709_551_615];
END_VAR
-------
TEST('Test_ULINT_Array_DifferInContent');
AssertArrayEquals_ULINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_USINT

```declaration
METHOD PUBLIC AssertArrayEquals_USINT
VAR_IN_OUT
    Expecteds : ARRAY[*] OF USINT;
    Actuals : ARRAY[*] OF USINT;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two USINT arrays are equal.
If they are not, an assertion error is created.

Parameters:

- `Expecteds` - USINT array with expected values
- `Actuals` - USINT array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[0..100] OF USINT := [42, 100(33)];
    b : ARRAY[0..100] OF USINT := [42, 100(33)];
END_VAR
-------
TEST('Test_USINT_Array_Equals');
AssertArrayEquals_USINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[0..100] OF USINT := [101(42)];
    b : ARRAY[0..70] OF USINT := [71(42)];
END_VAR
-------
TEST('Test_USINT_Array_DifferInSize');
AssertArrayEquals_USINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```example
VAR
    a : ARRAY[0..10] OF USINT := [0,1,2,3,6(4),5];
    b : ARRAY[0..10] OF USINT := [0,1,2,3,6(5),6];
END_VAR
-------
TEST('Test_USINT_Array_DifferInContent');
AssertArrayEquals_USINT(Expecteds := a,
                        Actuals := b,
                        Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertArrayEquals_WORD

```declaration
METHOD PUBLIC AssertArrayEquals_WORD
VAR_IN_OUT
    Expecteds : ARRAY[*] OF WORD;
    Actuals : ARRAY[*] OF WORD;
END_VAR
VAR_INPUT
    Message : T_MaxString;
END_VAR
```

Asserts that two WORD arrays are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expecteds` - WORD array with expected values
- `Actuals` - WORD array with actual values
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ARRAY[1..5] OF WORD := [16#AAAA, 16#BBBB, 16#CCCC, 16#DDDD, 16#EEEE];
    b : ARRAY[1..5] OF WORD := [16#AAAA, 16#BBBB, 16#CCCC, 16#DDDD, 16#EEEE];
END_VAR
-------
TEST('Test_WORD_Array_Equals');
AssertArrayEquals_WORD(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #1:**

```example
VAR
    a : ARRAY[1..5] OF WORD := [16#0000, 16#3333, 16#5555, 16#7777, 16#BBBB];
    b : ARRAY[1..7] OF WORD := [16#0000, 16#3333, 16#5555, 16#7777, 16#BBBB, 16#FFFF, 16#1122];
END_VAR
-------
TEST('Test_WORD_Array_DifferInSize');
AssertArrayEquals_WORD(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

```example
VAR
    a : ARRAY[1..7] OF WORD := [16#2323, 16#876A, 16#4CD4, 16#F3DC, 16#BBBB, 16#FFFF, 16#1133];
    b : ARRAY[1..7] OF WORD := [16#2323, 16#876A, 16#4CD4, 16#F3DC, 16#BBBB, 16#FFFF, 16#1122];
END_VAR
-------
TEST('Test_WORD_Array_DifferInContent');
AssertArrayEquals_WORD(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

### AssertEquals

```declaration
METHOD PUBLIC AssertEquals
VAR_INPUT
    Expected : ANY;
    Actual : ANY;
    Message : T_MaxString;
END_VAR
```

Asserts that two objects (of any type) are equal.
If they are not, an assertion error is created.
For `REAL` and `LREAL` it's recommended to use the [AssertEquals_REAL](#assertequals_real) or [AssertEquals_LREAL](#assertequals_lreal) respectively as these give the possibility to specify a delta between the expected and actual value.

**Parameters:**

- `Expected` - Expected value
- `Actual` - Actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : INT := 5;
    b : INT := 5;
END_VAR
-------
TEST('Test_ANY_Equals');
AssertEquals(Expected := a,
             Actual := b,
             Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : INT := 5;
    b : UINT := 5;
END_VAR
-------
TEST('Test_ANY_Differ_DataType');
AssertEquals(Expected := a,
             Actual := b,
             Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_BOOL

```declaration
METHOD PUBLIC AssertEquals_BOOL
VAR_INPUT
    Expected : BOOL;
    Actual : BOOL;
    Message : T_MaxString;
END_VAR
```

Asserts that two BOOLs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - BOOL expected value
- `Actual` - BOOL actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : BOOL := TRUE;
    b : BOOL := TRUE;
END_VAR
-------
TEST('Test_BOOL_Equals');
AssertEquals_BOOL(Expected := a,
                  Actual := b,
                  Message := 'Bools differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : BOOL := TRUE;
    b : BOOL := FALSE;
END_VAR
-------
TEST('Test_BOOL_Differ');
AssertEquals_BOOL(Expected := a,
                  Actual := b,
                  Message := 'Bools differ');
TEST_FINISHED();
```

### AssertEquals_BYTE

```declaration
METHOD PUBLIC AssertEquals_BYTE
VAR_INPUT
    Expected : BYTE;
    Actual : BYTE;
    Message : T_MaxString;
END_VAR
```

Asserts that two BYTEs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - BYTE expected value
- `Actual` - BYTE actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : BYTE := 16#CD;
    b : BYTE := 16#CD;
END_VAR
-------
TEST('Test_BYTE_Equals');
AssertEquals_BYTE(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : BYTE := 16#AB;
    b : BYTE := 16#CD;
END_VAR
-------
TEST('Test_BYTE_Differ');
AssertEquals_BYTE(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_DATE

```declaration
METHOD PUBLIC AssertEquals_DATE
VAR_INPUT
    Expected : DATE;
    Actual : DATE;
    Message : T_MaxString;
END_VAR
```

Asserts that two DATEs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - DATE expected value
- `Actual` - DATE actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : DATE := DATE#1996-05-06;
    b : DATE := DATE#1996-05-06;
END_VAR
-------
TEST('Test_DATE_Equals');
AssertEquals_DATE(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : DATE := DATE#1996-05-06;
    b : DATE := DATE#2019-01-20;
END_VAR
-------
TEST('Test_DATE_Differ');
AssertEquals_DATE(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_DATE_AND_TIME

```declaration
METHOD PUBLIC AssertEquals_DATE_AND_TIME
VAR_INPUT
    Expected : DATE_AND_TIME;
    Actual : DATE_AND_TIME;
    Message : T_MaxString;
END_VAR
```

Asserts that two DATE_AND_TIMEs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - DATE_AND_TIME expected value
- `Actual` - DATE_AND_TIME actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : DATE_AND_TIME := DATE_AND_TIME#2019-01-20-13:54:30;
    b : DATE_AND_TIME := DATE_AND_TIME#2019-01-20-13:54:30;
END_VAR
-------
TEST('Test_DATE_AND_TIME_Equals');
AssertEquals_DATE_AND_TIME(Expected := a,
                           Actual := b,
                           Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : DATE_AND_TIME := DATE_AND_TIME#1996-05-06-15:36:30;
    b : DATE_AND_TIME := DATE_AND_TIME#1972-03-29-00:00:00;
END_VAR
-------
TEST('Test_DATE_AND_TIME_Differ');
AssertEquals_DATE_AND_TIME(Expected := a,
                           Actual := b,
                           Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_DINT

```declaration
METHOD PUBLIC AssertEquals_DINT
VAR_INPUT
    Expected : DINT;
    Actual : DINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two DINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - BYTE expected value
- `Actual` - BYTE actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : DINT := -80000;
    b : DINT := -80000;
END_VAR
-------
TEST('Test_DINT_Equals');
AssertEquals_DINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : DINT := -55555;
    b : DINT := 70000;
END_VAR
-------
TEST('Test_DINT_Differ');
AssertEquals_DINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_DWORD

```declaration
METHOD PUBLIC AssertEquals_DWORD
VAR_INPUT
    Expected : DWORD;
    Actual : DWORD;
    Message : T_MaxString;
END_VAR
```

Asserts that two DWORDs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - DWORD expected value
- `Actual` - DWORD actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : DWORD := 16#7890ABCD;
    b : DWORD := 16#7890ABCD;
END_VAR
-------
TEST('Test_DWORD_Equals');
AssertEquals_DWORD(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : DWORD := 16#12345678;
    b : DWORD := 16#90ABCDEF;
END_VAR
-------
TEST('Test_DWORD_Differ');
AssertEquals_DWORD(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_INT

```declaration
METHOD PUBLIC AssertEquals_INT
VAR_INPUT
    Expected : INT;
    Actual : INT;
    Message : T_MaxString;
END_VAR
```

Asserts that two INTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - INT expected value
- `Actual` - INT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : INT := -12345;
    b : INT := -12345;
END_VAR
-------
TEST('Test_INT_Equals');
AssertEquals_INT(Expected := a,
                 Actual := b,
                 Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : INT := -32000;
    b : INT := 15423;
END_VAR
-------
TEST('Test_INT_Differ');
AssertEquals_INT(Expected := a,
                 Actual := b,
                 Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_LINT

```declaration
METHOD PUBLIC AssertEquals_LINT
VAR_INPUT
    Expected : LINT;
    Actual : LINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two LINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- Expected - LINT expected value
- Actual - LINT actual value
- Message - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : LINT := -123456789;
    b : LINT := -123456789;
END_VAR
-------
TEST('Test_LINT_Equals');
AssertEquals_LINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : LINT := -451416345;
    b : LINT := 589532453;
END_VAR
-------
TEST('Test_LINT_Differ');
AssertEquals_LINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_LREAL

```declaration
METHOD PUBLIC AssertEquals_LREAL
VAR_INPUT
    Expected : LREAL;
    Actual : LREAL;
    Delta : LREAL;
    Message : T_MaxString;
END_VAR
```

Asserts that two LREALs are equal to within a positive delta.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - LREAL expected value
- `Actual` - LREAL actual value
- `Delta` - The maximum delta between the absolute value of expected and actual for which both numbers are still considered equal
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : LREAL := 1234567.89;
    b : LREAL := 1234567.76;
END_VAR
-------
TEST('Test_LREAL_Equals');
AssertEquals_LREAL(Expected := a,
                   Actual := b,
                   Delta := 0.2,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : LREAL := 1234567.89;
    b : LREAL := 1234567.76;
END_VAR
-------
TEST('Test_LREAL_Differ');
AssertEquals_LREAL(Expected := a,
                   Actual := b,
                   Delta := 0.1,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_LTIME

```declaration
METHOD PUBLIC AssertEquals_LTIME
VAR_INPUT
    Expected : LTIME;
    Actual : LTIME;
    Message : T_MaxString;
END_VAR
```

Asserts that two LTIMEs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - LTIME expected value
- `Actual` - LTIME actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : LTIME := LTIME#213503D23H34M33S709MS551US615NS;
    b : LTIME := LTIME#213503D23H34M33S709MS551US615NS;
END_VAR
-------
TEST('Test_LTIME_Equals');
AssertEquals_LTIME(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : LTIME := LTIME#213503D23H34M33S709MS551US615NS;
    b : LTIME := LTIME#1000D15H23M12S34MS2US44NS;
END_VAR
-------
TEST('Test_LTIME_Differ');
AssertEquals_LTIME(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_LWORD

```declaration
METHOD PUBLIC AssertEquals_LWORD
VAR_INPUT
    Expected : LWORD;
    Actual : LWORD;
    Message : T_MaxString;
END_VAR
```

Asserts that two LWORDs are equal.
If they are not, an assertion error is created.

**Parameters:**

- Expected - LWORD expected value
- Actual - LWORD actual value
- Message - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : LWORD := 16#0123456789ABCDEF;
    b : LWORD := 16#0123456789ABCDEF;
END_VAR
-------
TEST('Test_LWORD_Equals');
AssertEquals_LWORD(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : LWORD := 16#0123656789ABCBEC;
    b : LWORD := 16#0123256789ABCAEE;
END_VAR
-------
TEST('Test_LWORD_Differ');
AssertEquals_LWORD(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_REAL

```declaration
METHOD PUBLIC AssertEquals_REAL
VAR_INPUT
    Expected : REAL;
    Actual : REAL;
    Delta : REAL;
    Message : T_MaxString;
END_VAR
```

Asserts that two REALs are equal to within a positive delta.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - REAL expected value
- `Actual` - REAL actual value
- `Delta` - The maximum delta between the absolute value of expected and actual for which both numbers are still considered equal
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : REAL := 1234.5;
    b : REAL := 1234.4;
END_VAR
-------
TEST('Test_REAL_Equals');
AssertEquals_REAL(Expected := a,
                  Actual := b,
                  Delta := 0.1,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : REAL := 1234.5;
    b : REAL := 1234.4;
END_VAR
-------
TEST('Test_REAL_Differ');
AssertEquals_REAL(Expected := a,
                  Actual := b,
                  Delta := 0.05,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_SINT

```declaration
METHOD PUBLIC AssertEquals_SINT
VAR_INPUT
    Expected : SINT;
    Actual : SINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two SINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - SINT expected value
- `Actual` - SINT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : SINT := -128;
    b : SINT := -128;
END_VAR
-------
TEST('Test_SINT_Equals');
AssertEquals_SINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : SINT := 127;
    b : SINT := -30;
END_VAR
-------
TEST('Test_SINT_Differ');
AssertEquals_SINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_STRING

```declaration
METHOD PUBLIC AssertEquals_STRING
VAR_INPUT
    Expected : T_MaxString;
    Actual : T_MaxString;
    Message : T_MaxString;
END_VAR
```

Asserts that two STRINGs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - STRING expected value
- `Actual` - STRING actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : STRING := 'Hello there';
    b : STRING := 'Hello there';
END_VAR
 
TEST('Test_STRING_Equals');
AssertEquals_STRING(Expected := a,
                    Actual := b,
                    Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : STRING := 'This is a string';
    b : STRING := 'This is another string';
END_VAR
 
TEST('Test_STRING_Differ');
AssertEquals_STRING(Expected := a,
                    Actual := b,
                    Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_TIME

```declaration
METHOD PUBLIC AssertEquals_TIME
VAR_INPUT
    Expected : TIME;
    Actual : TIME;
    Message : T_MaxString;
END_VAR
```

Asserts that two TIMEs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - TIME expected value
- `Actual` - TIME actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : TIME := T#11H34M13S244MS;
    b : TIME := T#11H34M13S244MS;
END_VAR
 
TEST('Test_TIME_Equals');
AssertEquals_TIME(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : TIME := T#12H34M15S10MS;
    b : TIME := T#11H34M13S244MS;
END_VAR
 
TEST('Test_TIME_Differ');
AssertEquals_TIME(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_TIME_OF_DAY

```declaration
METHOD PUBLIC AssertEquals_TIME_OF_DAY
VAR_INPUT
    Expected : TIME_OF_DAY;
    Actual : TIME_OF_DAY;
    Message : T_MaxString;
END_VAR
```

Asserts that two TIME_OF_DAYs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - TIME_OF_DAY expected value
- `Actual` - TIME_OF_DAY actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : TIME_OF_DAY := TIME_OF_DAY#06:21:11.492;
    b : TIME_OF_DAY := TIME_OF_DAY#06:21:11.492;
END_VAR
-------
TEST('Test_TIME_OF_DAY_Equals');
AssertEquals_TIME_OF_DAY(Expected := a,
                         Actual := b,
                         Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : TIME_OF_DAY := TIME_OF_DAY#15:36:30.123;
    b : TIME_OF_DAY := TIME_OF_DAY#06:21:11.492;
END_VAR
-------
TEST('Test_TIME_OF_DAY_Differ');
AssertEquals_TIME_OF_DAY(Expected := a,
                         Actual := b,
                         Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_UDINT

```declaration
METHOD PUBLIC AssertEquals_UDINT
VAR_INPUT
    Expected : UDINT;
    Actual : UDINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two UDINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected - UDINT expected value
- `Actual` - UDINT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : UDINT := 21845123;
    b : UDINT := 21845123;
END_VAR
-------
TEST('Test_UDINT_Equals');
AssertEquals_UDINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : UDINT := 34124214;
    b : UDINT := 52343244;
END_VAR
-------
TEST('Test_UDINT_Differ');
AssertEquals_UDINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_UINT

```declaration
METHOD PUBLIC AssertEquals_UINT
VAR_INPUT
    Expected : UINT;
    Actual : UINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two UINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - UINT expected value
- `Actual` - UINT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```declaration
VAR
    a : UINT := 65535;
    b : UINT := 65535;
END_VAR
-------
TEST('Test_UINT_Equals');
AssertEquals_UINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : UINT := 64322;
    b : UINT := 32312;
END_VAR
-------
TEST('Test_UINT_Differ');
AssertEquals_UINT(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_ULINT

```declaration
METHOD PUBLIC AssertEquals_ULINT
VAR_INPUT
    Expected : ULINT;
    Actual : ULINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two ULINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - ULINT expected value
- `Actual` - ULINT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : ULINT := 45683838383;
    b : ULINT := 45683838383;
END_VAR
-------
TEST('Test_ULINT_Equals');
AssertEquals_ULINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : ULINT := 10000;
    b : ULINT := 53685437234;
END_VAR
-------
TEST('Test_ULINT_Differ');
AssertEquals_ULINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_USINT

```declaration
METHOD PUBLIC AssertEquals_USINT
VAR_INPUT
    Expected : USINT;
    Actual : USINT;
    Message : T_MaxString;
END_VAR
```

Asserts that two USINTs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - USINT expected value
- `Actual` - USINT actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : USINT := 5;
    b : USINT := 5;
END_VAR
-------
TEST('Test_USINT_Equals');
AssertEquals_USINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : USINT := 3;
    b : USINT := 7;
END_VAR
-------
TEST('Test_USINT_Differ');
AssertEquals_USINT(Expected := a,
                   Actual := b,
                   Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_WORD

```declaration
METHOD PUBLIC AssertEquals_WORD
VAR_INPUT
    Expected : WORD;
    Actual : WORD;
    Message : T_MaxString;
END_VAR
```

Asserts that two WORDs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - WORD expected value
- `Actual` - WORD actual value
- `Message` - The identifying message for the assertion error

Positive example:

```example
VAR
    a : WORD := 16#ABCD;
    b : WORD := 16#ABCD;
END_VAR
-------
TEST('Test_WORD_Equals');
AssertEquals_WORD(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : WORD := 16#EF01;
    b : WORD := 16#2345;
END_VAR
-------
TEST('Test_WORD_Differ');
AssertEquals_WORD(Expected := a,
                  Actual := b,
                  Message := 'Values differ');
TEST_FINISHED();
```

### AssertEquals_WSTRING

```declaration
METHOD PUBLIC AssertEquals_WSTRING
VAR_INPUT
    Expected : WSTRING(255);
    Actual : WSTRING(255);
    Message : T_MaxString;
END_VAR
```

Asserts that two WSTRINGs are equal.
If they are not, an assertion error is created.

**Parameters:**

- `Expected` - WSTRING expected value
- `Actual` - WSTRING actual value
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : WSTRING := "ĠĦ";
    b : WSTRING := "ĠĦ";
END_VAR
-------
TEST('Test_WSTRING_Equals');
AssertEquals_WSTRING(Expected := a,
                     Actual := b,
                     Message := 'Values differ');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : WSTRING := "äö";
    b : WSTRING := "æå";
END_VAR
-------
TEST('Test_WSTRING_Differ');
AssertEquals_WSTRING(Expected := a,
                     Actual := b,
                     Message := 'Values differ');
TEST_FINISHED();
```

### AssertFalse

```declaration
METHOD PUBLIC AssertFalse
VAR_INPUT
    Condition: BOOL;
    Message : T_MaxString;
END_VAR
```

Asserts that a condition is false.
If it is not, an assertion error is created.

**Parameters:**

- `Condition` - Condition to be checked
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : INT := -33;
    b : INT := -44;
END_VAR
-------
TEST('AssertThatINTsAreNotEqual');
AssertFalse(Condition := (a = b),
            Message := 'INTs are equal');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : INT := -33;
    b : INT := -44;
END_VAR
-------
TEST('AssertThatINTsAreEqual');
AssertFalse(Condition := (a <> b),
            Message := 'INTs are equal');
TEST_FINISHED();
```

### AssertTrue

```declaration
METHOD PUBLIC AssertTrue
VAR_INPUT
    Condition: BOOL;
    Message : T_MaxString;
END_VAR
```

Asserts that a condition is true.
If it is not, an assertion error is created.

**Parameters:**

- `Condition` - Condition to be checked
- `Message` - The identifying message for the assertion error

**Positive example:**

```example
VAR
    a : WORD := 16#ABCD;
    b : WORD := 16#0123;
END_VAR
-------
TEST('AssertThatWORDsAreNotEqual');
AssertTrue(Condition := (a <> b),
           Message := 'WORDs are equal');
TEST_FINISHED();
```

**Failing example:**

```example
VAR
    a : WORD := 16#ABCD;
    b : WORD := 16#0123;
END_VAR
-------
TEST('AssertThatWORDsAreEqual');
AssertTrue(Condition := (a = b),
           Message := 'WORDs are not equal');
TEST_FINISHED();
```
