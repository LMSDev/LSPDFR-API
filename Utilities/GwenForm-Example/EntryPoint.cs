using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Rage;
using Rage.Attributes;

/* GwenForm-Example by LtFlash
 * Changes:
 * 2015-08-20 - Created
 * /

/* Suggested sequence of developing a project with GwenForm:
 * 1. Create an EntryPoint of your plugin.
 * 2. Create a Windows Form, fill it with controls.
 *    Don't forget to change the font unit to 'Pixel'!
 * 3. Add a derived class from GwenForm - in this example 'ExemplaryForm'.
 * 5. Remember to define variables for all of your controls.
 */

[assembly: Plugin("GwenForm-Example", Author="LtFlash")]
namespace GwenForm_Example
{
    public static class EntryPoint
    {
        //This variable will be storing a name entered in our text box
        internal static string sName;

        public static void Main()
        {
            sName = string.Empty;

            while(true)
            {
                if(Game.IsKeyDown(System.Windows.Forms.Keys.F9))
                {
                    Rage.Forms.GwenForm fExample = new UI.ExemplaryForm();
                    fExample.Show();
                }

                GameFiber.Yield();
            }
        }
    }
}