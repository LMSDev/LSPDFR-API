using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting;
using Rage;
using Stealth.Examples.Callouts.Extensions;
using Stealth.Examples.Callouts.Models.Peds;

namespace Stealth.Examples.Callouts.Models.Callouts
{

    public abstract class CalloutBase : LSPD_First_Response.Mod.Callouts.Callout, ICalloutBase
    {
        public CalloutBase()
        {
            State = Common.CalloutState.Created;
            CalloutMessage = "";
            ResponseType = Common.CallResponseType.Code_2;
            Peds = new List<PedBase>();
        }
        
        public CalloutBase(string pCalloutMessage, Common.CallResponseType pResponseType = Common.CallResponseType.Code_2)
        {
            State = Common.CalloutState.Created;
            CalloutMessage = pCalloutMessage;
            ResponseType = pResponseType;
            Peds = new List<PedBase>();
        }

        protected Vector3 GetRandomSpawnPoint(int pMin, int pMax)
        {
            return World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(Common.gRandom.Next(pMin, pMax)));
        }

        public override bool OnBeforeCalloutDisplayed()
        {
            //Base spawn point
            SpawnPoint = GetRandomSpawnPoint(150, 401);

            CalloutPosition = SpawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Radio.DispatchCallout(this.ScriptInfo.Name, SpawnPoint, CrimeEnums, ResponseType);
            State = Common.CalloutState.Dispatched;
            base.OnCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            //Radio.AcknowledgeCallout(this.ScriptInfo.Name, ResponseType);

            State = Common.CalloutState.UnitResponding;
            CreateBlip();

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            State = Common.CalloutState.Cancelled;

            foreach (PedBase p in Peds)
            {
                if (p != null)
                {
                    if (p.Exists())
                    {
                        p.Delete();
                    }
                }
            }

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (Game.LocalPlayer.Character.IsDead)
            {
                OfficerDown();
                End();
            }

            if (State == Common.CalloutState.UnitResponding)
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) < 30f)
                {
                    //Radio.UnitIsOnScene();
                    State = Common.CalloutState.AtScene;
                    OnArrivalAtScene();
                }
            }
            else if (State == Common.CalloutState.AtScene)
            {
                //If Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) > 1000.0F Then
                //    'If player is more than 1 KM from the scene
                //    'If LSPD_First_Response.Mod.API.Functions
                //End If

                if (Game.IsKeyDown(System.Windows.Forms.Keys.T))
                {
                    if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.ControlKey))
                    {
                        //AskPedToFollowOfficer();
                    }
                }
            }
        }

        public virtual void OfficerDown()
        {
            //code here
        }

        public virtual void OnArrivalAtScene()
        {
            DeleteBlip();
        }

        public override void End()
        {
            base.End();

            DeleteBlip();

            foreach (PedBase p in Peds)
            {
                if (p != null)
                {
                    if (p.Exists() == true)
                    {
                        p.DeleteBlip();
                        p.Dismiss();
                    }
                }
            }

            Peds.Clear();

            State = Common.CalloutState.Completed;
        }

        public void CreateBlip()
        {
            CallBlip = new Blip(CalloutPosition);
            CallBlip.Color = System.Drawing.Color.LimeGreen;
            CallBlip.EnableRoute(System.Drawing.Color.LimeGreen);
        }

        public void DeleteBlip()
        {
            if (CallBlip != null)
            {
                if (CallBlip.Exists())
                {
                    CallBlip.DisableRoute();
                    CallBlip.Delete();
                }
            }
        }

        public PedBase GetPed(string pName)
		{
			return (from x in Peds where x.Name == pName select x).FirstOrDefault();
		}

        public Vector3 GetRandomSpawnPoint(float pMin, float pMax)
        {
            return Vector3.Zero;
        }

        public void DeleteEntities()
        {
            throw new NotImplementedException();
        }

        public Common.CallResponseType ResponseType { get; set; }
        public Vector3 SpawnPoint { get; set; }
        public new Common.CalloutState State { get; set; }
        public Blip CallBlip { get; set; }
        public List<PedBase> Peds { get; set; }
    }

}