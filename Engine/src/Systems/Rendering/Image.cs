namespace Termule.Rendering;

public class Image(int sizeX, int sizeY)
{
    public readonly int sizeX = sizeX, sizeY = sizeY;

    //public readonly char[,] text = new char[sizeX, sizeY];
    //public readonly Color[,] textColor = new Color[sizeX, sizeY];
    public readonly Color[,] backgroundColor = new Color[sizeX, sizeY];
}