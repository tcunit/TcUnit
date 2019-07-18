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
            try
            {
                vsInstance = new VisualStudioInstance(@tcUnitVerifierPath);
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
            //vsInstance.CleanAndBuildSolution();
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
            FB_PrimitiveTypes_Test_Asserter primitiveTypes_Test = new FB_PrimitiveTypes_Test_Asserter(errorItems, "PrimitiveTypesTest");
            FB_AssertTrueFalse_Test_Asserter assertTrueFalse_Test = new FB_AssertTrueFalse_Test_Asserter(errorItems, "AssertTrueFalse_Test");
            FB_AssertEveryFailedTestTwice_Asserter assertEveryFailedTestTwice = new FB_AssertEveryFailedTestTwice_Asserter(errorItems, "AssertEveryFailedTestTwice");
            FB_CreateFourTestsWithSameName createFourTestsWithSameName = new FB_CreateFourTestsWithSameName(errorItems, "CreateFourTestsWithSameName");
            FB_ArrayPrimitiveTypes_Test_Asserter arrayPrimitiveTypes_Test = new FB_ArrayPrimitiveTypes_Test_Asserter(errorItems, "ArrayPrimitiveTypes_Test");
            FB_CreateDisabledTest_Asserter createDisabledTest = new FB_CreateDisabledTest_Asserter(errorItems, "CreateDisabledTest");
            FB_AnyPrimitiveTypes_Test anyPrimitiveTypesTest = new FB_AnyPrimitiveTypes_Test(errorItems, "AnyPrimitiveTypes_Test");
            FB_AssertEveryFailedTestTwiceArrayVersion_Asserter assertEveryFailedTestTwiceArrayVersion = new FB_AssertEveryFailedTestTwiceArrayVersion_Asserter(errorItems, "AssertEveryFailedTestTwice_ArrayVersion");
            FB_AnyToUnionValue_Test_Asserter anyToUnionValue_Test_Asserter = new FB_AnyToUnionValue_Test_Asserter(errorItems, "AnyToUnionValue_Test");

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

        private static void CleanUp()
        {
            try
            {
                vsInstance.Close();
            }
            catch { }
            MessageFilter.Revoke();
        }
    }
}
