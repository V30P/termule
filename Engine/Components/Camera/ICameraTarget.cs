using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Denotes a target that can be rendered to by <see cref="Camera" />s.
/// </summary>
public interface ICameraTarget
{
    /// <summary>
    ///     Gets the size of this target (in cells).
    /// </summary>
    public VectorInt Size { get; }

    internal IRenderTarget GetRenderTarget();

    internal void Update();
}
