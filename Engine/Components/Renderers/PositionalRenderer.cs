using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Base component for renderers that draw relative to their local <see cref="Transform"/>'s
///     position.
/// </summary>
public abstract class PositionalRenderer : Renderer
{
    internal PositionalRenderer()
    {
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="Transform" />'s position should be treated as
    ///     target-space during rendering.
    /// </summary>
    public bool TargetSpace { get; set; }

    /// <summary>
    ///     Gets an offset applied to the transform position before rendering.
    /// </summary>
    protected virtual Vector Offset { get; }

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

        RenderAtPosition(
            new PositionalRenderContext(
                frame,
                frameSpaceCellOrigin,
                frameSpaceOrigin - frameSpaceCellOrigin
            )
        );
    }

    private protected abstract void RenderAtPosition(PositionalRenderContext context);

    private protected readonly struct PositionalRenderContext(
        FrameBuffer frame,
        VectorInt origin,
        Vector offset)
    {
        public readonly FrameBuffer Frame = frame;
        public readonly VectorInt Origin = origin;
        public readonly Vector Offset = offset;
    }
}
