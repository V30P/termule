namespace Termule.Rendering;

public sealed class Camera : Component
{
    private Transform _transform;

    public Color? BackgroundColor { get; set; }
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
        Vector viewCenter = _transform != null ? _transform.Pos : (0, 0);
        Vector viewOrigin = viewCenter + new Vector(-ViewSize.X / 2f, ViewSize.Y / 2f);

        _lastFrame = RenderSystem.Render(viewOrigin, ViewSize, BackgroundColor);
        if (Draw)
        {
            Display.Draw(_lastFrame);
        }
    }

    public Renderer[] GetOverlappers(Renderer renderer)
    {
        List<Renderer> overlappers = [];
        if (_lastFrame.Contributions.TryGetValue(renderer, out List<VectorInt> contributions))
        {
            foreach (VectorInt contributionPosition in contributions)
            {
                overlappers
                .AddRange(_lastFrame.Blame[contributionPosition.X, contributionPosition.Y]
                .Where(contributor => contributor != renderer));
            }
        }

        return [.. overlappers];
    }
}