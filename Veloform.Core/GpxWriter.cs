using System.Xml.Linq;

namespace Veloform.Core;

public static class GpxWriter
{
    private static readonly XNamespace NS = "http://www.topografix.com/GPX/1/1";
    public static string Write(string name, IReadOnlyList<Coordinate> points)
    {

       XElement trackName = new XElement(NS + "name", name);

       XElement trkpt = new XElement(NS + "trkpt",
        new XAttribute("lat", points[0].Lat.ToString("F6")),
        new XAttribute("lon", points[0].Lon.ToString("F6"))
       );
       XElement trkseg = new XElement(NS + "trkseg", trkpt);

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