using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Examples.Callouts
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        private static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                //Code here
            }
        }

        public override void Finally()
        {
            throw new NotImplementedException();
        }
    }
}
