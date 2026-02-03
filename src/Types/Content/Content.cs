namespace Termule.Types;

using System.Text.Json.Serialization;
using Systems.ResourceLoader;

public class Content : IResource
{
    public Content(int width, int height)
    {
        this.Resize(width, height);
    }

    public Content(Content c)
    {
        this.Cells = (Cell[,])c.Cells.Clone();
    }

    [JsonConstructor]
#pragma warning disable IDE0051
    private Content(Cell[,] cells)
#pragma warning restore IDE0051
    {
        this.Cells = cells;
    }

    static string IResource.FileExtension => ".tmc";

    [JsonIgnore]
    public VectorInt Size => (this.Cells.GetLength(0), this.Cells.GetLength(1));

    [JsonInclude]
    protected Cell[,] Cells { get; set; }

    internal bool EqualsAt(Content content, VectorInt pos)
    {
        return this.At(pos.X, pos.Y) == content.At(pos.X, pos.Y);
    }

    protected internal Cell At(int x, int y)
    {
        if (x < 0 || x >= this.Size.X)
        {
            throw new ArgumentOutOfRangeException(nameof(x), x, "x must be within the bounds of the Content");
        }
        else if (y < 0 || y >= this.Size.Y)
        {
            throw new ArgumentOutOfRangeException(nameof(y), y, "y must be within the bounds of the Content");
        }

        return this.Cells[x, y];
    }

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

        this.Cells = new Cell[width, height];
        for (int x = 0; x < this.Size.X; x++)
        {
            for (int y = 0; y < this.Size.Y; y++)
            {
                this.Cells[x, y] = default;
            }
        }
    }
}

#pragma warning disable SA1313
public readonly record struct Cell(Color Color, char Char, Color CharColor) { }
#pragma warning restore SA1313
