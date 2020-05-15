using NDesk.Options;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TCatSysManagerLib;
using System.Threading;
using EnvDTE80;

namespace TcUnit.Verifier
{
    class Program
    {
        private static string tcUnitVerifierPath = null;
        private static string tcUnitTargetNetId = "127.0.0.1.1.1";
        private static VisualStudioInstance vsInstance = null;
        private static ILog log = LogManager.GetLogger("TcUnit-Verifier");
        private static int expectedNumberOfFailedTests = 112; // Update this if you add intentionally failing tests

        [STAThread]
        static int Main(string[] args)
        {
            bool showHelp = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);
            log4net.GlobalContext.Properties["LogLocation"] = AppDomain.CurrentDomain.BaseDirectory + "\\logs";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            OptionSet options = new OptionSet()
                .Add("v=|TcUnitVerifierPath=", "Path to TcUnit-Verifier TwinCAT solution", v => tcUnitVerifierPath = v)
                .Add("t=|TcUnitTargetNetId=", "(Optional, default 127.0.0.1.1.1) Target NetId of TwinCAT runtime to deploy TcUnit-Verifier PLC to", t => tcUnitTargetNetId = t)
                .Add("?|h|help", h => showHelp = h != null);

            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `TcUnit-Verifier --help' for more information.");
                return Constants.RETURN_ERROR;
            }

            /* Make sure the user has supplied the path for the Visual Studio solution file.
            * Also verify that this file exists.
            */
            if (showHelp || tcUnitVerifierPath == null)
            {
                DisplayHelp(options);
                return Constants.RETURN_ERROR;
            }
            if (!File.Exists(tcUnitVerifierPath))
            {
                log.Error("TcUnit-verifier solution " + tcUnitVerifierPath + " does not exist.");
                return Constants.RETURN_ERROR;
            }

            MessageFilter.Register();

            log.Info("Starting TcUnit-Verifier...");
            try
            {
                vsInstance = new VisualStudioInstance(@tcUnitVerifierPath);
                vsInstance.Load();
            }
            catch
            {
                log.Error("Solution load failed");  // Detailed error messages output by vsInstance.Load()
                CleanUp();
                return Constants.RETURN_ERROR;
            }

            if (vsInstance.GetVisualStudioVersion() == null)
            {
                log.Error("Did not find Visual studio version in Visual studio solution file.");
                CleanUp();
                return Constants.RETURN_ERROR;
            }

            log.Info("Cleaning and building TcUnit-Verifier_TwinCAT solution...");
            AutomationInterface automationInterface = new AutomationInterface(vsInstance);
            automationInterface.ITcSysManager.SetTargetNetId(tcUnitTargetNetId);
            ITcSmTreeItem plcProject = automationInterface.PlcTreeItem.Child[1];
            ITcPlcProject iecProject = (ITcPlcProject)plcProject;

            log.Info("Generating TcUnit-Verifier_TwinCAT boot project...");
            System.Threading.Thread.Sleep(10000);
            iecProject.GenerateBootProject(true);
            iecProject.BootProjectAutostart = true;

            log.Info("Activating TcUnit-Verifier_TwinCAT configuration...");
            automationInterface.ITcSysManager.ActivateConfiguration();

            /* Clean the solution. This is the only way to clean the error list which needs to be
             * clean prior to starting the TwinCAT runtime */
            vsInstance.CleanSolution();

            // Wait
            System.Threading.Thread.Sleep(1000);

            log.Info("Restarting TwinCAT...");
            automationInterface.ITcSysManager.StartRestartTwinCAT();

            // Wait until tests have been running and are finished
            bool testsFinishedRunningFirstLineFound = false;
            bool amountOfTestSuitesLineFound = false;
            bool amountOfTestsLineFound = false;
            bool amountOfSuccesfulTestsLineFound = false;
            bool amountOfFailedTestsLineFound = false;
            bool testsFinishedRunningLastLineFound = false;
            int numberOfFailedTests = 0;

            log.Info("Waiting for TcUnit-Verifier_TwinCAT to finish running tests...");

            ErrorList errorList = new ErrorList();

            while (true)
            {
                Thread.Sleep(10000);

                ErrorItems errorItems = vsInstance.GetErrorItems();
                log.Info("... got " + errorItems.Count + " report lines so far.");

                var newErrors = errorList.AddNew(errorItems);

                foreach (var error in newErrors.Where(e => e.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh))
                {
                    if (error.Description.Contains("| ==========TESTS FINISHED RUNNING=========="))
                        testsFinishedRunningFirstLineFound = true;
                    if (error.Description.Contains("| TEST SUITES:"))
                        amountOfTestSuitesLineFound = true;
                    if (error.Description.Contains("| TESTS:"))
                        amountOfTestsLineFound = true;
                    if (error.Description.Contains("| SUCCESSFUL TESTS:"))
                        amountOfSuccesfulTestsLineFound = true;
                    if (error.Description.Contains("| FAILED TESTS:"))
                    {
                        amountOfFailedTestsLineFound = true;
                        // Grab the number of failed tests so we can validate it during the assertion phase
                        numberOfFailedTests = Int32.Parse(error.Description.Split().Last());
                    }
                    if (error.Description.Contains("| ======================================"))
                        testsFinishedRunningLastLineFound = true;
                }

                if (testsFinishedRunningFirstLineFound && amountOfTestSuitesLineFound && amountOfTestsLineFound && amountOfSuccesfulTestsLineFound
                    && amountOfFailedTestsLineFound && testsFinishedRunningLastLineFound)
                    break;
            }

            log.Info("Asserting results...");

            if (numberOfFailedTests != expectedNumberOfFailedTests)
            {
                log.Error("The number of tests that failed (" + numberOfFailedTests + ") does not match expectations (" + expectedNumberOfFailedTests + ")");
            }

            List<ErrorList.Error> errors = new List<ErrorList.Error>(errorList.Where(e => (e.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh || e.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelLow)));

            /* Insert the test classes here */
            new FB_PrimitiveTypes(errors);
            new FB_ExtendedTestInformation(errors);
            new FB_AssertTrueFalse(errors);
            new FB_AssertEveryFailedTestTwice(errors);
            new FB_CreateFourTestsWithSameName(errors);
            new FB_ArrayPrimitiveTypes(errors);
            new FB_CreateDisabledTest(errors);
            new FB_AnyPrimitiveTypes(errors);
            new FB_AssertEveryFailedTestTwiceArrayVersion(errors);
            new FB_AnyToUnionValue(errors);
            new FB_MultipleAssertWithSameParametersInSameCycleWithSameTest(errors);
            new FB_MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests(errors);
            new FB_MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest(errors);
            new FB_SkipAssertionsWhenFinished(errors);
            new FB_AdjustAssertFailureMessageToMax253CharLengthTest(errors);
            new FB_CheckIfSpecificTestIsFinished(errors);
            new FB_WriteProtectedFunctions(errors);
            new FB_TestFileControl(errors);
            new FB_TestXmlControl(errors);
            new FB_TestStreamBuffer(errors);
            new FB_TestFinishedNamed(errors);
            new FB_TestNumberOfAssertionsCalculation(errors);

            log.Info("Done.");

            CleanUp();

            return Constants.RETURN_SUCCESSFULL;
        }

        private static void DisplayHelp(OptionSet p)
        {
            Console.WriteLine("Usage: TcUnit-Verifier [OPTIONS]");
            Console.WriteLine("Runs TcUnit-Verifier TwinCAT solution, collect results from it and verifies the results are as expected");
            Console.WriteLine("Example: TcUnit-Verifier -v \"C:\\Code\\workspace\\TcUnit-Verifier\\TcUnit-Verifier_TwinCAT\\TcUnit-Verifier_TwinCAT.sln\"");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// Executed if user interrupts the program (i.e. CTRL+C)
        /// </summary>
        static void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs args)
        {
            log.Info("Application interrupted by user");
            CleanUp();
            Environment.Exit(0);
        }

        /// <summary>
        /// Cleans the system resources (the VS DTE)
        /// </summary>
        private static void CleanUp()
        {
            try
            {
                vsInstance.Close();
            }
            catch { }

            log.Info("Exiting application...");
            MessageFilter.Revoke();
        }
    }
}
