using Termule.Core;
using Termule.Types;
using Termule.Systems.RenderSystem;
using Termule.Systems.Display;

namespace Termule.Components;

public sealed class Camera : Component
{
    private Transform _transform;

    public VectorInt ViewSize
    {
        get => MatchDisplaySize ? GetRequiredSystem<Display>().Size : field;
        set
        {
            if (value.X < 0 || value.Y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ViewSize), ViewSize, "ViewSize dimensions cannot be negative");
            }

            field = value;
        }
    } = (0, 0);

    public bool Draw = true;
    public bool MatchDisplaySize = false;

    private Frame _lastFrame;

    public Camera()
    {
        Registered += () => _transform = GetRequiredComponent<Transform>();
        Ticked += RenderView;
    }

    private void RenderView()
    {
        Vector viewOrigin = _transform.Pos + (new Vector(-ViewSize.X, ViewSize.Y) / 2f);

        _lastFrame = Game.Systems.Get<RenderSystem>().Render(viewOrigin, ViewSize);
        if (Draw)
        {
            GetRequiredSystem<Display>().Draw(_lastFrame);
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

    public Vector DisplayToGamePos(Vector pos)
    {
        Vector relativeDisplayPos = pos - ((Vector)ViewSize / 2f);
        Vector relativePos = (relativeDisplayPos.X, -relativeDisplayPos.Y);
        return relativePos - _transform.Pos;
    }

    public Vector GameToDisplayPos(Vector pos)
    {
        Vector relativePos = pos - _transform.Pos;
        Vector relativeDisplayPos = (relativePos.X, -relativePos.Y);
        return relativeDisplayPos - ((Vector)ViewSize / 2f);
    }
}