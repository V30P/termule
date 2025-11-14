namespace Termule.Rendering;

public class Image
{
    public readonly VectorInt Size;

    public Color[,] Color;
    public char[,] Text;

    public Image(int sizeX, int sizeY)
    {
        Size = (sizeX, sizeY);

        Color = new Color[Size.X, Size.Y];
        Text = new char[Size.X, Size.Y];
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Text[x, y] = ' ';
            }
        }
    }
}