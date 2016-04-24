using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Windows.Forms;

namespace LSPDFR_API_Guide
{
    public class Main : Plugin
    {
        //For further information and explanation please check the PDF file.
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin LSPDFR_API_Guide " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by Albo1125 has been initialised.");
            Game.LogTrivial("Go on duty to fully load LSPDFR_API_Guide.");
        }
        public override void Finally()
        {
            Game.LogTrivial("LSPDFR_API_Guide has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts(); 
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.StolenVehicle));
            
        }
    }
}
