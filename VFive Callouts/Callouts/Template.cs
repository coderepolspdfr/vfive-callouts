using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;

namespace VFiveCallouts.Callouts
{
    [CalloutInfo("HighSpeedChase", CalloutProbability.Never)]

    public class Template : Callout
    {
        // vehicles peds declare here for callout

        public override bool OnBeforeCalloutDisplayed()
        {
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
            base.End();
        }
    }
}
