using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Denotes a target that can be rendered to by <see cref="Camera" />s.
/// </summary>
public interface ICameraTarget
{
    /// <summary>
    ///     Gets the size of the camera target (in cells).
    /// </summary>
    VectorInt Size { get; }

    internal FrameBuffer Buffer { get; private protected set; }

    internal void Update();
}