using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stealth.Examples.Callouts.Extensions;
using Stealth.Examples.Callouts.Models.Peds;

namespace Stealth.Examples.Callouts.Models.Callouts.Callout_Types
{
    [CalloutInfo("ExampleCallout", CalloutProbability.Medium)]
    class MyNewCallout : CalloutBase
    {
        private LHandle pursuit;
        private bool pursuitInitiated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            //Create our ped in the world
            Suspect myPed = new Suspect("Suspect1", "a_m_y_mexthug_01", SpawnPoint.Around(10), 0);

            //Create the vehicle for our ped
            Vehicle myVehicle = new Vehicle("DUKES2", SpawnPoint);

            //Now we have spawned them, check they actually exist and if not return false (preventing the callout from being accepted and aborting it)
            if (!myPed.Exists()) return false;
            if (!myVehicle.Exists()) return false;

            //Add the Ped to the callout's list of Peds
            Peds.Add(myPed);

            //If we made it this far both exist so let's warp the ped into the driver seat
            myPed.WarpIntoVehicle(myVehicle, -1);

            // Show the user where the pursuit is about to happen and block very close peds.
            this.ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 15f);
            this.AddMinimumDistanceCheck(5f, myPed.Position);

            // Set up our callout message and location
            this.CalloutMessage = "Example Callout Message";
            this.CalloutPosition = SpawnPoint;

            //Play the police scanner audio for this callout (available as of the 0.2a API)
            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT CRIME_RESIST_ARREST IN_OR_ON_POSITION", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnArrivalAtScene()
        {
            base.OnArrivalAtScene();

            Suspect mySuspect = (Suspect)GetPed("Suspect1");

            if (mySuspect != null && mySuspect.Exists())
            {
                pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, mySuspect);
            }
        }

        public override void Process()
        {
            base.Process();

            if (pursuitInitiated && !Functions.IsPursuitStillRunning(pursuit))
            {
                End();
            }
        }
    }
}
