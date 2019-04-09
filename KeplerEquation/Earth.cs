using System;
using System.Collections.Generic;

namespace KeplerEquation
{
    public struct Earth
    {
        private Moment Moment { get; }

        public Earth(Moment moment)
        {
            Moment = moment;
        }

        /// <summary>
        /// Orbital element mean longitude L coefficients from Meeus Table 31.A
        /// </summary>
        public Dictionary<string, double> L => new Dictionary<string, double>
        {
            { "a0", 100.466457 },
            { "a1", 36000.7698278 },
            { "a2", 0.00030322 },
            { "a3", 0.000000020 }
        };

        /// <summary>
        /// Orbital element perihilion longitude π (ϖ) coefficients from Meeus Table 31.A 
        /// </summary>
        public Dictionary<string, double> P => new Dictionary<string, double>
        {
            { "a0", 102.937348 },
            { "a1", 1.7195366 },
            { "a2", 0.00045688 },
            { "a3", -0.000000018 }
        };

        /// <summary>
        /// Mean anomaly of the Earth from Meeus p.210 where M = L - π or 
        /// Colwell p. 3 has M = 2πt / T. Note that Colwell's use of π means 
        /// literally 3.1415... and NOT the longitude of the perihilion as 
        /// Meeus uses it.
        /// </summary>
        public double MeanAnomaly
        {
            get
            {
                var T = Moment.TimeT;

                var L0 = L["a0"] +
                    (L["a1"] * T) +
                    (L["a2"] * Math.Pow(T, 2)) +
                    (L["a3"] * Math.Pow(T, 3));

                var P0 = P["a0"] +
                    (P["a1"] * T) +
                    (P["a2"] * Math.Pow(T, 2)) +
                    (P["a3"] * Math.Pow(T, 3));

                return L0 - P0;
            }
        }

        /// <summary>
        /// Orbital element eccentricity e coefficients from Meeus Table 31.A 
        /// </summary>
        public Dictionary<string, double> e => new Dictionary<string, double>
        {
            { "a0", 0.01670863 },
            { "a1", -0.000042037 },
            { "a2", -0.0000001267 },
            { "a3", -0.00000000014 }
        };

        /// <summary>
        /// Eccentricity of the Earth's orbit at a given moment in time. 
        /// </summary>
        /// <param name="m">Moment</param>
        /// <returns></returns>
        public double Eccentricity
        {
            get
            {
                var T = Moment.TimeT;
                return e["a0"] + 
                    (e["a1"] * T) + 
                    (e["a2"] * (T * T)) +
                    (e["a3"] * (T * T * T));
            }
        }

        /// <summary>
        /// Using orbital elements from Meeus Table 31.A. This one's a
        /// freebie since Earth's major axis AB is by definition 1 AU.
        /// </summary>
        public double SemiMajorAxisA
        {
            get
            {
                return 1.000001018;
            }
        }

        /// <summary>
        /// The standard properties of an ellipse such that if you know
        /// a and e you can easily calculate b. See the wiki entry:
        /// https://en.wikipedia.org/wiki/Semi-major_and_semi-minor_axes
        /// </summary>
        public double SemiMinorAxisB
        {
            get
            {
                var a = SemiMajorAxisA;
                var e = Eccentricity;
                return a * Math.Sqrt(1 - (e * e));
            }
        }
    }
}
