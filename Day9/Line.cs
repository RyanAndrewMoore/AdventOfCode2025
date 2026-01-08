namespace Day9;

public class Line
{
    public (int X, int Y) Start { get; set; }
    public (int X, int Y) End { get; set; }
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public Direction Orientation => Start.X == End.X ? Direction.Vertical : Direction.Horizontal;

    public bool IsPointOnLine(int x, int y)
    {
        return x >= Math.Min(Start.X, End.X) && x <= Math.Max(Start.X, End.X) &&
               y >= Math.Min(Start.Y, End.Y) && y <= Math.Max(Start.Y, End.Y);
    }

    public void Rectify()
    {
        if (Start.X + Start.Y > End.X + End.Y)
        {
            (Start, End) = (End, Start);
        }
    }
}