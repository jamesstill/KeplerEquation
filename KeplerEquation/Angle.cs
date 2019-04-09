using System;

namespace KeplerEquation
{
    public struct Angle
    {
        public double Radians { get; }
        public double Degrees { get; }

        /// <summary>
        /// Constructs an angle based on decimal, e.g. 28°.5793
        /// </summary>
        /// <param name="degrees"></param>
        public Angle(double degrees)
        {
            Degrees = degrees;
            Radians = (Math.PI / 180) * degrees;
        }

        /// <summary>
        /// Constructs an angle based on R.A. 13h 07m 31s
        /// </summary>
        /// <param name="hours">23</param>
        /// <param name="minutes">26</param>
        /// <param name="seconds">44.001</param>
        public Angle(double hours, double minutes, double seconds)
        {
            Degrees = hours + (minutes / 60) + (seconds / 3600);
            Radians = (Math.PI / 180) * Degrees;
        }
    }
}
