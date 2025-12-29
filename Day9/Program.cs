using Day9;

var part1Tiles = new TileMap(args[0]);
var largestArea = part1Tiles.GetAllRectangleAreas().Max();

Console.WriteLine($"Part 1 Answer: {largestArea}");