using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Reflection;

namespace VFive_Callouts
{
    public class Main : Plugin
    {
        public string pluginversion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("\n-------------------------- VFive Callouts --------------------------\nVersion: " + pluginversion);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("VFive Callouts cleaned up its own shit.");
        }

        private static void OnOnDutyStateChangedHandler(bool duty)
        {
            if (duty)
            {
                RegisterCallouts();
                Game.DisplayNotification("VFive Callouts: \nVersion:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\nStatus: Loaded!");
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.HighSpeedChase));
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }

            return null;
        }

        public static bool isLSPDFRPluginRunning(string Plugin, Version minver = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();

                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minver == null || an.Version.CompareTo(minver) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
