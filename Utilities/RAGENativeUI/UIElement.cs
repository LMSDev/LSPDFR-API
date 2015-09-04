//Added by LtFlash
﻿//Edited by alexguirre: File renamed to UIElement, added UIElement interface, UIText inherits from UIElement interface, added UIRectangle and UIContainer
using System;
using System.Drawing;
using Rage;
using Rage.Native;

namespace RAGENativeUI
{
    public interface UIElement
    {
        void Draw();
        void Draw(Size offset);

        bool Enabled { get; set; }
        Point Position { get; set; }
        Color Color { get; set; }
    }

    public class UIText : UIElement
    {
        public float Scale { get; set; }
        public string Caption { get; set; }
        public bool Centered { get; set; }
        public Font Font { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Point Position { get; set; }
        public virtual Color Color { get; set; }

        public UIText(String caption, Point position, float scale)
	{
		this.Enabled = true;
		this.Caption = caption;
		this.Position = position;
		this.Scale = scale;
		this.Color = Color.WhiteSmoke;
		this.Font = GTA::Font::ChaletLondon;
		this.Centered = false;
	}
        public UIText(String caption, Point position, float scale, Color color)
	{
                Enabled = true;
		Caption = caption;
		Position = position;
		Scale = scale;
		Color = color;
		Font = GTA::Font::ChaletLondon;
		Centered = false;
        }
        public UIText(String caption, Point position, float scale, Color color, Font font, bool centered)
	{
                Enabled = true;
		Caption = caption;
		Position = position;
		Scale = scale;
		Color = color;
		Font = font;
		Centered = centered;
        }
        public virtual void Draw()
        {
            Draw(new Size());
        }
        public virtual void Draw(Size offset)
        {
            if (!this.Enabled) return;

            float x = (this.Position.X + offset.Width) / 1280;
            float y = (this.Position.Y + offset.Height) / 720;

            NativeFunction.CallByName<uint>("SET_TEXT_FONT", (int)this.Font);
            NativeFunction.CallByName<uint>("SET_TEXT_SCALE", this.Scale, this.Scale);
            NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)this.Color.R, (int)this.Color.G, (int)this.Color.B, (int)this.Color.A);
            NativeFunction.CallByName<uint>("SET_TEXT_CENTRE", this.Centered);
            NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING"); // SetTextEntry native
            NativeFunction.CallByHash<uint>(0x6c188be134e074aa, substr); // AddTextComponentString native
            NativeFunction.CallByHash<uint>(0xcd015e5bb0d96a57, x, y); // DrawText native
        }
    }
    
    public class UIRectangle : UIElement
    {
        public Size Size { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Point Position { get; set; }
        public virtual Color Color { get; set; }
     
        public UIRectangle()
        {
            this.Enabled = true;
            this.Position = new Point();
            this.Size = new Size(1280, 720);
            this.Color = Color.Transparent;
        }
        public UIRectangle(Point position, Size size)
        {
            this.Enabled = true;
            this.Position = position;
            this.Size = size;
            this.Color = Color.Transparent;
        }
        public UIRectangle(Point position, Size size, Color color)
        {
            this.Enabled = true;
            this.Position = position;
            this.Size = size;
            this.Color = color;
        }

        public virtual void Draw()
        {
            Draw(new Size());
        }
        public virtual void Draw(Size offset)
        {
            if (!this.Enabled) return;

            float w = Size.Width / 1280;
            float h = Size.Height / 720;
            float x = ((Position.X + offset.Width) / 1280) + w * 0.5f;
            float y = ((Position.Y + offset.Height) / 720) + h * 0.5f;

            NativeFunction.CallByName<uint>("DRAW_RECT", x, y, w, h, (int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A);
        }
    }
    
    public class UIContainer : UIRectangle
    {
        private List<UIElement> _mItems = new List<UIElement>();
        public List<UIElement> Items { get { return this._mItems; } set { this._mItems = value; } }

        public UIContainer() : base()
        {

        }
        public UIContainer(Point pos, Size size) : base(pos, size)
        {
        }
        public UIContainer(Point pos, Size size, Color color) : base(pos, size, color)
        { 
        }
       
        public override void Draw() 
        {
            Draw(new Size());
        }

        public override void Draw(Size offset)
        {
            if (!this.Enabled) return;


            foreach (UIElement item in this.Items)
            {
                item.Draw(offset);
            }
        }
    }

    public enum Font
    {
        ChaletLondon = 0,
        HouseScript = 1,
        Monospace = 2,
        ChaletComprimeCologne = 4,
        Pricedown = 7
    }

}
