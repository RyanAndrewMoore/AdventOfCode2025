using System.Drawing;
using SkiaSharp;
using Day9;
using Rectangle = Day9.Rectangle;

var bmp = new SKBitmap(3000, 3000);
using SKCanvas canvas = new(bmp);

var part1Tiles = new TileMap(args[0]);
var largestArea = part1Tiles.GetAllRectangleAreas().Max();

Console.WriteLine($"Part 1 Answer: {largestArea}");

var part2Tiles = new TileMap(args[0]);

List<(long Area, Tile TileA, Tile TileB)> allAreasWithTiles = part2Tiles.GetAllRectangleAreasWithTiles();

allAreasWithTiles.Sort(((areaA, areaB) => areaA.Area.CompareTo(areaB.Area)));
allAreasWithTiles.Reverse();

var largestValidArea = 0L;

var valid = true;

foreach (var area in allAreasWithTiles)
{
    var rectangle = new Rectangle(area.TileA, area.TileB);

    foreach (var line in rectangle.BoundaryLines)
    {
        if (part2Tiles.DoesLineCrossBoundary(line)) valid = false;
    }

    if (valid)
    {
        largestValidArea = area.Area;
        break;
    }

    valid = true;
}

foreach (var tile in part2Tiles._map)
{
    canvas.DrawCircle(new SKPoint((float)tile.X / 35, (float)tile.Y / 35), 5f, new SKPaint
    {
        Color = new SKColor((uint)Color.Red.ToArgb())
    });
}

foreach (var line in part2Tiles._boundary)
{
    canvas.DrawLine(
        line.Start.X / 35f,
        line.Start.Y / 35f,
        line.End.X / 35f,
        line.End.Y / 35f,
        new SKPaint
        {
            Color = new SKColor((uint)Color.Green.ToArgb()),
            StrokeWidth = 5
        }
    );
}

// SKFileWStream fs = new("/home/rmoore/RiderProjects/AdventOfCode/AdventOfCode2025/Day9/input/canvas.jpg");

// bmp.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 50);