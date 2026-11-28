using Veloform.Core;

public class GpxWriterTests
{
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void Write_OneCoordinate_ContainsTheCoordinate()
    {
        var points = new List<Coordinate>();
        points.Add(new Coordinate(35, 32));

        String gpx = GpxWriter.Write("", points);

        Assert.Contains("lat=\"35.000000\"", gpx);
        Assert.Contains("lon=\"32.000000\"", gpx);
    }
}