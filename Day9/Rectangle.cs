namespace Day9;

public class Rectangle
{
    public readonly Line[] BoundaryLines;

    public Rectangle(Tile tileA, Tile tileB)
    {
        var startX = Math.Min(tileA.X, tileB.X);
        var startY = Math.Min(tileA.Y, tileB.Y);
        var endX = Math.Max(tileA.X, tileB.X);
        var endY = Math.Max(tileA.Y, tileB.Y);

        BoundaryLines =
        [
            new Line()
            {
                Start = (startX, startY),
                End = (endX, startY)
            },
            new Line()
            {
                Start = (startX, endY),
                End = (endX, endY)
            },
            new Line()
            {
                Start = (startX, startY),
                End = (startX, endY)
            },
            new Line()
            {
                Start = (endX, startY),
                End = (endX, endY)
            }
        ];

    }
}