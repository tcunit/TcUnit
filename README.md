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

Then fill the body of your test suite FB with calls to the test methods. You don't need a `RunTests` method, and you don't need to specify when all the tests are finished, because that is done on a per-test basis (with `TEST_FINISHED()` above)
