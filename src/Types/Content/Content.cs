using System.Text.Json.Serialization;
using Termule.Systems.ResourceLoader;

namespace Termule.Types;

public class Content : IResource
{
    [JsonIgnore]
    public VectorInt Size => (Cells.GetLength(0), Cells.GetLength(1));

    [JsonInclude]
    protected Cell[,] Cells;

    static string IResource.FileExtension => ".tmc";

    internal Cell At(int x, int y)
    {
        if (x < 0 || x >= Size.X)
        {
            throw new ArgumentOutOfRangeException(nameof(x), x, "x must be withing the bounds of the Content");
        }
        else if (y < 0 || y >= Size.Y)
        {
            throw new ArgumentOutOfRangeException(nameof(y), y, "y must be withing the bounds of the Content");
        }

        return Cells[x, y];
    }

    internal bool EqualsAt(Content content, VectorInt pos)
    {
        return At(pos.X, pos.Y) == content.At(pos.X, pos.Y);
    }

    public Content(int width, int height)
    {
        Resize(width, height);
    }

    // Used for deserialization
    public Content() : this(0, 0) { }

    protected void Resize(int width, int height)
    {
        if (width < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), width, "Width cannot be negative");
        }
        if (height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), height, "Height cannot be negative");
        }

        Cells = new Cell[width, height];
        for (width = 0; width < Size.X; width++)
        {
            for (height = 0; height < Size.Y; height++)
            {
                Cells[width, height] = new Cell();
            }
        }
    }
}

public readonly record struct Cell(Color Color, char Char, Color CharColor) { }