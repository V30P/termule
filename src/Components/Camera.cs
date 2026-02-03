namespace Termule.Components;

using Core;
using Types;
using Systems.RenderSystem;
using Systems.Display;

public sealed class Camera : Component
{
    private Transform transform;
    private Frame lastFrame;

    public Camera()
    {
        this.Registered += () => this.transform = this.GetRequiredComponent<Transform>();
        this.Ticked += this.RenderView;
    }

    public bool Draw { get; set; } = true;

    public bool MatchDisplaySize { get; set; } = false;

    public VectorInt ViewSize
    {
        get => MatchDisplaySize ? GetRequiredSystem<Display>().Size : field;
        set
        {
            if (value.X < 0 || value.Y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ViewSize), ViewSize, "ViewSize dimensions cannot be negative");
            }

            MatchDisplaySize = false;
            field = value;
        }
    }

    = (0, 0);

    public Color BackgroundColor { get; set; }

    private Content Background
    {
        get
        {
            // Regenerate value if the ViewSize or BackgroundColor have changed
            if (ViewSize != field.Size || (field.Size != (0, 0) && field.At(0, 0).Color == BackgroundColor))
            {
                Image newBaseContent = new(ViewSize.X, ViewSize.Y);
                for (int x = 0; x < ViewSize.X; x++)
                {
                    for (int y = 0; y < ViewSize.Y; y++)
                    {
                        newBaseContent[x, y] = new Cell() { Color = BackgroundColor };
                    }
                }

                field = newBaseContent;
            }

            return field;
        }
    }

    = new(0, 0);

    public Renderer[] GetOverlappers(Renderer renderer)
    {
        if (this.lastFrame == null)
        {
            return [];
        }

        List<Renderer> overlappers = [];
        if (this.lastFrame.Contributions.TryGetValue(renderer, out HashSet<VectorInt> contributions))
        {
            foreach (VectorInt pos in contributions)
            {
                overlappers.AddRange(this.lastFrame.Contributors[pos.X, pos.Y].Where(contributor => contributor != renderer));
            }
        }

        return [.. overlappers];
    }

    public Vector DisplayToGamePos(Vector pos)
    {
        Vector relativeDisplayPos = pos - ((Vector)this.ViewSize / 2f);
        Vector relativePos = (relativeDisplayPos.X, -relativeDisplayPos.Y);
        return relativePos - this.transform.Pos;
    }

    public Vector GameToDisplayPos(Vector pos)
    {
        Vector relativePos = pos - this.transform.Pos;
        Vector relativeDisplayPos = (relativePos.X, -relativePos.Y);
        return relativeDisplayPos - ((Vector)this.ViewSize / 2f);
    }

    private void RenderView()
    {
        Vector viewOrigin = this.transform.Pos + (new Vector(-this.ViewSize.X, this.ViewSize.Y) / 2f);

        this.lastFrame = this.Game.Systems.Get<RenderSystem>().Render(viewOrigin, this.Background);
        if (this.Draw)
        {
            this.GetRequiredSystem<Display>().Draw(this.lastFrame);
        }
    }
}