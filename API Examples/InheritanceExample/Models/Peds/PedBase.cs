using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Examples.Callouts.Models.Peds
{
    public class PedBase : Ped, IPedBase
    {
        public Common.PedType Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Blip Blip { get; set; }
        public Vector3 OriginalSpawnPoint { get; set; }

        public PedBase(string pName, Common.PedType pType, Rage.Model model, Rage.Vector3 position, float heading) : base(model, position, heading)
        {
            Name = pName;
            Type = pType;
            Init();
        }

        protected internal PedBase(string pName, Common.PedType pType, Rage.PoolHandle handle) : base(handle)
        {
            Name = pName;
            Type = pType;
            Init();
        }

        public PedBase(string pName, Rage.Vector3 position) : this(pName, Common.PedType.Unknown, position)
        {
        }

        public PedBase(string pName, Rage.Model model, Rage.Vector3 position, float heading) : this(pName, Common.PedType.Unknown, model, position, heading)
        {
        }

        protected internal PedBase(string pName, Rage.PoolHandle handle) : this(pName, Common.PedType.Unknown, handle)
        {
        }

        public PedBase(string pName, Common.PedType pType, Rage.Vector3 position) : base(position)
        {
            Name = pName;
            Type = pType;
            Init();
        }

        protected internal void Init()
        {
            OriginalSpawnPoint = this.Position;
        }

        public override void Dismiss()
        {
            DeleteBlip();
            base.Dismiss();
        }

        public override void Delete()
        {
            DeleteBlip();
            base.Delete();
        }

        public void CreateBlip(Color? pColor = null)
        {
            if (this.Exists())
            {
                Color color = default(Color);

                if (pColor == null)
                {
                    switch (Type)
                    {
                        case Common.PedType.Suspect:
                            color = Color.Red;
                            break;
                        case Common.PedType.Unknown:
                            color = Color.Yellow;
                            break;
                        default:
                            color = Color.Lime;
                            break;
                    }
                }
                else
                {
                    color = (Color)pColor;
                }

                this.Blip = new Blip(this);
                this.Blip.Color = color;
            }
        }

        public void DeleteBlip()
        {
            try
            {
                if (this.Blip != null)
                {
                    if (this.Blip.Exists())
                    {
                        this.Blip.Delete();
                    }
                }
                else
                {
                    //Game.LogVerboseDebug("Tried to delete Ped blip, but it was null");
                }
            }
            catch (Exception ex)
            {
                Game.LogVerboseDebug("Error deleting Ped blip -- " + ex.Message);
            }
        }
    }
}
