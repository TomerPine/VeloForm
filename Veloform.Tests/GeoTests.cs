using Veloform.Core;

public class GeoTests
{
    [Fact]
    public void Distance_SamePoint_IsZero()
    {
        var p = new Coordinate(32.0, 35.0);
        Assert.Equal(0, Geo.Distance(p, p), precision: 3);
    }

    [Fact]
    public void Distance_OneDegreeLatitude_IsAbout111Km()
    {
        var a = new Coordinate(32.0, 35.0);
        var b = new Coordinate(33.0, 35.0);
        Assert.InRange(Geo.Distance(a, b), 110_000, 112_000);
    }



    [Fact]
    public void PathLength_EmptyList_IsZero()
    {
        var points = new List<Coordinate>();
        Assert.Equal(0, Geo.PathLength(points));
    }

    [Fact]
    public void PathLength_NullList_ArgumentNullExceptionThrown()
    {
        Assert.Throws<ArgumentNullException>(() => Geo.PathLength(null!));
    }

    [Fact]
    public void PathLength_ThreePoints_IsSumOfSegments()
    {
        var a = new Coordinate(32.0, 35.0);
        var b = new Coordinate(32.5, 35.0);
        var c = new Coordinate(33.0, 35.0);

        double expected = Geo.Distance(a, b) + Geo.Distance(b, c);

        Assert.Equal(expected, Geo.PathLength([a, b, c]), precision: 6);
    }
}