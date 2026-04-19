namespace Termule.Engine.Systems.RenderSystem;

/// <summary>
///     Basic layer implementation that provides registration-order-based sorting.
/// </summary>
public sealed class SimpleLayer : Layer
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleLayer" /> class.
    /// </summary>
    public SimpleLayer()
        : base((r1, r2) => r1.ElementId.CompareTo(r2.ElementId))
    {
    }
}