namespace Termule.Engine.Types;

/// <summary>
///     Rectangular collection of terminal cells.
/// </summary>
public interface IContent
{
    /// <summary>
    ///     Gets the Size of this <see cref="IContent"/>.
    /// </summary>
    protected internal VectorInt Size { get; }

    /// <summary>
    ///     Gets the cell at (<paramref name="x"/>, <paramref name="y"/>).
    /// </summary>
    /// <param name="x">The x position of the cell.</param>
    /// <param name="y">The y position of the cell.</param>
    /// <returns>The <see cref="Cell" /> at the given position.</returns>
    protected internal Cell this[int x, int y] { get; }
}
