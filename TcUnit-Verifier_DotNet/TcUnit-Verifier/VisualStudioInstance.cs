//using log4net;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcUnit.Verifier
{
    class VisualStudioInstance
    {
        private string @filePath = null;
        private string vsVersion = null;
        private EnvDTE80.DTE2 dte = null;
        private Type type = null;
        private EnvDTE.Solution visualStudioSolution = null;
        EnvDTE.Project pro = null;
        //ILog log = LogManager.GetLogger("TcUnit-Verifier");

        public VisualStudioInstance(string @visualStudioSolutionFilePath)
        {
            this.filePath = visualStudioSolutionFilePath;
            string visualStudioVersion = FindVisualStudioVersion();
            this.vsVersion = visualStudioVersion;
            LoadDevelopmentToolsEnvironment(visualStudioVersion);
            LoadSolution(@visualStudioSolutionFilePath);
            LoadProject();
        }

        public VisualStudioInstance(int vsVersionMajor, int vsVersionMinor)
        {
            string visualStudioVersion = vsVersionMajor.ToString() + "." + vsVersionMinor.ToString();
            this.vsVersion = visualStudioVersion;
            LoadDevelopmentToolsEnvironment(visualStudioVersion);
        }

        public void Close()
        {
            dte.Quit();
        }

        private string FindVisualStudioVersion()
        {
            /* Find visual studio version */
            string line;
            string vsVersion = null;

            System.IO.StreamReader file = new System.IO.StreamReader(@filePath);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("VisualStudioVersion"))
                {
                    string version = line.Substring(line.LastIndexOf('=') + 2);
                    //log.Info("In Visual Studio solution file, found visual studio version " + version);
                    string[] numbers = version.Split('.');
                    string major = numbers[0];
                    string minor = numbers[1];

                    int n;
                    int n2;

                    bool isNumericMajor = int.TryParse(major, out n);
                    bool isNumericMinor = int.TryParse(minor, out n2);

                    if (isNumericMajor && isNumericMinor)
                    {
                        vsVersion = major + "." + minor;
                    }
                }
            }
            file.Close();
            return vsVersion;
        }

        private void LoadDevelopmentToolsEnvironment(string visualStudioVersion)
        {
            /* Make sure the DTE loads with the same version of Visual Studio as the
             * TwinCAT project was created in
             */
            string VisualStudioProgId = "VisualStudio.DTE." + visualStudioVersion;
            type = System.Type.GetTypeFromProgID(VisualStudioProgId);
            dte = (EnvDTE80.DTE2) System.Activator.CreateInstance(type);
            dte.UserControl = false; // have devenv.exe automatically close when launched using automation
            dte.SuppressUI = true;
            var tcAutomationSettings = dte.GetObject("TcAutomationSettings");
            tcAutomationSettings.SilentMode = true;
        }

        private void LoadSolution(string filePath)
        {
            visualStudioSolution = dte.Solution;
            visualStudioSolution.Open(@filePath);
        }

        private void LoadProject()
        {
            pro = visualStudioSolution.Projects.Item(1);
        }

        public string GetVisualStudioVersion()
        {
            return this.vsVersion;
        }

        public EnvDTE.Project GetProject()
        {
            return this.pro;
        }

        public EnvDTE80.DTE2 GetDevelopmentToolsEnvironment()
        {
            return this.dte;
        }

        public void CleanAndBuildSolution()
        {
            this.visualStudioSolution.SolutionBuild.Clean(true);
            this.visualStudioSolution.SolutionBuild.Build(true);

        }

        public ErrorItems GetErrorItems()
        {
            return dte.ToolWindows.ErrorList.ErrorItems;
            /*
            ErrorItems errorItems = dte.ToolWindows.ErrorList.ErrorItems;
            ErrorItems returnErrorItems;

            for (int i = 1; i <= errorItems.Count; i++)
            {
                ErrorItem item = errorItems.Item(i);
                if (item.ErrorLevel == vsBuildErrorLevel.vsBuildErrorLevelHigh)
                {
                    returnErrorItems.Item. = item;
                }
            }

            return returnErrorItems;*/
        }

    }
}