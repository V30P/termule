using System.Text.Json.Serialization;
using Termule.Resources;

namespace Termule.Rendering;

public class Content : IResource
{
    [JsonIgnore]
    public VectorInt Size => (Cells.GetLength(0), Cells.GetLength(1));

    [JsonInclude]
    protected Cell[,] Cells;

    static string IResource.FileExtension => ".tmc";

    internal Cell At(int x, int y)
    {
        return Cells[x, y];
    }

    internal bool EqualsAt(Content content, VectorInt pos)
    {
        return At(pos.X, pos.Y) == content.At(pos.X, pos.Y);
    }

    public Content(int x, int y)
    {
        Resize(x, y);
    }

    // Used for deserialization
    public Content() : this(0, 0) { }

    protected void Resize(int x, int y)
    {
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