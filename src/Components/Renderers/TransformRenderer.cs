using Termule.Systems.RenderSystem;
using Termule.Types;

namespace Termule.Components;

public abstract class TransformRenderer : Renderer
{
    private Transform _transform;
    public bool ScreenSpace;

    protected virtual Vector Offset { get; }

    internal TransformRenderer()
    {
        Registered += () => _transform = GetRequiredComponent<Transform>();
    }

    internal sealed override void Render(Frame frame, Vector viewOrigin)
    {
        Vector framespacePos;
        if (!ScreenSpace)
        {
            // Get integer position relative to viewOrigin
            framespacePos = _transform.Pos - viewOrigin;
            // Flip y to account for the change from world to screen space 
            framespacePos = (framespacePos.X, -framespacePos.Y);
        }
        else
        {
            framespacePos = _transform.Pos;
        }
        framespacePos += Offset + (0.5f, 0.5f);

        Render(frame, framespacePos.FloorToInt());
    }

    private protected abstract void Render(Frame frame, VectorInt framespacePos);
}