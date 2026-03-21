namespace Termule.Engine.Types.Content;

/// <summary>
///     Content with methods for easy modification.
/// </summary>
public sealed class Image : Content
{
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

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="width">The number of cells per row.</param>
    /// <param name="height">The number of cells per column.</param>
    public Image(int width, int height)
        : base(width, height)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="content">The content whose cells should be duplicated.</param>
    public Image(Content content)
        : base(content)
    {
    }
}