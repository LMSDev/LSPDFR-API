using Rage;
using Stealth.Examples.Callouts.Models.Peds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Examples.Callouts.Models.Callouts
{
    interface ICalloutBase
    {
        Vector3 GetRandomSpawnPoint(float pMin, float pMax);
        void OnArrivalAtScene();
        void CreateBlip();
        void DeleteBlip();
        PedBase GetPed(string pName);
        void OfficerDown();
        void DeleteEntities();

        List<PedBase> Peds { get; set; }
    }
}