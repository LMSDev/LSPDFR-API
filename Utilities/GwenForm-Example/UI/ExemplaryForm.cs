using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Rage;
using RagePluginHook;

namespace GwenForm_Example.UI
{
    //This class defines the actual form which will be displayed in-game.
    //It uses the Windows Form you created as a template to get sizes, positions of
    //controls etc. so you don't need to code it manually here.

    public class ExemplaryForm : GwenForm 
    {
        //Before you fill up this class with code you need to create a new "Windows Form...",
        //it is crucial to set the Font->Unit of the actual form to 'Pixel'
        //instead of default 'Point'. You can place controls at the form as you want,
        //your design will be used to display the form in-game.

        //All names come from the Form_Template, you got to copy them here
        private Gwen.Control.Label lbName;
        private Gwen.Control.TextBox tbName;
        private Gwen.Control.Button btnAccept;

        public ExemplaryForm() : base(typeof(Form_Template))
        {
        }

        public override void InitializeLayout()
        {
            //You can customize the initial position of your window
            this.Position =
                new System.Drawing.Point(1,
                    Game.Resolution.Height / 2 - this.Size.Height / 2);

            //In this loop we check if the window was closed, it let you to save
            //the data inside text boxes etc. to prevent it being removed when
            //player click at the red X. You can remove this check.
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    if (!this.Window.IsVisible)
                    {
                        GwenForm_Example.EntryPoint.sName = tbName.Text;

                        break;
                    }
                    GameFiber.Yield();
                }
            });

            tbName.Text = GwenForm_Example.EntryPoint.sName;

            btnAccept.Clicked += btnAccept_Clicked;

            base.InitializeLayout();
        }

        void btnAccept_Clicked(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs arguments)
        {
            //You can easily create a message boxes known from OS 
            Gwen.Control.MessageBox mbName =
                new Gwen.Control.MessageBox(this, "Your name is " + tbName.Text, "Message Box");

            btnAccept.Text = "Accepted!";
        }
    }
}