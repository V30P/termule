using System.Text.Json.Serialization;
using Termule.Engine.Systems.Resources;

namespace Termule.Engine.Types;

/// <summary>
///     Content implementation with methods for easy modification.
/// </summary>
public class Image : IContent, IResource
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="width">The width in cells.</param>
    /// <param name="height">The height in cells.</param>
    public Image(int width, int height)
    {
        Resize(width, height);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="i">The image whose cells should be duplicated.</param>
    public Image(Image i)
    {
        Cells = (Cell[,]) i.Cells.Clone();
    }

    [JsonConstructor]
#pragma warning disable IDE0051 // Remove unused private members
    private Image(Cell[,] cells)
#pragma warning restore IDE0051 // Remove unused private members
    {
        Cells = cells;
    }

    static string IResource.FileExtension => ".tmc";

    /// <summary>
    ///     Gets the size of this content.
    /// </summary>
    [JsonIgnore]
    public VectorInt Size => (Cells.GetLength(0), Cells.GetLength(1));

    /// <summary>
    ///     Gets or sets the array of <see cref="Cell" />s that make up this content.
    /// </summary>
    [JsonInclude]
    protected Cell[,] Cells { get; set; }

    /// <summary>
    ///     Gets or sets the cell at (x, y).
    /// </summary>
    /// <param name="x">The x position of the cell.</param>
    /// <param name="y">The y position of the cell.</param>
    /// <returns>The <see cref="Cell" /> at the given position.</returns>
    public Cell this[int x, int y]
    {
        get => Cells[x, y];
        set => Cells[x, y] = value;
    }

    internal bool EqualsAt(IContent content, VectorInt pos)
    {
        return this[pos.X, pos.Y] == content[pos.X, pos.Y];
    }

    /// <summary>
    ///     Resizes this image to the specified dimensions.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="width" /> or <paramref name="height" /> is negative.
    /// </exception>
    protected void Resize(int width, int height)
    {
        if (width < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), width, "Width cannot be negative");
        }

        if (height < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(height),
                height,
                "Height cannot be negative"
            );
        }

        Cells = new Cell[width, height];
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Cells[x, y] = default;
            }
        }
    }
}
