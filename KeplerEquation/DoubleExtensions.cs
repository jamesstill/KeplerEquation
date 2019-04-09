using System;

namespace KeplerEquation
{
    public static class DoubleExtensions
    {
        public static double ToRadians(this double d)
        {
            return d * (Math.PI / 180); // convert degrees to radians
        }

        public static double ToDegrees(this double d)
        {
            return d * (180 / Math.PI); // convert radians to degrees
        }

        /// <summary>
        /// For very large angles reduce to between 0 and 360 degrees
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ToCoterminal(this double d)
        {
            d = d % 360;
            if (d < 0)
            {
                d += 360;
            }

            return d;
        }

    }
}
