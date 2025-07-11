namespace Termule.Rendering;

public class Image(int sizeX, int sizeY)
{
    public readonly int sizeX = sizeX, sizeY = sizeY;

    readonly Color[,] values = new Color[sizeX, sizeY];
    public Color this[int x, int y] { get => values[x, y]; set => values[x, y] = value; } 
}