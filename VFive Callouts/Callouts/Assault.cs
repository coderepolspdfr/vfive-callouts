using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine;
using StopThePed;

namespace VFive_Callouts.Callouts
{
    [CalloutInfo("Simple Assault", CalloutProbability.Medium)]

    public class Assault : Callout
    {
        // vehicles peds declare here for callout
        private Ped _Attacker, _Victim;
        private Vector3 _Spawnpoint;
        private Blip _AttackerBlip, _VictimBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            _Spawnpoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 500f));
            ShowCalloutAreaBlipBeforeAccepting(_Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _Spawnpoint);
            CalloutMessage = "Simple Assault";
            CalloutPosition = _Spawnpoint;

            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CITIZENS_REPORT_02 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION", CalloutPosition);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.DisplayNotification("~bold~~r~DISPATCH REPORT\n~w~Multiple witnesses report a citizen has been assaulted.");
            _Attacker = new Ped();
            _Attacker.IsPersistent = true;
            _AttackerBlip = new Blip(_Attacker);
            _Attacker.Inventory.GiveNewWeapon("WEAPON_KNIFE", 1, false);
            _Attacker.Inventory.GiveNewWeapon("WEAPON_PIPEBOMB", 1, false);
            StopThePed.API.Functions.setPedAlcoholOverLimit(_Attacker, true);
            StopThePed.API.Functions.setPedUnderDrugsInfluence(_Attacker, true);
            //StopThePed.API.Functions.injectPedSearchItems(_Attacker);

            _Victim = new Ped();
            _Victim.IsPersistent = true;
            _VictimBlip = new Blip(_Victim);

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!_Victim || !_Attacker)
            {
                End();
            }
            
            if (_Attacker.IsDead || _Victim.IsDead)
            {
                End();
            }

            base.Process();
        }

        public override void End()
        {
            if (_Victim) _Victim.Dismiss();
            if (_Attacker) _Attacker.Dismiss();
            if (_AttackerBlip) _AttackerBlip.Delete();
            if (_VictimBlip) _VictimBlip.Delete();

            base.End();
        }
    }
}
