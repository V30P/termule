using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Base component for renderers that render based on their local transform's position.
/// </summary>
public abstract class TransformRenderer : Renderer
{
    /// <summary>
    ///     Gets or sets whether the <see cref="Transform" />'s position is interpreted as display-space during rendering.
    /// </summary>
    public bool DisplaySpace { get; set; }

    /// <summary>
    ///     Gets an offset applied to the transform position before rendering.
    /// </summary>
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    protected virtual Vector Offset { get; }

    internal TransformRenderer()
    {
    }

    /// <inheritdoc />
    protected internal sealed override void Render(FrameBuffer frame, Vector viewOrigin)
    {
        Vector frameSpacePos;
        if (!DisplaySpace)
        {
            // Get integer position relative to viewOrigin
            frameSpacePos = GetRequiredComponent<Transform>().Pos - viewOrigin;

            // Flip y to account for the change from game space to display space.
            frameSpacePos = (frameSpacePos.X, -frameSpacePos.Y);
        }
        else
        {
            frameSpacePos = GetRequiredComponent<Transform>().Pos;
        }

        frameSpacePos += Offset + (0.5f, 0.5f);
        RenderAtPosition(frame, frameSpacePos);
    }

    private protected abstract void RenderAtPosition(FrameBuffer frame, Vector frameSpacePos);
}