using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using System.Windows.Forms;
[assembly: Rage.Attributes.Plugin("IniFileDemo", Description = "Shows how to set up and use a .ini file.", Author = "Albo1125")]

//To try this out, compile this code and place the .dll and .ini file in your /plugins folder.
namespace IniFileDemo
{
    public class EntryPoint
    {
        /// <summary>
        /// In this method, we load up the .ini file so other methods can use it.
        /// </summary>
        /// <returns></returns>
        public static InitializationFile initialiseFile()
        {
            //InitializationFile is a Rage class.
            InitializationFile ini = new InitializationFile("Plugins/IniFileDemo.ini");
            ini.Create();
            return ini;
        }

        /// <summary>
        /// In this method, we load up the ini file with the method above and we read one of the values: in this case, a keybinding.
        /// </summary>
        /// <returns></returns>
        public static String getMyKeyBinding()
        {
            InitializationFile ini = initialiseFile();

            //ReadString takes 3 parameters: the first is the category, the second is the name of the entry and the third is the default value should the user leave the field blank.
            //Take a look at the example .ini file to understand this better.
            string keyBinding = ini.ReadString("Keybindings", "myKeyBinding", "B");
            return keyBinding;
        }

        /// <summary>
        /// In this method, we read the player's name from the .ini file. If it is too long, we let the player know in a somewhat subtle way.
        /// </summary>
        /// <returns></returns>
        public static String getPlayerName()
        {
            //We use the first method we created
            InitializationFile ini = initialiseFile();

            //ReadString takes 3 parameters: the first is the category, the second is the name of the entry and the third is the default value should the user leave the field blank.
            //Take a look at the example .ini file to understand this better.
            string playerName = ini.ReadString("Misc", "Playername", "NoNameSet");

            //If the name has more than 12 characters
            if (playerName.Length > 12)
            {
                playerName = "NameTooLong";
            }
            return playerName;
        }

        /// <summary>
        /// In the main method, we attempt to read all the information from the .ini file. To convert a string to a System.Windows.Forms.Keys, we use a KeyConverter.
        /// Do not forget to add a reference to System.Windows.Forms, which can be done via project> add reference> assemblies> framework.
        /// I also added using System.Windows.Keys; at the beginning of the project so we don't have to type that every time we use one of its methods.
        /// </summary>
        public static void Main()
        {
            //A keysconverter is used to convert a string to a key.
            KeysConverter kc = new KeysConverter();

            //We create two variables: one is a System.Windows.Keys, the other is a string.
            Keys myKeyBinding;
            string playerName;
            

            //Use a try/catch, because reading values from files is risky: we can never be sure what we're going to get and we don't want our plugin to crash.
            try
            {
                //We assign myKeyBinding the value of the string read by the method getMyKeyBinding(). We then use the kc.ConvertFromString method to convert this to a key.
                //If the string does not represent a valid key (see .ini file for a link) an exception is thrown. That's why we need a try/catch.
                myKeyBinding = (Keys)kc.ConvertFromString(getMyKeyBinding());

                //For the playerName, we don't need to convert the value to a Key, so we can simply assign playerName to the return value of getPlayerName(). 
                //Remember we've already made sure the name can't be longer than 12 characters.
                playerName = getPlayerName();
            }
            //If there was an error reading the values, we set them to their defaults. We also let the user know via a notification.
            catch
            {
                myKeyBinding = Keys.B;
                playerName = "DefaultName";
                Game.DisplayNotification("There was an error reading the .ini file. Setting defaults...");
            }

            //Now you can do whatever you like with them! To finish off the example, we create a notification with our name when we press our keybinding.

            //We create a new GameFiber to listen for our key input. 
            GameFiber.StartNew(delegate
            {
                //This loop runs until it's broken
                while (true)
                {
                    //If our key has been pressed
                    if (Game.IsKeyDown(myKeyBinding))
                    {
                        //Create a notification displaying our name.
                        Game.DisplayNotification("Your name is: " + playerName + ".");
                        //And break out of the loop.
                        break;
                    }

                    //Let other GameFibers do their job by sleeping this one for a bit.
                    GameFiber.Yield();
                }
            });
        }
    }
}
