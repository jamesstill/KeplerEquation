using System;
using System.Diagnostics;

namespace KeplerEquation
{
    /// <summary>
    /// Sample code for Kepler's Equation written by James Still. For 
    /// the full writeup see https://squarewidget.com/keplers-equation
    /// </summary>
    class Program
    {
        /// <summary>
        /// Kepler's Equation: E = M + e sin E
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Moment moment = new Moment(2019, 04, 07, 21, 0, 0);
            Moment moment = new Moment(DateTime.UtcNow);
            Earth earth = new Earth(moment);

            Console.WriteLine("Solving Kepler's Equation for moment {0}", moment.ToString());
            Console.WriteLine("Reference epoch: J2000.0");
            Console.WriteLine("Julian Day Number: " + moment.JulianDay);
            Console.WriteLine("Time T: " + moment.TimeT);
            
            Console.WriteLine();

            double E;
            var e = earth.Eccentricity;
            var a = earth.SemiMajorAxisA;
            var b = earth.SemiMinorAxisB;
            var M = earth.MeanAnomaly;

            Console.WriteLine("Earth eccentricity e: " + e);
            Console.WriteLine("Earth semi-major axis a: " + earth.SemiMajorAxisA);
            Console.WriteLine("Earth semi-minor axis b: " + earth.SemiMinorAxisB);
            Console.WriteLine("Earth mean anomaly M: " + M.ToCoterminal());
            Console.WriteLine();

            E = FirstMethod(e, M);
            Console.WriteLine("E for first method: " + E.ToCoterminal());
            Console.WriteLine();

            E = SecondMethod(e, M);
            Console.WriteLine("E for second method: " + E.ToCoterminal());
            Console.WriteLine();

            var v = GetTrueAnomaly(e, M);
            Console.WriteLine("True anomaly v from M: " + v.ToCoterminal());
            Console.WriteLine();

            v = GetTrueAnomaly2(e, E);
            Console.WriteLine("True Anomaly v from E: " + v.ToCoterminal());
            Console.WriteLine();

            var r = GetRadiusVector(e, v);
            Console.WriteLine("Radius vector r from e and v: " + r);
            Console.WriteLine();

            var coordinates = GetCoordinates(a, b, e, E);
            Console.WriteLine("Earth coordinates x: " + coordinates.X + " and y: " + coordinates.Y);

            Console.ReadLine();
        }

/// <summary>
/// Meeus First Method (p. 196)
/// </summary>
/// <returns></returns>
private static double FirstMethod(double e, double M)
{
    double e0 = e.ToDegrees();
    double E = M;

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    int n = 120;
    for(int i = 1; i < n; i++)
    {
        E = M + e0 * Math.Sin(E.ToRadians());
    }

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine("After " + n + " iterations " + "(" + ts.TotalMilliseconds + "ms)");
    return E;
}

/// <summary>
/// Meeus Second Method (p. 199)
/// </summary>
/// <returns></returns>
private static double SecondMethod(double e, double M)
{
    double e0 = e.ToDegrees();
    double E0 = M;
    double E1 = 0;

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    int n = 5;
    for(int i = 1; i < n; i++)
    {
        E1 = E0 + (M + e0 * Math.Sin(E0.ToRadians()) - E0) / 
                  (1 - e * Math.Cos(E0.ToRadians()));
        E0 = E1;
    }

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine("After " + n + " iterations:  " + "(" + ts.TotalMilliseconds + "ms)");
    return E1;
}

        /// <summary>
        /// From the mean anomaly using a Fourier expansion from Wiki
        /// https://en.wikipedia.org/wiki/True_anomaly
        /// </summary>
        /// <param name="e"></param>
        /// <param name="M"></param>
        /// <returns></returns>
        private static double GetTrueAnomaly(double e, double M)
        {
            return M + (2 * e - 0.25 * Math.Pow(e, 3)) * 
                Math.Sin(M.ToRadians()) + 1.25 * 
                Math.Pow(e, 2) * 
                Math.Sin(2 * M.ToRadians()) + 1.08333 * 
                Math.Pow(e, 3) * 
                Math.Sin(3 * M.ToRadians());
        }

/// <summary>
/// True anomaly expressed as a series in terms of e and E
/// via Smart pp. 118-119
/// </summary>
/// <param name="e"></param>
/// <param name="E"></param>
/// <returns></returns>
private static double GetTrueAnomaly2(double e, double E)
{
    return E + (e + 0.25 * Math.Pow(e, 3)) * 
        Math.Sin(E.ToRadians()) + (0.25 * Math.Pow(e, 2) * 
        Math.Sin(2 * E.ToRadians())) + (0.083333 * Math.Pow(e, 3) * 
        Math.Sin(3 * E.ToRadians()));
}

 /// <summary>
 /// Radius vector from Meeus (25.5) on p. 164 and also see section
 /// on Wiki entry for "True Anomaly" entitled "Radius from true anomaly"
 /// </summary>
 /// <param name="e">eccentricity</param>
 /// <param name="v">true anomaly</param>
 /// <returns></returns>
 private static double GetRadiusVector(double e, double v)
 {
     return 1.000001018 * (1 - Math.Pow(e, 2)) / 1 + e * Math.Cos(v.ToRadians());
 }

        private static Coordinates GetCoordinates(double a, double b, double e, double E)
        {
            var x = a * (Math.Cos(E.ToRadians() - e.ToRadians()));
            var y = b * Math.Sin(E.ToRadians());
            return new Coordinates(x, y);
        }
    }
}
