namespace Termule.Components;

using Systems.RenderSystem;
using Types;

/// <summary>
/// Base component class for renderers that render based on their local transform's position.
/// </summary>
public abstract class TransformRenderer : Renderer
{
    internal TransformRenderer()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Transform"/>'s
    /// position should be considered display-space when rendering.
    /// </summary>
    public bool DisplaySpace { get; set; }

    /// <summary>
    /// Gets an offset to apply to position before rendering.
    /// </summary>
    protected virtual Vector Offset { get; }

    /// <inheritdoc/>
    protected internal sealed override void Render(Frame frame, Vector viewOrigin)
    {
        Vector framespacePos;
        if (!this.DisplaySpace)
        {
            // Get integer position relative to viewOrigin
            framespacePos = this.GetRequiredComponent<Transform>().Pos - viewOrigin;

            // Flip y to account for the change from game space to display space.
            framespacePos = (framespacePos.X, -framespacePos.Y);
        }
        else
        {
            framespacePos = this.GetRequiredComponent<Transform>().Pos;
        }

        framespacePos += this.Offset + (0.5f, 0.5f);
        this.Render(frame, framespacePos.FloorToInt());
    }

    private protected abstract void Render(Frame frame, VectorInt displaySpacePos);
}