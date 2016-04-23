namespace alexguirre.Common.Extensions
{
    using Rage;
    using Rage.Native;
    using System.Drawing;
    using System;
    
    //Credit to Stealth22 for this struct
    /// <summary>
    /// Struct for vehicles primary and secondary colors
    /// </summary>
    public struct VehicleColor
    {
        /// <summary>
        /// The primary color paint index 
        /// </summary>
        public EPaint PrimaryColor { get; set; }

        /// <summary>
        /// The secondary color paint index 
        /// </summary>
        public EPaint SecondaryColor { get; set; }



        /// <summary>
        /// Gets the primary color name
        /// </summary>
        public string PrimaryColorName
        {
            get { return GetColorName(PrimaryColor); }
        }
        /// <summary>
        /// Gets the secondary color name
        /// </summary>
        public string SecondaryColorName
        {
            get { return GetColorName(SecondaryColor); }
        }



        /// <summary>
        /// Gets the color name
        /// </summary>
        /// <param name="paint">Color to get the name from</param>
        /// <returns></returns>
        public string GetColorName(EPaint paint)
        {
            String name = Enum.GetName(typeof(EPaint), paint);
            return name.Replace("_", " ");
        }
    }
}
