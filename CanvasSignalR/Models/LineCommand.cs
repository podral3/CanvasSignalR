namespace CanvasSignalR.Models;

public record LineCommand
{
    public int X1 { get; init; }
    public int Y1 { get; init; }
    public int X2 { get; init; }
    public int Y2 { get; init; }
    public string Color { get; init; }
    public int PenSize { get; init; }
}
