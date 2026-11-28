using System.Xml.Linq;
using System.Globalization;

namespace Veloform.Core;

public static class GpxWriter
{
    private static readonly XNamespace NS = "http://www.topografix.com/GPX/1/1";

    /// <summary>
    /// Builds a GPX document with a single track from a list of points.
    /// </summary>
    /// <param name="name">The name of the track. If null or whitespace, defaults to "My Track".</param>
    /// <param name="points">Points along the track in order</param>
    /// <returns>The GPX as a string.</returns>
    public static string Write(string name, IReadOnlyList<Coordinate> points)
    {
        name = string.IsNullOrWhiteSpace(name) ? "My Track" : name;
        ArgumentNullException.ThrowIfNull(points);

        XElement trackName = new XElement(NS + "name", name);
        XElement trkseg = new XElement(NS + "trkseg");

        // Add the points from the list to the trkseg element.
        foreach (Coordinate point in points)
        {
            XElement trkpt = new XElement(NS + "trkpt",
                new XAttribute("lat", point.Lat.ToString("F6", CultureInfo.InvariantCulture)),
                new XAttribute("lon", point.Lon.ToString("F6", CultureInfo.InvariantCulture))
                );
            trkseg.Add(trkpt);
        }

        XElement trk = new XElement(NS + "trk", trackName, trkseg);

        XElement gpx = new XElement(NS + "gpx",
         new XAttribute("version", "1.1"),
         new XAttribute("creator", "Veloform"),
         trk
        );

        XDocument doc = new XDocument(
         new XDeclaration("1.0", "UTF-8", null),
         gpx
        );

        return doc.Declaration + "\n" + doc.ToString();

    }


}