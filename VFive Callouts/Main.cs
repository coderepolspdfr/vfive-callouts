using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using Rage;
using VFiveCallouts.Callouts;

namespace VFiveCallouts
{
    public class Main : Plugin
    {
        public override void Finally() { }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }
        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                });
        }
        private static void RegisterCallouts()
        {
            Game.Console.Print();
            Game.Console.Print("================================================== VFive Callouts ===================================================");
            Game.Console.Print();
            Functions.RegisterCallout(typeof(Assault));
            Game.Console.Print("[LOG]: Loaded Callouts");
            Game.Console.Print();
            Game.Console.Print("================================================== VFive Callouts ===================================================");
        }
    }
}
