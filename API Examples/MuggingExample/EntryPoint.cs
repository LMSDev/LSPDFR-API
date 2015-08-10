using System;
using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using StraysCallouts.Callouts;

namespace StraysCallouts
{
    /// <summary>
    /// Inherit Plugin, since this is a plugin.
    /// </summary>
    public class EntryPoint : Plugin
    {
        /// <summary>
        /// This method is run when the plugin is first initialized.
        /// </summary>
        public override void Initialize()
        {
            //Subscribe to the OnOnDutyStateChanged event, so we don't register our callouts unless the player is on duty.
            Functions.OnOnDutyStateChanged += this.OnDutyStateChangedEvent;

            //Logging is a great tool, so we log to make sure the plugins loaded.
            Game.LogTrivial("StraysCallouts initialized");
        }

        /// <summary>
        /// Called when the OnOnDutyStateChanged event is raised.
        /// </summary>
        /// <param name="onDuty"></param>
        public void OnDutyStateChangedEvent(bool onDuty)
        {
            //If the player is going on duty, register the callout.
            if (onDuty)
            {
                Functions.RegisterCallout(typeof(Mugging));
            }
        }

        /// <summary>
        /// Called before the plugin is unloaded.
        /// </summary>
        public override void Finally()
        {
            
        }
    }
}
