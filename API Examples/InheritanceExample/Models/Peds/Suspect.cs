using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace Stealth.Examples.Callouts.Models.Peds
{
    public class Suspect : PedBase
    {
        public Suspect(string pName, Rage.Vector3 position, bool confirmedSuspect = true) : base(pName, Common.PedType.Suspect, position)
        {
            if (confirmedSuspect == false)
            {
                this.Type = Common.PedType.Unknown;
            }
        }

        public Suspect(string pName, Rage.Model model, Rage.Vector3 position, float heading, bool confirmedSuspect = true) : base(pName, Common.PedType.Suspect, model, position, heading)
        {
            if (confirmedSuspect == false)
            {
                this.Type = Common.PedType.Unknown;
            }
        }

        protected internal Suspect(string pName, Rage.PoolHandle handle, bool confirmedSuspect = true) : base(pName, Common.PedType.Suspect, handle)
        {
            if (confirmedSuspect == false)
            {
                this.Type = Common.PedType.Unknown;
            }
        }

    }
}
