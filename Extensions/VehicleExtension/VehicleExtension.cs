namespace alexguirre.Common.Extensions
{
    using Rage;
    using Rage.Native;
    using System.Drawing;
    using System;

    /// <summary>
    /// Vehicle extensions
    /// </summary>
    public static class VehicleExtension
    {
        /// <summary>
        /// Toggles the neon light in a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="neonLight">Neon index</param>
        /// <param name="toggle">Toggle the neon</param>
        public static void ToggleNeonLight(this Vehicle vehicle, ENeonLights neonLight, bool toggle)
        {
            ulong SetVehicleNeonLightEnabledHash = 0x2aa720e4287bf269;
            
            NativeFunction.CallByHash<uint>(SetVehicleNeonLightEnabledHash, vehicle, (int)neonLight, toggle);
        }


        /// <summary>
        /// Sets the neon light color
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="color">Color to set</param>
        public static void SetNeonLightsColor(this Vehicle vehicle, Color color)
        {
            ulong SetVehicleNeonLightsColoursHash = 0x8e0a582209a62695;

            NativeFunction.CallByHash<uint>(SetVehicleNeonLightsColoursHash, vehicle, (int)color.R, (int)color.G, (int)color.B);
        }


        /// <summary>
        /// Returns true if the neon light is enabled
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="neonLight">Neon index</param>
        /// <returns>true if the neon light is enabled</returns>
        public static bool IsNeonLightEnable(this Vehicle vehicle, ENeonLights neonLight)
        {
            ulong IsVehicleNeonLightEnabledHash = 0x8c4b92553e4766a5;
            if (NativeFunction.CallByHash<bool>(IsVehicleNeonLightEnabledHash, vehicle, (int)neonLight)) return true;
            else if (!NativeFunction.CallByHash<bool>(IsVehicleNeonLightEnabledHash, vehicle, (int)neonLight)) return false;
            else return false;
        }


        /// <summary>
        /// Returns the neon light color
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>the neon light color</returns>
        public static Color GetNeonLightsColor(this Vehicle vehicle)
        {
            return UnsafeGetNeonLightsColor(vehicle);
        }
        private static unsafe Color UnsafeGetNeonLightsColor(Vehicle vehicle)
        {
            Color color;
            int red;
            int green;
            int blue;
            ulong GetVehicleNeonLightsColourHash = 0x7619eee8c886757f;
            NativeFunction.CallByHash<uint>(GetVehicleNeonLightsColourHash, vehicle, &red, &green, &blue);

            return color = Color.FromArgb(red, green, blue);
        }
       
        // Credit to Stealth22 for this function
        /// <summary>
        /// Gets the primary and secondary colors of this instance of Rage.Vehicle
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static VehicleColor GetColors(this Vehicle v)
        {
            return UnsafeGetVehicleColors(v);
        }

        private static unsafe VehicleColor UnsafeGetVehicleColors(Vehicle vehicle)
        {
            int colorPrimaryInt;
            int colorSecondaryInt;

            ulong GetVehicleColorsHash = 0xa19435f193e081ac;
            NativeFunction.CallByHash<uint>(GetVehicleColorsHash, vehicle, &colorPrimaryInt, &colorSecondaryInt);

            VehicleColor colors = new VehicleColor();

            colors.PrimaryColor = (EPaint)colorPrimaryInt;
            colors.SecondaryColor = (EPaint)colorSecondaryInt;

            return colors;
        }


        /// <summary>
        /// Sets the color to this Rage.Vehicle instance
        /// </summary>
        /// <param name="v"></param>
        /// <param name="primaryColor">The primary color</param>
        /// <param name="secondaryColor">The secondary color</param>
        public static void SetColors(this Vehicle v, EPaint primaryColor, EPaint secondaryColor)
        {
            NativeFunction.CallByName<uint>("SET_VEHICLE_COLOURS", v, (int)primaryColor, (int)secondaryColor);
        }
        /// <summary>
        /// Sets the color to this Rage.Vehicle instance
        /// </summary>
        /// <param name="v"></param>
        /// <param name="color">The color</param>
        public static void SetColors(this Vehicle v, VehicleColor color)
        {
            NativeFunction.CallByName<uint>("SET_VEHICLE_COLOURS", v, (int)color.PrimaryColor, (int)color.SecondaryColor);
        }
    }
    
    public enum ENeonLights
    {
        Front = 2,
        Back = 3,
        Left = 0,
        Right = 1,
    }
    
    public enum EPaint
    {
        /* CLASSIC|METALLIC */
        Black = 0,
        Carbon_Black = 147,
        Graphite = 1,
        Anhracite_Black = 11,
        Black_Steel = 2,
        Dark_Steel = 3,
        Silver = 4,
        Bluish_Silver = 5,
        Rolled_Steel = 6,
        Shadow_Silver = 7,
        Stone_Silver = 8,
        Midnight_Silver = 9,
        Cast_Iron_Silver = 10,
        Red = 27,
        Torino_Red = 28,
        Formula_Red = 29,
        Lava_Red = 150,
        Blaze_Red = 30,
        Grace_Red = 31,
        Garnet_Red = 32,
        Sunset_Red = 33,
        Cabernet_Red = 34,
        Wine_Red = 143,
        Candy_Red = 35,
        Hot_Pink = 135,
        Pfister_Pink = 137,
        Salmon_Pink = 136,
        Sunrise_Orange = 36,
        Orange = 38,
        Bright_Orange = 138,
        Gold = 37,
        Bronze = 90,
        Yellow = 88,
        Race_Yellow = 89,
        Dew_Yellow = 91,
        Green = 139,
        Dark_Green = 49,
        Racing_Green = 50,
        Sea_Green = 51,
        Olive_Green = 52,
        Bright_Green = 53,
        Gasoline_Green = 54,
        Lime_Green = 92,
        Hunter_Green = 144,
        Securiror_Green = 125,
        Midnight_Blue = 141,
        Galaxy_Blue = 61,
        Dark_Blue = 62,
        Saxon_Blue = 63,
        Blue = 64,
        Bright_Blue = 140,
        Mariner_Blue = 65,
        Harbor_Blue = 66,
        Diamond_Blue = 67,
        Surf_Blue = 68,
        Nautical_Blue = 69,
        Racing_Blue = 73,
        Ultra_Blue = 70,
        Light_Blue = 74,
        Police_Car_Blue = 127,
        Epsilon_Blue = 157,
        Chocolate_Brown = 96,
        Bison_Brown = 101,
        Creek_Brown = 95,
        Feltzer_Brown = 94,
        Maple_Brown = 97,
        Beechwood_Brown = 103,
        Sienna_Brown = 104,
        Saddle_Brown = 98,
        Moss_Brown = 100,
        Woodbeech_Brown = 102,
        Straw_Brown = 99,
        Sandy_Brown = 105,
        Bleached_Brown = 106,
        Schafter_Purple = 71,
        Spinnaker_Purple = 72,
        Midnight_Purple = 142,
        Metallic_Midnight_Purple = 146,
        Bright_Purple = 145,
        Cream = 107,
        Ice_White = 111,
        Frost_White = 112,
        Pure_White = 134,
        Default_Alloy = 156,
        Champagne = 93,


        /* MATTE */
        Matte_Black = 12,
        Matte_Gray = 13,
        Matte_Light_Gray = 14,
        Matte_Ice_White = 131,
        Matte_Blue = 83,
        Matte_Dark_Blue = 82,
        Matte_Midnight_Blue = 84,
        Matte_Midnight_Purple = 149,
        Matte_Schafter_Purple = 148,
        Matte_Red = 39,
        Matte_Dark_Red = 40,
        Matte_Orange = 41,
        Matte_Yellow = 42,
        Matte_Lime_Green = 55,
        Matte_Green = 128,
        Matte_Forest_Green = 151,
        Matte_Foliage_Green = 155,
        Matte_Brown = 129,
        Matte_Olive_Darb = 152,
        Matte_Dark_Earth = 153,
        Matte_Desert_Tan = 154,


        /* METALS */
        Brushed_Steel = 117,
        Brushed_Black_Steel = 118,
        Brushed_Aluminum = 119,
        Pure_Gold = 158,
        Brushed_Gold = 159,

        
        /* CHROME */
        Chrome = 120,
    }
}
