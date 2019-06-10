# TcUnit - TwinCAT unit testing framework

This fork of TcUnit contains a number of quality of life upgrades, but breaks backwards compatibility with the current version of the official TcUnit. These improvements include:

- Reduced boilerplate
- Easier to use with multi-cycle tests

I recommend following the official TcUnit starting guide, with the following exceptions:

For each test suite FB, boilerplate is reduced down to:

- Add the `{attribute 'call_after_init'}` pragma
- Add `EXTENDS TcUnit.FB_TestSuite` inheritance

Then create a method for each test (as normal in TcUnit), except:

- Start it with `TEST('TestMethodName');` (as normal)
- End it with `TEST_FINISHED();` -- this makes it much easier to run multi-cycle tests
- Call assert methods directly, like `AssertTrue_BOOL(MyVariable, 'My message.');`. There is no standalone assert FB

Then fill the body of your test suite FB with calls to your test methods. You don't need a `RunTests` method, and you don't need to specify when all the tests are finished from your test suite, because that is done on a per-test basis (with `TEST_FINISHED()` above)

Here's an example test suite that has one normal test method and one multi-cycle test method:

MyTestSuite
------------------
```
{attribute 'call_after_init'}
FUNCTION_BLOCK MyTestSuite EXTENDS TcUnit.FB_TestSuite
VAR
    CycleTestCounter: UINT := 0;
END_VAR
==================================================================
Test_OnePlusOne();
Test_OneHundredCycles(ADR(CycleTestCounter));
```

MyTestSuite.Test_OnePlusOne method
----------------------------------------
```
METHOD Test_OnePlusOne
VAR
    Result: UINT;
END_VAR
==================================================================
TEST('Test_OnePlusOne');
Result := 1 + 1;
AssertEquals_UINT(2, Result, '1 + 1 = 2');
TEST_FINISHED();
```

MyTestSuite.Test_OneHundredCycles method
----------------------------------------
```
METHOD Test_OneHundredCycles
VAR_INPUT
    Counter: POINTER TO UINT;
END_VAR
==================================================================
TEST('Test_OneHundredCycles');
Counter^ := Counter^ + 1;
// This doesn't actually assert anything, but you get the idea
IF Counter^ >= 100 THEN
    TEST_FINISHED();
END_IF
```
