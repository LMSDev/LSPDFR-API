using System;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace StraysCallouts.Callouts
{
    //Name the callout, and set the probability.
    [CalloutInfo("Mugging", CalloutProbability.Medium)]
    //Inherit the Callout class, since we're making a callout.
    public class Mugging : LSPD_First_Response.Mod.Callouts.Callout
    {
        /// <summary>
        /// This callout waits until the player is on scene to start, which is the purpose of EMuggingState, so we know when to start running the callout's logic.
        /// </summary>
        public EMuggingState state;
        public LHandle pursuit;
        public Vector3 spawnPoint;
        public Blip ABlip;
        public Ped Aggressor;
        public Ped Victim;

        /// <summary>
        /// Called before the callout is displayed. Do all spawning here, so that if spawning isn't successful, the player won't notice, as the callout won't be shown.
        /// </summary>
        /// <returns></returns>
        public override bool OnBeforeCalloutDisplayed()
        {
            //Get a valid spawnpoint for the callout, and spawn the Aggressor there
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f));
            Aggressor = new Ped(spawnPoint);

            //Spawn the victim in front of the aggressor
            Victim = new Ped(Aggressor.GetOffsetPosition(new Vector3(0, 1.8f, 0)));

            //If for some reason, the spawning of either two peds failed, don't display the callout
            if(!Aggressor.Exists()) return false;
            if(!Victim.Exists()) return false;

            //If the peds are valid, display the area that the callout is in.
            this.ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 15f);
            this.AddMinimumDistanceCheck(5f, spawnPoint);

            //Give the aggressor his weapon
            Aggressor.GiveNewWeapon("WEAPON_PISTOL", 500, true);

            //Set the callout message(displayed in the notification), and the position(also shown in the notification)
            this.CalloutMessage = "Mugging";
            this.CalloutPosition = spawnPoint;

            //Play the scanner audio.
            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_POSSIBLE_MUGGING IN_OR_ON_POSITION", this.spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        /// <summary>
        /// Called when the player accepts the callout
        /// </summary>
        /// <returns></returns>
        public override bool OnCalloutAccepted()
        {
            //Set the player as en route to the scene
            state = EMuggingState.EnRoute;

            //Attach a blip to the Aggressor, so the player knows where to go, and can find the aggressor if he flees
            ABlip = Aggressor.AttachBlip();

            //Have the aggressor aim at the victim, and have the victim put their hands up. -1 makes the task permanent, or until we clear the task, which we do later.
            NativeFunction.CallByName<uint>("TASK_AIM_GUN_AT_ENTITY", Aggressor, Victim, -1, true);
            Victim.Tasks.PutHandsUp(-1, Aggressor);

            //Block permanent events, so the victim doesn't flee if something disturbs them(A vehicle tapping them, etc..), as this would completely disrupt the callout's logic.
            Victim.BlockPermanentEvents = true;

            //Display a message to let the user know that the callout was accepted.
            Game.DisplaySubtitle("Get to the ~r~scene~w~.", 6500);
            return base.OnCalloutAccepted();
        }

        /// <summary>
        /// Called if the player ignores the callout
        /// </summary>
        public override void OnCalloutNotAccepted()
        {
            base.OnCalloutNotAccepted();
            //Clean up what we spawned earlier, since the player didn't accept the callout.
            if (Aggressor.Exists()) Aggressor.Delete();
            if (Victim.Exists()) Victim.Delete();
            if (ABlip.Exists()) ABlip.Delete();
        }

        /// <summary>
        /// All callout logic should be done here.
        /// </summary>
        public override void Process()
        {
            base.Process();

            //If the player is driving to the scene, and their distance to the scene is less than 15, start the callout's logic.
            if (state == EMuggingState.EnRoute && Game.LocalPlayer.Character.Position.DistanceTo(spawnPoint) <= 15)
            {
                //Set the player as on scene
                state = EMuggingState.OnScene;

                //Start the callout's logic. You can paste the logic from StartMuggingScenario straight into here, but I don't, since I like it to look clean, and place any long methods towards the bottom of the class.
                StartMuggingScenario();
            }

            //If the state is DecisionMade(The aggressor already decided what random outcome to execute), and the pursuit isn't running anymore, end the callout.
            if (state == EMuggingState.DecisionMade && !Functions.IsPursuitStillRunning(pursuit))
            {
                this.End();
            }
        }

        /// <summary>
        /// Called when the callout ends
        /// </summary>
        public override void End()
        {
            //Dismiss the aggressor and victim, so they can be deleted by the game once the player leaves the scene.
            if (Aggressor.Exists()) Aggressor.Dismiss();
            if (Victim.Exists()) Victim.Dismiss();

            //Delete the blip attached to the aggressor
            if (ABlip.Exists()) ABlip.Delete();
            base.End();
        }

        /// <summary>
        /// The method that contains the callout's logic
        /// </summary>
        public void StartMuggingScenario()
        {
            //ALWAYS START A NEW GAME FIBER IF YOU'RE GOING TO USE GameFiber.Sleep, DON'T SLEEP THE MAIN FIBER.
            GameFiber.StartNew(delegate
            {
                //Create the pursuit
                this.pursuit = Functions.CreatePursuit();

                //Pick a random number, to choose a random outcome
                int r = new Random().Next(1, 4);

                //Set the state to decision made, since the outcome is chosen.
                state = EMuggingState.DecisionMade;

                //Execute one of the random outcomes
                if (r == 1)
                {
                    //The aggressor kills the victim before fleeing from the scene, and the victim flees the scene, trying to escape the aggressor.
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", Aggressor, Victim, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_REACT_AND_FLEE_PED", Victim, Aggressor);

                    //The aggressor shoots at the victim for 5 seconds, which either kills them, or severely injures them.
                    GameFiber.Sleep(5000);

                    NativeFunction.CallByName<uint>("TASK_REACT_AND_FLEE_PED", Victim, Aggressor);
                    //Now for another random outcome
                    if (new Random().Next(1, 3) == 2)
                    {
                        //The aggressor attacks the player.
                        NativeFunction.CallByName<uint>("TASK_COMBAT_PED", Aggressor, Game.LocalPlayer.Character, 0, 1);

                        //We wait 4.5 seconds before adding the ped to a pursuit, since as soon as we add the aggressor to a pursuit, LSPDFR takes over the AI, and they won't attack the player anymore. They'll flee instead.
                        GameFiber.Sleep(4500); 
                    }
                }
                else
                {
                    //The aggressor doesn't attack the victim, instead, both peds flee. We don't need to tell the aggressor to flee, as LSPDFR's pursuit system does that for us.
                    NativeFunction.CallByName<uint>("TASK_REACT_AND_FLEE_PED", Victim, Aggressor);
                }
                //Dismiss the aggressor from our plugin
                Aggressor.Dismiss();

                //Add the aggressor to a pursuit
                Functions.AddPedToPursuit(this.pursuit, Aggressor);

                //Dispatch a backup unit.
                Functions.RequestBackup(Game.LocalPlayer.Character.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            });
        }
    }

    /// <summary>
    /// Mugging states
    /// </summary>
    public enum EMuggingState
    {
        EnRoute,
        OnScene,
        DecisionMade
    }
}
