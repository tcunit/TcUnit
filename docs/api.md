---
layout: page
title: API
---

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

### `AssertArrayEquals_BOOL`

```StructuredText
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

- `Expecteds` – BOOL array with expected values
- `Actuals` – BOOL array with actual values
- `Message` – The identifying message for the assertion error

**Positive example:**

```StructuredText
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

```StructuredText
VAR
    a : ARRAY[1..6] OF BOOL := [TRUE, TRUE, TRUE, TRUE, TRUE, TRUE];
    b : ARRAY[1..4] OF BOOL := [TRUE, TRUE, TRUE, TRUE];
END_VAR
 
TEST('Test_BYTE_Array_DiffersInSize');
AssertArrayEquals_BOOL(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```

**Failing example #2:**

```StructuredText
VAR
    a : ARRAY[0..5] OF BOOL := [TRUE, TRUE, FALSE, TRUE, FALSE, TRUE];
    b : ARRAY[0..5] OF BOOL := [TRUE, TRUE, TRUE, TRUE, FALSE, FALSE];
END_VAR
 
TEST('Test_BYTE_Array_DiffersInContent');
AssertArrayEquals_BOOL(Expecteds := a,
                       Actuals := b,
                       Message := 'Arrays differ');
TEST_FINISHED();
```
