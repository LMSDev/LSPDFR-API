namespace alexguirre.Common.Extensions
{
    using Rage;
    using Rage.Native;
    using alexguirre.Common.Enum;
    using alexguirre.Common.Extensions;
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
}
