namespace Termule.Types;

using System.Text.Json.Serialization;
using Systems.ResourceLoader;

/// <summary>
/// Rectangular collection of terminal cells.
/// </summary>
public class Content : IResource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Content"/> class.
    /// </summary>
    /// <param name="width">The number of cells per row.</param>
    /// <param name="height">The number of cells per column.</param>
    public Content(int width, int height)
    {
        this.Resize(width, height);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Content"/> class.
    /// </summary>
    /// <param name="c">The content whose cells should be duplicated.</param>
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

    /// <summary>
    /// Gets the size of this content.
    /// </summary>
    [JsonIgnore]
    public VectorInt Size => (this.Cells.GetLength(0), this.Cells.GetLength(1));

    /// <summary>
    /// Gets or sets the array of <see cref="Cell"/>s that make up this content.
    /// </summary>
    [JsonInclude]
    protected Cell[,] Cells { get; set; }

    internal bool EqualsAt(Content content, VectorInt pos)
    {
        return this.At(pos.X, pos.Y) == content.At(pos.X, pos.Y);
    }

    /// <summary>
    /// Gets the cell at position (<paramref name="x"/>, <paramref name="y"/>).
    /// </summary>
    /// <param name="x">The x position of the cell.</param>
    /// <param name="y">The y position of the cell.</param>
    /// <returns>The cell at the given position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="x"/> or <paramref name="y"/> is outside the bounds of the content.</exception>
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

    /// <summary>
    /// Resizes this content to the specified dimensions.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/> or <paramref name="height"/> are negative.</exception>
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
