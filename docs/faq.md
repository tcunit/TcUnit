---
layout: page
title: F.A.Q.
---

## Frequently asked questions
Here you’ll find the most commonly asked questions and their answers.
If you don’t find what you are looking for here, you can look through the:

- [Open](https://github.com/tcunit/TcUnit/issues?q=is%3Aopen+is%3Aissue) and [closed](https://github.com/tcunit/TcUnit/issues?q=is%3Aissue+is%3Aclosed) issues on GitHub
- [Discussions](https://github.com/tcunit/TcUnit/discussions) on GitHub


**Frequently asked questions** (click on link to get to question and answer)  
[How can I run a test across multiple PLC cycles?](#how-can-i-run-a-test-across-multiple-plc-cycles)  
[How can I disable/ignore a test?](#how-can-i-disableignore-a-test)

---


### How can I run a test across multiple PLC cycles?
This can be accomplished by keeping the function block under test as an instance variable of the test suite rather than the test method.
You can download an [example here](https://tcunit.org/temp/TimedTest_1x.zip).
In this example, the `FB_ToBeTested` is instantiated under the test suite (`FB_ToBeTested_Test`), and can thus be controlled over multiple cycles.
Then all that’s necessary to do is to set the condition for when the assertion should be made in the test itself, which in the example is when the `TestSuiteTimer` has elapsed (`TestSuiteTimer.Q`).

**Required TcUnit version:** 1.0 or later

### How can I disable/ignore a test?
Add `DISABLED_` in front of the test name, for example:

```
TEST('DISABLED_ThisTestWillBeIgnored');
 
AssertEquals(Expected := a,
             Actual := b,
             Message := 'A does not equal B');
 
TEST_FINISHED();
```

**Required TcUnit version:** 1.0 or later