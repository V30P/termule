namespace Termule.Types;

public sealed class Image : Content
{
    public Cell this[int x, int y]
    {
        get => Cells[x, y];
        set => Cells[x, y] = value;
    }

    public Image(int x, int y) : base(x, y) { }

    public Image(Content content) : base(content.Size.X, content.Size.Y)
    {
        Cells = new Cell[Size.X, Size.Y];
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                this[x, y] = content.At(x, y);
            }
        }
    }
}
