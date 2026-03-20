using Termule.Systems.Display;
using Termule.Types.Vectors;

namespace Termule.Components;

/// <summary>
///     Base component for renderers that contribute based on their local transform's position.
/// </summary>
public abstract class TransformRenderer : Renderer
{
    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="Transform" />'s
    ///     position should be considered display-space when rendering.
    /// </summary>
    public bool DisplaySpace { get; set; }

    /// <summary>
    ///     Gets an offset to apply to the transform's position before rendering.
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
        Render(frame, frameSpacePos.FloorToInt());
    }

    private protected abstract void Render(FrameBuffer frame, VectorInt frameSpacePos);
}