using EnvDTE80;
using log4net;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TCatSysManagerLib;

namespace TcUnit.Verifier
{
    /// <summary>
    /// This class is used to instantiate the Visual Studio Development Tools Environment (DTE)
    /// which is used to programatically access all the functions in VS.
    /// </summary>
    class VisualStudioInstance
    {
        private string @filePath = null;
        private string vsVersion = null;
        private EnvDTE80.DTE2 dte = null;
        private Type type = null;
        private EnvDTE.Solution visualStudioSolution = null;
        EnvDTE.Project pro = null;
        ILog log = LogManager.GetLogger("TcUnit-Verifier");
        private bool loaded = false;

        public VisualStudioInstance(string @visualStudioSolutionFilePath)
        {
            this.filePath = visualStudioSolutionFilePath;
            string visualStudioVersion = FindVisualStudioVersion();
            this.vsVersion = visualStudioVersion;
        }

        public VisualStudioInstance(int vsVersionMajor, int vsVersionMinor)
        {
            string visualStudioVersion = vsVersionMajor.ToString() + "." + vsVersionMinor.ToString();
            this.vsVersion = visualStudioVersion;
        }

        /// <summary>
        /// Loads the development tools environment
        /// </summary>
        public void Load()
        {
            loaded = true;

            try
            {
                LoadDevelopmentToolsEnvironment(vsVersion);
            }
            catch (Exception e)
            {
                string message = string.Format(
                    "{0} Error loading VS DTE version {1}. Is the correct version of Visual Studio installed?",
                    e.Message, vsVersion);
                log.Error(message);
                throw;
            }
            
            if (!String.IsNullOrEmpty(@filePath))
            {
                try
                {
                    LoadSolution(@filePath);
                    LoadProject();
                }
                catch (Exception e)
                {
                    string message = string.Format(
                        "{0} Error loading solution at \"{1}\". Is the path correct?", 
                        e.Message, filePath);
                    log.Error(message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Closes the DTE and makes sure the VS process is completely shutdown
        /// </summary>
        public void Close()
        {
            if (loaded)
            {
                log.Info("Closing the Visual Studio Development Tools Environment (DTE), please wait...");
                Thread.Sleep(20000); // Makes sure that there are no visual studio processes left in the system if the user interrupts this program (for example by CTRL+C)
                dte.Quit();
            }
            loaded = false;
        }

        /// <summary>
        /// Opens the main *.sln-file and finds the version of VS used for creation of the solution
        /// </summary>
        /// <returns>The version of Visual Studio used to create the solution</returns>
        private string FindVisualStudioVersion()
        {
            string file;
            try
            {
                file = File.ReadAllText(@filePath);
            }
            catch (ArgumentException)
            {
                return null;
            }

            string pattern = @"^VisualStudioVersion\s+=\s+(?<version>\d+\.\d+)";
            Match match = Regex.Match(file, pattern, RegexOptions.Multiline);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        private void LoadDevelopmentToolsEnvironment(string visualStudioVersion)
        {
            /* Make sure the DTE loads with the same version of Visual Studio as the
             * TwinCAT project was created in
             */
            string VisualStudioProgId = "VisualStudio.DTE." + visualStudioVersion;
            type = System.Type.GetTypeFromProgID(VisualStudioProgId);
            log.Info("Loading the Visual Studio Development Tools Environment (DTE)...");
            dte = (EnvDTE80.DTE2) System.Activator.CreateInstance(type);
            dte.UserControl = false; // have devenv.exe automatically close when launched using automation
            dte.SuppressUI = true;
            dte.ToolWindows.ErrorList.ShowErrors = true;
            dte.ToolWindows.ErrorList.ShowMessages = true;
            dte.ToolWindows.ErrorList.ShowWarnings = true;
            var tcAutomationSettings = dte.GetObject("TcAutomationSettings");
            tcAutomationSettings.SilentMode = true;
            // Uncomment this if you want to run a specific version of TwinCAT
            ITcRemoteManager remoteManager = dte.GetObject("TcRemoteManager");
            remoteManager.Version = "3.1.4022.30";
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

        /// <returns>Returns null if no version was found</returns>
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

        public void CleanSolution()
        {
            visualStudioSolution.SolutionBuild.Clean(true);
        }

        public ErrorItems GetErrorItems()
        {
            return dte.ToolWindows.ErrorList.ErrorItems;
        }
    }
}
