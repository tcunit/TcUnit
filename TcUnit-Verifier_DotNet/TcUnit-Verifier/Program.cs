using NDesk.Options;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;
using System.Threading;
using EnvDTE80;

namespace TcUnit.Verifier
{
    class Program
    {
        private static string tcUnitVerifierPath = null;
        private static VisualStudioInstance vsInstance = null;
        private static ILog log = LogManager.GetLogger("TcUnit-Verifier");


        [STAThread]
        static int Main(string[] args)
        {
            bool showHelp = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);
            log4net.GlobalContext.Properties["LogLocation"] = AppDomain.CurrentDomain.BaseDirectory + "\\logs";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            OptionSet options = new OptionSet()
                .Add("v=|TcUnitVerifierPath=", "Path to TcUnit-Verifier TwinCAT solution", v => tcUnitVerifierPath = v)
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
                log.Error("Error loading VS DTE. Is the correct version of Visual Studio installed?");
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
            automationInterface.ITcSysManager.SetTargetNetId("127.0.0.1.1.1");
            ITcSmTreeItem plcProject = automationInterface.PlcTreeItem.Child[1];
            ITcPlcProject iecProject = (ITcPlcProject)plcProject;

            log.Info("Generating TcUnit-Verifier_TwinCAT boot project...");
            iecProject.GenerateBootProject(true);
            iecProject.BootProjectAutostart = true;

            log.Info("Activating TcUnit-Verifier_TwinCAT configuration...");
            automationInterface.ITcSysManager.ActivateConfiguration();

            log.Info("Restaring TwinCAT...");
            automationInterface.ITcSysManager.StartRestartTwinCAT();

            // Wait until tests have been running and are finished
            bool testsFinishedRunningFirstLineFound = false;
            bool amountOfTestSuitesLineFound = false;
            bool amountOfTestsLineFound = false;
            bool amountOfSuccesfulTestsLineFound = false;
            bool amountOfFailedTestsLineFound = false;
            bool testsFinishedRunningLastLineFound = false;

            log.Info("Waiting for TcUnit-Verifier_TwinCAT to finish running tests...");

            ErrorItems errorItems;
            while (true)
            {
                Thread.Sleep(1000);

                errorItems = vsInstance.GetErrorItems();

                for (int i = 1; i <= errorItems.Count; i++)
                {
                    ErrorItem item = errorItems.Item(i);
                    if (item.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh)
                    {
                        if (item.Description.ToUpper().Contains("| ==========TESTS FINISHED RUNNING=========="))
                            testsFinishedRunningFirstLineFound = true;
                        if (item.Description.ToUpper().Contains("| TEST SUITES:"))
                            amountOfTestSuitesLineFound = true;
                        if (item.Description.ToUpper().Contains("| TESTS:"))
                            amountOfTestsLineFound = true;
                        if (item.Description.ToUpper().Contains("| SUCCESSFUL TESTS:"))
                            amountOfSuccesfulTestsLineFound = true;
                        if (item.Description.ToUpper().Contains("| FAILED TESTS:"))
                            amountOfFailedTestsLineFound = true;
                        if (item.Description.ToUpper().Contains("| ======================================"))
                            testsFinishedRunningLastLineFound = true;
                    }
                }

                if (testsFinishedRunningFirstLineFound && amountOfTestSuitesLineFound && amountOfTestsLineFound && amountOfSuccesfulTestsLineFound
                    && amountOfFailedTestsLineFound && testsFinishedRunningLastLineFound)
                    break;
            }

            log.Info("Asserting results...");

            /* Insert the test classes here */
            FB_PrimitiveTypes primitiveTypes = new FB_PrimitiveTypes(errorItems, "PrimitiveTypes");
            FB_AssertTrueFalse assertTrueFalse = new FB_AssertTrueFalse(errorItems, "AssertTrueFalse");
            FB_AssertEveryFailedTestTwice assertEveryFailedTestTwice = new FB_AssertEveryFailedTestTwice(errorItems, "AssertEveryFailedTestTwice");
            FB_CreateFourTestsWithSameName createFourTestsWithSameName = new FB_CreateFourTestsWithSameName(errorItems, "CreateFourTestsWithSameName");
            FB_ArrayPrimitiveTypes arrayPrimitiveTypes = new FB_ArrayPrimitiveTypes(errorItems, "ArrayPrimitiveTypes");
            FB_CreateDisabledTest createDisabledTest = new FB_CreateDisabledTest(errorItems, "CreateDisabledTest");
            FB_AnyPrimitiveTypes anyPrimitiveTypes = new FB_AnyPrimitiveTypes(errorItems, "AnyPrimitiveTypes");
            FB_AssertEveryFailedTestTwiceArrayVersion assertEveryFailedTestTwiceArrayVersion = new FB_AssertEveryFailedTestTwiceArrayVersion(errorItems, "AssertEveryFailedTestTwiceArrayVersion");
            FB_AnyToUnionValue anyToUnionValue = new FB_AnyToUnionValue(errorItems, "AnyToUnionValue");
            FB_MultipleAssertWithSameParametersInSameCycleWithSameTest multipleAssertWithSameParametersInSameCycleWithSameTest = new FB_MultipleAssertWithSameParametersInSameCycleWithSameTest(errorItems, "MultipleAssertWithSameParametersInSameCycleWithSameTest");
            FB_MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests multipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests = new FB_MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests(errorItems, "MultipleAssertWithSameParametersInDifferentCyclesButWithDifferentTests");
            FB_MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest multipleAssertWithSameParametersInDifferentCyclesAndInSameTest = new FB_MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest(errorItems, "MultipleAssertWithSameParametersInDifferentCyclesAndInSameTest");
            FB_AdjustAssertFailureMessageToMax252CharLengthTest adjustAssertFailureMessageToMax252CharLengthTest = new FB_AdjustAssertFailureMessageToMax252CharLengthTest(errorItems, "AdjustAssertFailureMessageToMax252CharLengthTest");

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
        /// Executed if user interrups the program (i.e. CTRL+C)
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
