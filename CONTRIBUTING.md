# Contributing to TcUnit
We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## We Develop with Github
We use github to host code, to track issues and feature requests, as well as accept pull requests.

## We Use [Github Flow](https://guides.github.com/introduction/flow/index.html), So All Code Changes Happen Through Pull Requests
Pull requests are the best way to propose changes to the codebase (we use [Github Flow](https://guides.github.com/introduction/flow/index.html)). We actively welcome your pull requests:

1. Fork the repo and create your branch from `master`.
2. If you've added new functionality, it's necessary to add tests. For this there is a separate test project called [TcUnit-Verifier](https://github.com/tcunit/TcUnit/tree/master/TcUnit-Verifier). In that project there are several test suites defined to test the different functionality of TcUnit. Please read the [README.MD](https://github.com/tcunit/TcUnit/blob/master/TcUnit-Verifier/README.md) in that project for further instructions. **No new functionality will be accepted without any proper tests**.
3. If you've changed APIs, include the API change in the Pull Request so that the official homepage can be updated.
4. Ensure the test suite passes.
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
* Make sure to edit the project with the same version of Visual Studio as the master branch. All software (TcUnit and TcUnit-Verifier) has been developed using Visual Studio 2013. Note that VS2013 shell (provided with TwinCAT 3.1.4022.x) can't open the TcUnit-Verifier_DotNet (as it's a .NET/C# project). Instead it's recommended to use [VS2013 community edition](https://visualstudio.microsoft.com/vs/older-downloads/) which can be used both for TwinCAT and .NET/C# projects
* Make sure to edit the project with the same build of TwinCAT XAE as the master branch. Which build of TwinCAT that is currently used can be deduced from the [TcUnit.tsproj](https://github.com/tcunit/TcUnit/blob/master/TcUnit/TcUnit.tsproj)-file. The "TcVersion"-attribute will give you the version of TwinCAT that was lastly used to edit the project. The build is the third number from the left (for example 4020, 4022 or 4024)
* Make sure that your TwinCAT development environment uses spaces instead of tabs. The default behaviour of the TwinCAT development environment is to use tabs so it needs to be changed, which can be done according to [this guide](https://alltwincat.com/2017/04/14/replace-tabs-with-whitespaces/)
* The prefixes of naming of function blocks/variables/etc from the [Beckhoff TwinCAT3 identifier/name conventions](https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/18014401873267083.html&id=) are ignored as a modern integrated development environment (as Visual Studio) gives all the hints/information of data types etc
* Make sure to set your TwinCAT development environment to use Separate LineIDs. This is available in the TwinCAT XAE under **Tools→Options→TwinCAT→PLC Environment→Write options→Separate LineIDs** (set this to TRUE, more information is available [here](https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_userinterface/18014403202147467.html&id=))

## License
By contributing, you agree that your contributions will be licensed under its MIT License.

## References
This document was adapted from briandk's excellent [contribution guidelines template](https://gist.github.com/briandk/3d2e8b3ec8daf5a27a62).
