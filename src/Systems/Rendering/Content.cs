namespace Termule.Rendering;

public class Content
{
    public VectorInt Size { get; private set; }
    protected Cell[,] Cells;

    internal Cell At(int x, int y)
    {
        return Cells[x, y];
    }

    internal bool EqualsAt(Content content, VectorInt pos)
    {
        return (uint)pos.X < Size.X && (uint)pos.X < content?.Size.X
            && (uint)pos.Y < Size.Y && (uint)pos.Y < content.Size.Y
            && At(pos.X, pos.Y) == content.At(pos.X, pos.Y);
    }

    internal Content() { }

    internal Content(int x, int y)
    {
        Resize(x, y);
    }

    protected void Resize(int x, int y)
    {
        Size = (x, y);
        Cells = new Cell[x, y];
        for (x = 0; x < Size.X; x++)
        {
            for (y = 0; y < Size.Y; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
    }
}

public readonly record struct Cell(Color Color, char Char, Color CharColor) { }