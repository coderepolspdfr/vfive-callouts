using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
using System.Drawing;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Runtime.CompilerServices;
using LSPD_First_Response.Engine;

namespace VFive_Callouts.Callouts
{
    [CalloutInfo("HighSpeedChase", CalloutProbability.Medium)]

    public class HighSpeedChase : Callout
    {
        private Ped Suspect;
        private Vehicle SuspectVehicle;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private Vector3 spawnpoint;
        private bool PursuitCreated;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetRandomPositionOnStreet();
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);
            CalloutMessage = "High Speed Pursuit";
            CalloutPosition = spawnpoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE_CRIME_RESISTING_ARREST_02 IN_OR_ON_POSITION", spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            SuspectVehicle = new Vehicle("ADDER", spawnpoint);
            SuspectVehicle.IsPersistent = true;

            Suspect = new Ped(SuspectVehicle.GetOffsetPositionFront(5f));
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            Suspect.WarpIntoVehicle(SuspectVehicle, -1);

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Red;
            SuspectBlip.IsRouteEnabled = true;

            SuspectVehicle.IsEngineOn = true;
            SuspectVehicle.IsStolen = true;
            SuspectVehicle.LicensePlate = "VFive";

            PursuitCreated = false;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!PursuitCreated && Game.LocalPlayer.Character.DistanceTo(SuspectVehicle) <= 120f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                PursuitCreated = true;
            }

            if (PursuitCreated && !Functions.IsPursuitStillRunning(Pursuit))
            {
                End();
            }

            base.Process();
        }

        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }

            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            if (SuspectVehicle.Exists())
            {
                SuspectVehicle.Dismiss();
            }

            Game.LogTrivial("VFive Callouts: High Speed Pursuit ended.");
        }
    }
}
