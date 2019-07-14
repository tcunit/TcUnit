using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;

namespace TcUnit.Verifier
{
    class AutomationInterface
    {
        private ITcSysManager13 sysManager = null;
        private ITcConfigManager configManager = null;
        private ITcSmTreeItem plcTreeItem = null;
        private ITcSmTreeItem routesTreeItem = null;


        public AutomationInterface(EnvDTE.Project project)
        {
            sysManager = project.Object;
            configManager = (ITcConfigManager)sysManager.ConfigurationManager;
            plcTreeItem = sysManager.LookupTreeItem(Constants.PLC_CONFIGURATION_SHORTCUT);
            routesTreeItem = sysManager.LookupTreeItem(Constants.RT_CONFIG_ROUTE_SETTINGS_SHORTCUT);
        }

        public AutomationInterface(VisualStudioInstance vsInst) : this(vsInst.GetProject())
        { }

        public ITcSysManager13 ITcSysManager
        {
            get
            {
                return this.sysManager;
            }
        }

        public ITcSmTreeItem PlcTreeItem
        {
            get
            {
                return this.plcTreeItem;
            }
        }

        public ITcSmTreeItem RoutesTreeItem
        {
            get
            {
                return this.routesTreeItem;
            }
        }

        public string ActiveTargetPlatform
        {
            set
            {
                this.configManager.ActiveTargetPlatform = value;
            }
            get
            {
                return this.configManager.ActiveTargetPlatform;
            }
        }

        public string TargetNetId
        {
            set
            {
                this.sysManager.SetTargetNetId(value);
            }
            get
            {
                return sysManager.GetTargetNetId();
            }
        }
    }
}
