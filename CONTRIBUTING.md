# Contributing to TcUnit

We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## We Develop with Github

We use github to host code, to track issues and feature requests, as well as accept pull requests.

## We Use [Github Flow](https://docs.github.com/en/get-started/quickstart/github-flow), So All Code Changes Happen Through Pull Requests

Pull requests are the best way to propose changes to the codebase (we use [Github Flow](https://docs.github.com/en/get-started/quickstart/github-flow)). We actively welcome your pull requests:

1. Fork the repo and create your branch from `master`.
2. If you've added new functionality, it's necessary to add tests. For this there is a separate test project called [TcUnit-Verifier](https://github.com/tcunit/TcUnit/tree/master/TcUnit-Verifier). In that project there are several test suites defined to test the different functionality of TcUnit. Please read the [README.MD](https://github.com/tcunit/TcUnit/blob/master/TcUnit-Verifier/README.md) in that project for further instructions. **No new functionality will be accepted without any proper tests**.
3. Ensure the test suite passes.
4. If changes that affect the documentation have been made (such as API-changes), make sure to update the [documentation](https://github.com/tcunit/TcUnit/tree/master/docs).
5. Issue that pull request!

## Any contributions you make will be under the MIT Software License

In short, when you submit code changes, your submissions are understood to be under the same [MIT License](http://choosealicense.com/licenses/mit/) that covers the project. Feel free to contact the maintainers if that's a concern.

## Report bugs using Github's [issues](https://github.com/tcunit/TcUnit/issues)

We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/tcunit/TcUnit/issues/new); it's that easy!

## Write bug reports with detail, background, and sample code

**Great Bug Reports** tend to have:

- A quick summary and/or background
- Steps to reproduce
  - Be specific!
  - Give sample code if you can.
- What you expected would happen
- What actually happens
- Notes (possibly including why you think this might be happening, or stuff you tried that didn't work)

## Use a Consistent Coding Style

- Make sure to edit the project with the same version of Visual Studio as the master branch. All software (TcUnit and TcUnit-Verifier) has been developed using Visual Studio 2019. Note that the TwinCAT XAE Shell (provided with TwinCAT XAE installer) can't open the TcUnit-Verifier_DotNet (as it's a .NET/C# project). Instead it's recommended to use [VS2019 community edition](https://visualstudio.microsoft.com/vs/older-downloads/) which can be used both for TwinCAT and .NET/C# projects
- Make sure to use TwinCAT XAE version 3.1.4024.44 or later
- The prefixes of naming of function blocks/variables/etc such as the [Beckhoff TwinCAT 3 and PLCopen identifier/name conventions](https://sagatowski.com/posts/plc_naming_conventions/) are ignored as a modern integrated development environment (as Visual Studio) gives all the hints/information of data types etc
- Make sure to set your TwinCAT development environment to use Separate LineIDs. This is available in the TwinCAT XAE under **Tools→Options→TwinCAT→PLC Environment→Write options→Separate LineIDs** (set this to TRUE, more information is available [here](https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_userinterface/18014403202147467.html&id=))

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## References

This document was adapted from briandk's excellent [contribution guidelines template](https://gist.github.com/briandk/3d2e8b3ec8daf5a27a62).
