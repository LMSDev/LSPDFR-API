public static class IsKeyDownWithComputerCheck 
{
        public static bool IsKeyDownComputerCheck(Keys KeyPressed)
        {


            if (Rage.Native.NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") != 0)
            {
                
                return Game.IsKeyDown(KeyPressed);
            }
            else
            {
                return false;
            }



        }
        public static bool IsKeyDownRightNowComputerCheck(Keys KeyPressed)
        {


            if (Rage.Native.NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") != 0)
            {
                return Game.IsKeyDownRightNow(KeyPressed);
            }
            else
            {
                return false;
            }



        }
}      
