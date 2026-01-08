using Day9;

namespace Day9Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var line = new Line()
        {
            Start = (2, 3),
            End = (2, 7)
        };


        Assert.That(line.IsPointOnLine(2, 4), Is.True);
    }

    [Test]
    public void DoesLineCrossTest()
    {
        var tiles = new List<Tile>()
        {
            new Tile()
            {
                X = 2,
                Y = 3
            },
            new Tile()
            {
                X = 5,
                Y = 3
            },
            new Tile()
            {
                X = 2,
                Y = 5
            },
            new Tile()
            {
                X = 5,
                Y = 5
            },
        };

        var tileMap = new TileMap(tiles);

        var line = new Line()
        {
            Start = (2, 3),
            End = (2, 7)
        };

        var result = tileMap.DoesLineCrossBoundary(line);

        Assert.That(result, Is.True);
        
        
        
    }
    // (2,3) to (2,7) crosses (2,5) to (5,5)?

    [Test]
    public void LineCrossTest()
    {
        var lineA = new Line()
        {
            Start = (2, 3),
            End = (2, 7)
        };
        var lineB = new Line()
        {
            Start = (2,5),
            End = (5,5)
        };

        var result = TileMap.DoLinesCross(lineA, lineB);
        
        Assert.That(result,Is.True);
    }
}