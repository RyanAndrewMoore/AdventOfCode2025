namespace Day9;

public class TileMap
{
    public readonly List<Tile> _map;
    public readonly List<Line> _boundary = [];

    public TileMap(string fileName)
    {
        _map = File.ReadAllLines(fileName)
            .Select(s =>
                s.Split(',')
                    .Select(int.Parse)
                    .ToArray()
            ).Select(arr => new Tile()
                {
                    X = arr[0],
                    Y = arr[1]
                }
            ).ToList();

        _boundary.AddRange(
            _map.Take(_map.Count - 1).Select((tile, i) => new Line()
            {
                Start = (tile.X, tile.Y),
                End = (_map[i + 1].X, _map[i + 1].Y)
            })
        );

        // Connect last to first to complete loop
        _boundary.Add(new Line()
        {
            Start = (_map[^1].X, _map[^1].Y),
            End = (_map[0].X, _map[0].Y)
        });
        
        foreach (var line in _boundary)
        {
            line.Rectify();
        }
    }

    public TileMap(List<Tile> map)
    {
        _map = map;

        _boundary.AddRange(
            _map.Take(_map.Count - 1).Select((tile, i) => new Line()
            {
                Start = (tile.X, tile.Y),
                End = (_map[i + 1].X, _map[i + 1].Y)
            })
        );

        // Connect last to first to complete loop
        _boundary.Add(new Line()
        {
            Start = (_map[^1].X, _map[^1].Y),
            End = (_map[0].X, _map[0].Y)
        });

        foreach (var line in _boundary)
        {
            line.Rectify();
        }
    }


    public List<long> GetAllRectangleAreas()
    {
        return _map.SelectMany((_, i) => _map.Skip(i + 1), GetRectangleArea)
            .ToList();
    }

    public List<(long, Tile, Tile)> GetAllRectangleAreasWithTiles()
    {
        return _map.SelectMany((_, i) => _map.Skip(i + 1), GetRectangleAreaWithTiles)
            .ToList();
    }

    private static long GetRectangleArea(Tile tileA, Tile tileB)
    {
        return (Math.Abs(tileB.X - tileA.X) + 1) * (Math.Abs(tileB.Y - tileA.Y) + 1);
    }

    private static (long Area, Tile TileA, Tile TileB) GetRectangleAreaWithTiles(Tile tileA, Tile tileB)
    {
        return (GetRectangleArea(tileA, tileB), tileA, tileB);
    }

    private bool IsPointInLoop(int x, int y)
    {
        var horizontalLinePassthroughCount = _boundary
            .Count(boundaryLine =>
                boundaryLine.Start.X <= x &&
                boundaryLine.Start.Y <= y &&
                boundaryLine.End.X <= x &&
                boundaryLine.End.Y <= y
            );

        return horizontalLinePassthroughCount % 2 == 1;
        // var totalCrossCount = 0;
        //
        // foreach (var line in _boundary)
        // {
        //     if (line.IsPointOnLine(x, y))
        //     {
        //         return true;
        //     }
        //
        //     var crossCount = 0;
        //
        //     for (var i = 0; i < x; i++)
        //     {
        //         if (line.IsPointOnLine(i, y))
        //         {
        //             crossCount++;
        //             break;
        //         }
        //     }
        //
        //     totalCrossCount += crossCount switch
        //     {
        //         1 => 1,
        //         _ => 0
        //     };
        // }
        //
        // return totalCrossCount % 2 != 0;
    }

    public bool DoesLineCrossBoundary(Line line)
    {
        // var intersectableLines = _boundary.Where(boundaryLine =>
        //     line.Orientation != boundaryLine.Orientation
        // );

        var intersectableLines = _boundary;

        return intersectableLines.Any(boundaryLine => DoLinesCross(boundaryLine, line));

        // for (var i = line.Start.X; i <= line.End.X; i++)
        // {
        //     for (var j = line.Start.Y; j <= line.End.Y; j++)
        //     {
        //         if (!IsPointInLoop(i, j)) return true;
        //     }
        // }
        //
        // return false;
    }

    public static bool DoLinesCross(Line lineA, Line lineB)
    {
        var (horizontalLine, verticalLine) =
            lineA.Orientation == Line.Direction.Horizontal ? (lineA, lineB) : (lineB, lineA);


        if (horizontalLine.Start.X > verticalLine.Start.X ||
            verticalLine.Start.X > horizontalLine.End.X ||
            verticalLine.Start.Y > horizontalLine.Start.Y ||
            horizontalLine.Start.Y > verticalLine.End.Y)
        {
            return false;
        }

        // Line segments intersect but still need to check if they fully cross
        // to the other side. They do unless they share a corner
        return (horizontalLine.Start.X, horizontalLine.Start.Y) != (verticalLine.Start.X, verticalLine.Start.Y) &&
               (horizontalLine.Start.X, horizontalLine.Start.Y) != (verticalLine.End.X, verticalLine.End.Y) &&
               (horizontalLine.End.X, horizontalLine.End.Y) != (verticalLine.Start.X, verticalLine.Start.Y) &&
               (horizontalLine.End.X, horizontalLine.End.Y) != (verticalLine.End.X, verticalLine.End.Y);
    }
}