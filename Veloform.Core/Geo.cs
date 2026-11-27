using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace Veloform.Core;

public static class Geo
{
    // Helper class for calcualtions

    public static class Constants
    {
        public const int R = 6371000; // [meters]
    }
    /// <summary>
    ///  Calculates the distance bettween two coordinates using the haversine formula
    /// </summary>
    /// <param name="cordA"></param>
    /// <param name="cordB"></param>
    /// <returns>Distance in meters.</returns>
    public static double Distance(Coordinate cordA, Coordinate cordB)
    {
        
        // a = sin²(Δφ/2) + cos(φ1) · cos(φ2) · sin²(Δλ/2)
        // d = 2R · atan2(√a, √(1−a))
        double phi1 = double.DegreesToRadians(cordA.Lat);
        double phi2 = double.DegreesToRadians(cordB.Lat);
        double deltaPhi = double.DegreesToRadians(cordB.Lat - cordA.Lat);
        double deltaLambda = double.DegreesToRadians(cordB.Lon - cordA.Lon);

        double a = Math.Pow(Math.Sin(deltaPhi / 2), 2) + Math.Cos(phi1) * 
                   Math.Cos(phi2) * Math.Pow(Math.Sin(deltaLambda / 2), 2);

        return 2 * Constants.R * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
    }


    /// <summary>
    /// Calculates the total length of a path.
    /// </summary>
    /// <param name="points">Points along the path, in order.</param>
    /// <returns>Length in meters. If fewer than two points, returns 0.</returns>
    public static double PathLength(IReadOnlyList<Coordinate> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        double len = 0;

        for(int i = 0; i < points.Count - 1; i++)
            len += Distance(points[i], points[i + 1]);
            
        return len;
        
    }

}