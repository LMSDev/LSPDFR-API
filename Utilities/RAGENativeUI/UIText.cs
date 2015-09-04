//Added by LtFlash

using System;
using System.Drawing;

namespace NativeUI
{
    public class UIText
    {
        public virtual bool Enabled { get; set; }
        public virtual Point Position { get; set; }
        public virtual Color Color { get; set; }
        public String Caption { get; set; }
        public Font Font { get; set; }
		public float Scale { get; set; }
        public bool Centered { get; set; }

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
        public void Draw()
        {
            Draw(Size());
        }
        public virtual void Draw(Size offset)
        {
            if (!this.Enabled)
            {
                return;
            }

            const float x = (static_cast<float>(this->Position.X) + offset.Width) / UI::WIDTH;
            const float y = (static_cast<float>(this->Position.Y) + offset.Height) / UI::HEIGHT;

            Native::Function::Call(Native::Hash::SET_TEXT_FONT, (int)this->Font);
            Native::Function::Call(Native::Hash::SET_TEXT_SCALE, this->Scale, this->Scale);
            Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, this->Color.R, this->Color.G, this->Color.B, this->Color.A);
            Native::Function::Call(Native::Hash::SET_TEXT_CENTRE, this->Centered ? 1 : 0);
            Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "STRING");
            Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, this->Caption);
            Native::Function::Call(Native::Hash::_DRAW_TEXT, x, y);
        }
    }
}