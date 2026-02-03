namespace Termule.Components;

using Systems.RenderSystem;
using Types;

public abstract class TransformRenderer : Renderer
{
    private Transform transform;

    internal TransformRenderer()
    {
        this.Registered += () => this.transform = this.GetRequiredComponent<Transform>();
    }

    public bool ScreenSpace { get; set; }

    protected virtual Vector Offset { get; }

    protected internal sealed override void Render(Frame frame, Vector viewOrigin)
    {
        Vector framespacePos;
        if (!this.ScreenSpace)
        {
            // Get integer position relative to viewOrigin
            framespacePos = this.transform.Pos - viewOrigin;

            // Flip y to account for the change from world to screen space
            framespacePos = (framespacePos.X, -framespacePos.Y);
        }
        else
        {
            framespacePos = this.transform.Pos;
        }

        framespacePos += this.Offset + (0.5f, 0.5f);
        this.Render(frame, framespacePos.FloorToInt());
    }

    private protected abstract void Render(Frame frame, VectorInt framespacePos);
}