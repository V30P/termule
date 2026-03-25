using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Component that uses the <see cref="RenderSystem" /> to render its view to the <see cref="Display" />.
/// </summary>
public sealed class Camera : Component
{
    /// <summary>
    /// </summary>
    public ICameraTarget Target { get; set; }

    /// <summary>
    ///     Gets or sets a cell that should make up the background of rendered <see cref="FrameBuffer" />s.
    /// </summary>
    public Cell BackgroundCell { get; set; }

    private Vector TransformPos => GameObject.Get<Transform>()?.Pos ?? (0, 0);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Camera" /> class.
    /// </summary>
    public Camera()
    {
        Registered += OnRegistered;
        Ticked += RenderToTarget;
    }

    /// <summary>
    ///     Converts a position from target-space to game-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in target-space.</param>
    /// <returns>The corresponding position in game-space.</returns>
    public Vector TargetToGamePos(Vector pos)
    {
        if (Target == null)
        {
            throw new InvalidOperationException("Camera has no target.");
        }

        var relativeTargetPos = pos - (Vector)Target.Size / 2f;
        Vector relativePos = (relativeTargetPos.X, -relativeTargetPos.Y);
        return relativePos - TransformPos;
    }

    /// <summary>
    ///     Converts a position from game-space to target-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in game-space.</param>
    /// <returns>The corresponding position in target-space.</returns>
    public Vector GameToTargetPos(Vector pos)
    {
        if (Target == null)
        {
            throw new InvalidOperationException("Camera has no target.");
        }

        var relativePos = pos + TransformPos;
        Vector relativeTargetPos = (relativePos.X, -relativePos.Y);
        return relativeTargetPos + (Vector)Target.Size / 2f;
    }

    private void OnRegistered()
    {
        Target ??= GetRequiredSystem<Display>();
    }

    private void RenderToTarget()
    {
        Target.Buffer.Reset(BackgroundCell);
        GetRequiredSystem<RenderSystem>().Render(TargetToGamePos((0, 0)), Target.Buffer);

        Target.Update();
    }
}