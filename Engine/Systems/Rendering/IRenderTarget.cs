using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     Denotes a type that can be rendered to by the <see cref="RenderSystem"/>.
/// </summary>
public interface IRenderTarget
{
    /// <summary>
    ///     Gets the (inclusive) bottom-leftmost corner of this target.
    /// </summary>
    public abstract VectorInt LowerBound { get; }

    /// <summary>
    ///     Gets the (exclusive) top-rightmost corner of this target.
    /// </summary>
    public abstract VectorInt UpperBound { get; }

    /// <summary>
    ///     Gets the size of this target.
    /// </summary>
    public VectorInt Size => UpperBound - LowerBound;

    /// <summary>
    ///     Gets a reference to the cell at (<paramref name="x"/>, <paramref name="y"/>).
    /// </summary>
    /// <param name="x">The x position of the cell.</param>
    /// <param name="y">The y position of the cell.</param>
    /// <returns>A reference to the <see cref="Cell" /> at the given position.</returns>
    protected internal abstract ref Cell this[int x, int y] { get; }
}
