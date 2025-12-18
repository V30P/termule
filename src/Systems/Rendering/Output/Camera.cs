namespace Termule.Rendering;

public sealed class Camera : Component
{
    private Transform _transform;

    public VectorInt ViewSize = (0, 0);
    public bool Draw = true;

    private Frame _lastFrame;

    public Camera()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
        Ticked += OnTicked;
    }

    private void OnTicked()
    {
        Vector viewOrigin = _transform.Pos + (new Vector(-ViewSize.X, ViewSize.Y) / 2f);

        _lastFrame = RenderSystem.Render(viewOrigin, ViewSize);
        if (Draw)
        {
            Display.Draw(_lastFrame);
        }
    }

    public Renderer[] GetOverlappers(Renderer renderer)
    {
        List<Renderer> overlappers = [];
        if (_lastFrame.Contributions.TryGetValue(renderer, out HashSet<VectorInt> contributions))
        {
            foreach (VectorInt pos in contributions)
            {
                overlappers.AddRange(_lastFrame.Contributors[pos.X, pos.Y].Where(contributor => contributor != renderer));
            }
        }

        return [.. overlappers];
    }
}