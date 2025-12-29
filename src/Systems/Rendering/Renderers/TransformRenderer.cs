namespace Termule.Rendering;

public abstract class TransformRenderer : Renderer
{
    private Transform _transform;
    public bool ScreenSpace;

    internal TransformRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
    }

    internal sealed override void Render(Frame frame, Vector viewOrigin)
    {
        VectorInt framespacePos;
        if (!ScreenSpace)
        {
            // Get integer position relative to viewOrigin
            framespacePos = (_transform.Pos - viewOrigin).RoundToInt();
            // Flip y to account for the change from world to screen space 
            framespacePos = new VectorInt(framespacePos.X, -framespacePos.Y);
        }
        else
        {
            framespacePos = _transform.Pos.RoundToInt();
        }

        Render(frame, framespacePos);
    }

    private protected abstract void Render(Frame frame, VectorInt framespacePos);
}