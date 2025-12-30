namespace Day9;

public class TileMap
{
    private readonly List<Tile> _map;
    
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
    }

    public List<long> GetAllRectangleAreas()
    {
        return _map.SelectMany((_, i) => _map.Skip(i + 1), GetRectangleArea)
            .ToList();
    }

    private static long GetRectangleArea(Tile tileA, Tile tileB)
    {
        return (Math.Abs(tileB.X - tileA.X) + 1) * (Math.Abs(tileB.Y - tileA.Y) + 1);
    }
}