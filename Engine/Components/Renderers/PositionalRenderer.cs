using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Base component for renderers that render based on their local transform's position.
/// </summary>
public abstract class PositionalRenderer : Renderer
{
    /// <summary>
    ///     Gets or sets whether the <see cref="Transform" />'s position is interpreted as target-space during rendering.
    /// </summary>
    public bool TargetSpace { get; set; }

    /// <summary>
    ///     Gets an offset applied to the transform position before rendering.
    /// </summary>
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    protected virtual Vector Offset { get; }

    internal PositionalRenderer()
    {
    }

    /// <inheritdoc />
    protected internal sealed override void Render(FrameBuffer frame, Vector viewOrigin)
    {
        Vector frameSpaceOrigin = GetRequiredComponent<Transform>().Pos;
        if (!TargetSpace)
        {
            // Get integer position relative to viewOrigin
            frameSpaceOrigin -= viewOrigin;

            // Flip y to account for the change from game space to frame space
            frameSpaceOrigin = (frameSpaceOrigin.X, -frameSpaceOrigin.Y);
        }

        frameSpaceOrigin += Offset;
        VectorInt frameSpaceCellOrigin = frameSpaceOrigin.RoundToInt();

        RenderAtPosition(new PositionalRenderContext(frame, frameSpaceCellOrigin,
            frameSpaceOrigin - frameSpaceCellOrigin));
    }

    private protected abstract void RenderAtPosition(PositionalRenderContext context);

    private protected class PositionalRenderContext(FrameBuffer frame, VectorInt origin, Vector offset)
    {
        public readonly FrameBuffer Frame = frame;
        public readonly VectorInt Origin = origin;
        public readonly Vector Offset = offset;
    }
}