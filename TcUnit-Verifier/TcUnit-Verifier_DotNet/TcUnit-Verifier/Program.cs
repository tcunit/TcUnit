using NDesk.Options;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TCatSysManagerLib;
using System.Threading;
using EnvDTE80;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TcUnit.Verifier
{
    class Program
    {
        private static string tcUnitVerifierPath = null;
        private static string tcUnitTargetNetId = "127.0.0.1.1.1";
        private static VisualStudioInstance vsInstance = null;
        private static ILog log = LogManager.GetLogger("TcUnit-Verifier");
        private static int expectedNumberOfFailedTests = 116; // Update this if you add intentionally failing tests

        [STAThread]
        static void Main(string[] args)
        {
            bool showHelp = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);
            GlobalContext.Properties["LogLocation"] = AppDomain.CurrentDomain.BaseDirectory + "\\logs";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            OptionSet options = new OptionSet()
                .Add("v=|TcUnitVerifierPath=", "Path to TcUnit-Verifier TwinCAT solution", v => tcUnitVerifierPath = v)
                .Add("t=|TcUnitTargetNetId=", "[OPTIONAL] Target NetId of TwinCAT runtime to run TcUnit-Verifier", t => tcUnitTargetNetId = t)
                .Add("?|h|help", h => showHelp = h != null);

            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `TcUnit-Verifier --help' for more information.");
                Environment.Exit(Constants.RETURN_ERROR);
            }

            /* Make sure the user has supplied the path for the Visual Studio solution file.
            *  Also verify that this file exists.
            */
            if (showHelp || tcUnitVerifierPath == null)
            {
                DisplayHelp(options);
                Environment.Exit(Constants.RETURN_ERROR);
            }
            if (!File.Exists(tcUnitVerifierPath))
            {
                log.Error("TcUnit-verifier solution " + tcUnitVerifierPath + " does not exist.");
                Environment.Exit(Constants.RETURN_ERROR);
            }

            MessageFilter.Register();

            log.Info("Starting TcUnit-Verifier...");
            try
            {
                vsInstance = new VisualStudioInstance(tcUnitVerifierPath);
                vsInstance.Load();
            }
            catch
            {
                log.Error("Solution load failed");  // Detailed error messages output by vsInstance.Load()
                CleanUp();
                Environment.Exit(Constants.RETURN_ERROR);
            }

            if (vsInstance.GetVisualStudioVersion() == null)
            {
                log.Error("Did not find Visual studio version in Visual studio solution file.");
                CleanUp();
                Environment.Exit(Constants.RETURN_ERROR);
            }

            log.Info("Cleaning and building TcUnit-Verifier_TwinCAT solution...");
            AutomationInterface automationInterface = new AutomationInterface(vsInstance);
            automationInterface.ITcSysManager.SetTargetNetId(tcUnitTargetNetId);
            ITcSmTreeItem plcProject = automationInterface.PlcTreeItem.Child[1];
            ITcPlcProject iecProject = (ITcPlcProject)plcProject;

            log.Info("Generating TcUnit-Verifier_TwinCAT boot project...");
            Thread.Sleep(10000);
            iecProject.GenerateBootProject(true);
            iecProject.BootProjectAutostart = true;

            log.Info("Activating TcUnit-Verifier_TwinCAT configuration...");
            automationInterface.ITcSysManager.ActivateConfiguration();

            /* Clean the solution. This is the only way to clean the error list which needs to be
             * clean prior to starting the TwinCAT runtime */
            vsInstance.CleanSolution();

            // Wait
            Thread.Sleep(1000);

            log.Info("Restarting TwinCAT...");
            automationInterface.ITcSysManager.StartRestartTwinCAT();

            // Wait until tests have been running and are finished
            bool testsFinishedRunningFirstLineFound = false;
            bool numberOfTestSuitesLineFound = false;
            bool numberOfTestsLineFound = false;
            bool numberOfSuccesfulTestsLineFound = false;
            bool numberOfFailedTestsLineFound = false;
            bool durationLineFound = false;
            bool testsFinishedRunningLastLineFound = false;
            int numberOfFailedTests = 0;
            float duration;

            const string durationStr = "| Duration:";

            log.Info("Waiting for TcUnit-Verifier_TwinCAT to finish running tests...");

            ErrorList errorList = new ErrorList();

            ErrorItems errorItems;
            while (true)
            {
                Thread.Sleep(10000);

                errorItems = vsInstance.GetErrorItems();
                log.Info("... got " + errorItems.Count + " report lines so far.");
                for (int i = 1; i <= errorItems.Count; i++)
                {
                    ErrorItem error = errorItems.Item(i);
                    if (error.Description.Contains("| ==========TESTS FINISHED RUNNING=========="))
                        testsFinishedRunningFirstLineFound = true;
                    if (error.Description.Contains("| Test suites:"))
                        numberOfTestSuitesLineFound = true;
                    if (error.Description.Contains("| Tests:"))
                        numberOfTestsLineFound = true;
                    if (error.Description.Contains("| Successful tests:"))
                        numberOfSuccesfulTestsLineFound = true;
                    if (error.Description.Contains("| Failed tests:"))
                    {
                        numberOfFailedTestsLineFound = true;
                        // Grab the number of failed tests so we can validate it during the assertion phase
                        numberOfFailedTests = int.Parse(error.Description.Split().Last());
                    }
                    if (error.Description.Contains(durationStr))
                    {
                        int durationIndex = error.Description.IndexOf(durationStr);
                        durationLineFound = float.TryParse(error.Description.Substring(durationIndex + durationStr.Length), NumberStyles.Any, CultureInfo.InvariantCulture, out duration);
                    }
                    if (error.Description.Contains("| ======================================"))
                        testsFinishedRunningLastLineFound = true;
                }

                if (
                    testsFinishedRunningFirstLineFound 
                    && numberOfTestSuitesLineFound 
                    && numberOfTestsLineFound 
                    && numberOfSuccesfulTestsLineFound
                    && numberOfFailedTestsLineFound
                    && durationLineFound
                    && testsFinishedRunningLastLineFound
                )
                    break;

            }
            var newErrors = errorList.AddNew(errorItems);

            log.Info("Asserting results...");

            if (numberOfFailedTests != expectedNumberOfFailedTests)
            {
                log.Error(
                    "The number of tests that failed (" + numberOfFailedTests + ") " +
                    "does not match expectations (" + expectedNumberOfFailedTests + ")"
                );
            }

            List<ErrorList.Error> errors = new List<ErrorList.Error>(
                errorList.Where(e => (
                    e.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh 
                    || e.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelLow)
                )
            );

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
            new FB_EmptyAssertionMessage(errors);

            log.Info("Done.");

            CleanUp();

            Environment.Exit(Constants.RETURN_SUCCESSFULL);
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
            Environment.Exit(Constants.RETURN_SUCCESSFULL);
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
