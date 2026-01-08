using Clipper2Lib;

var filePath = args[0];

var redTiles = File.ReadAllLines(filePath)
    .SelectMany(s => s.Split(',')).Select(int.Parse).ToArray();

var tilePoly = new Paths64() { Clipper.MakePath(redTiles) };


var pointList = tilePoly.SelectMany(path64 => path64)
    .ToList();

var cornerPairs = pointList.SelectMany((_, i) =>
        pointList.Skip(i + 1), (point1, point2) => new
    {
        C1 = point1,
        C2 = point2
    }
);

var rectangles = cornerPairs.Select(pair =>
    new Rect64(
        l: Math.Min(pair.C1.X, pair.C2.X),
        t: Math.Min(pair.C1.Y, pair.C2.Y),
        r: Math.Max(pair.C1.X, pair.C2.X),
        b: Math.Max(pair.C1.Y, pair.C2.Y)
    )
).ToList();

var rectPaths = rectangles.Select(rect => new
        {
            RealRectangle = new Paths64()
            {
                Clipper.MakePath([
                    rect.left, rect.top,
                    rect.right, rect.top,
                    rect.right, rect.bottom,
                    rect.left, rect.bottom
                ])
            },

            BlockyRectangle = new Paths64()
            {
                Clipper.MakePath([
                    rect.left, rect.top,
                    rect.right + 1, rect.top,
                    rect.right + 1, rect.bottom + 1,
                    rect.left, rect.bottom + 1
                ])
            }
        }
    ).Select(arg => new
        {
            arg.RealRectangle,
            arg.BlockyRectangle,
            RealArea = Clipper.Area(arg.RealRectangle),
            BlockyArea = Clipper.Area(arg.BlockyRectangle)
        }
    )
    .ToList();


rectPaths.Sort((x, y) =>
    x.BlockyArea <= y.BlockyArea ? 1 : -1
);

var largestArea = 0L;
foreach (var tup in rectPaths)
{
    var intersect = Clipper.Intersect(tilePoly, tup.RealRectangle, FillRule.EvenOdd);
    if ((long) tup.RealArea == (long)Clipper.Area(intersect))
    {
        largestArea = (long) tup.BlockyArea;
        break;
    }
}

Console.WriteLine($"{largestArea}");