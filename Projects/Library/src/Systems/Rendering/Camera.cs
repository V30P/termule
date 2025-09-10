using Termule.Rendering;

namespace Termule;

public class Camera : Component
{
    internal static readonly List<Renderer> renderers = [];

    Transform transform;

    public Color backgroundColor { get; set; }
    public VectorInt viewSize = (0, 0);
    public bool drawToDisplay = true;

    Frame lastFrame;

    public Camera()
    {
        Rooted += () => transform = gameObject.Get<Transform>();
        Ticked += OnTicked;
    }

    void OnTicked()
    {
        lastFrame = GetFrame();
        if (drawToDisplay)
        {
            Display.Draw(lastFrame);
        }
    }

    static Frame GetFrame(Vector viewOrigin, VectorInt viewSize, Color backgroundColor = Color.Black)
    {
        Frame frame = new Frame(viewSize.x, viewSize.y);
        for (int x = 0; x < frame.size.x; x++) for (int y = 0; y < frame.size.y; y++) frame.Contribute(backgroundColor, x, y);

        foreach (Renderer renderer in renderers)
        {
            renderer.Render(frame, viewOrigin, viewSize);
        }

        return frame;
    }

    public Frame GetFrame()
    {
        Vector viewCenter = transform != null ? transform.pos : (0, 0);
        Vector viewOrigin = viewCenter + new Vector(-viewSize.x / 2f, viewSize.y / 2f);

        return GetFrame(viewOrigin, viewSize, backgroundColor);
    }

    public Renderer[] GetOverlappers(Renderer renderer)
    {
        List<Renderer> overlappers = [];
        if (lastFrame.contributions.TryGetValue(renderer, out List<VectorInt> contributions))
        {
            foreach (VectorInt contributionPosition in contributions)
            {
                overlappers
                .AddRange(lastFrame.blame[contributionPosition.x, contributionPosition.y]
                .Where(contributor => contributor != renderer));
            }
        }

        return [.. overlappers];
    }
}