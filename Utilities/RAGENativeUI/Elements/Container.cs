using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGENativeUI.Elements
{
    public class Container : Rectangle
    {
        private List<IElement> _mItems = new List<IElement>();
        public List<IElement> Items { get { return this._mItems; } set { this._mItems = value; } }

        public Container()
            : base()
        {

        }
        public Container(Point pos, Size size)
            : base(pos, size)
        {
        }
        public Container(Point pos, Size size, Color color)
            : base(pos, size, color)
        {
        }

        public override void Draw()
        {
            Draw(new Size());
        }

        public override void Draw(Size offset)
        {
            if (!this.Enabled) return;


            foreach (IElement item in this.Items)
            {
                item.Draw(offset);
            }
        }
    }
}
