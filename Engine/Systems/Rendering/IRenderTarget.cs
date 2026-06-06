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

    internal abstract ref Cell GetCellRef(int x, int y);
}
