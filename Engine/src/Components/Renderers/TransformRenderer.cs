namespace Termule.Components;

using Systems.RenderSystem;
using Types;

/// <summary>
/// A <see cref="Renderer"/> implementation to ease rendering at the position of a <see cref="Transform"/>.
/// </summary>
public abstract class TransformRenderer : Renderer
{
    internal TransformRenderer()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether rendering should occur at the Display space or Game space position.
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

            // Flip y to account for the change from world to display space
            framespacePos = (framespacePos.X, -framespacePos.Y);
        }
        else
        {
            framespacePos = this.GetRequiredComponent<Transform>().Pos;
        }

        framespacePos += this.Offset + (0.5f, 0.5f);
        this.Render(frame, framespacePos.FloorToInt());
    }

    /// <summary>
    /// Renders to the provided <see cref="Frame"/> at the specified integer frame-space position.
    /// </summary>
    /// <param name="frame">The frame to which contributions should be made.</param>
    /// <param name="displaySpacePos">The integer position within the frame (frame-space) at which to render.</param>
    private protected abstract void Render(Frame frame, VectorInt displaySpacePos);
}