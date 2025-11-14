using Termule.Rendering;

namespace Termule;

public class Camera : Component
{
    internal static readonly List<Renderer> renderers = [];

    private Transform _transform;

    public Color BackgroundColor { get; set; }
    public VectorInt ViewSize = (0, 0);
    public bool DrawToDisplay = true;

    private Frame _lastFrame;

    public Camera()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
        Ticked += OnTicked;
    }

    private void OnTicked()
    {
        _lastFrame = GetFrame();
        if (DrawToDisplay)
        {
            Display.Draw(_lastFrame);
        }
    }

    private static Frame GetFrame(Vector viewOrigin, VectorInt viewSize, Color backgroundColor = Color.Black)
    {
        Frame frame = new(viewSize.X, viewSize.Y);
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                frame.Contribute(backgroundColor, x, y);
            }
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.Render(frame, viewOrigin, viewSize);
        }

        return frame;
    }

    public Frame GetFrame()
    {
        Vector viewCenter = _transform != null ? _transform.Pos : (0, 0);
        Vector viewOrigin = viewCenter + new Vector(-ViewSize.X / 2f, ViewSize.Y / 2f);

        return GetFrame(viewOrigin, ViewSize, BackgroundColor);
    }

    public Renderer[] GetOverlappers(Renderer renderer)
    {
        List<Renderer> overlappers = [];
        if (_lastFrame.contributions.TryGetValue(renderer, out List<VectorInt> contributions))
        {
            foreach (VectorInt contributionPosition in contributions)
            {
                List<Renderer> contributors = _lastFrame.blame[contributionPosition.X, contributionPosition.Y];
                overlappers.AddRange(contributors.Where(contributor => contributor != renderer));
            }
        }

        return [.. overlappers];
    }
}