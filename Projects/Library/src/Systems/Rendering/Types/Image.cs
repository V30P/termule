using Microsoft.Build.Logging;

namespace Termule.Rendering;

public class Image
{
    public readonly VectorInt size;

    public Color[,] color;
    public char[,] text;

    public Image(int sizeX, int sizeY)
    {
        size = (sizeX, sizeY);

        color = new Color[size.x, size.y];
        text = new char[size.x, size.y];
        for (int x = 0; x < size.x; x++) for (int y = 0; y < size.y; y++) text[x, y] = ' ';
    }
}