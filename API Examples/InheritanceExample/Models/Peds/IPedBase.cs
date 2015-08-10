using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RagePluginHook;
using Rage;

namespace Stealth.Examples.Callouts.Models.Peds
{
    interface IPedBase
    {
        Common.PedType Type { get; set; }
        String Name { get; set; }
        String DisplayName { get; set; }
        Blip Blip { get; set; }
        Vector3 OriginalSpawnPoint { get; set; }

        void CreateBlip(Color? pColor = null);
        void DeleteBlip();
    }
}
