namespace Termule.Components;

using Core;
using Types;
using Systems.RenderSystem;
using Systems.Display;

/// <summary>
/// A <see cref="Component"/> that gets rendered <see cref="Frame"/>s and draws them to the <see cref="Display"/>.
/// </summary>
public sealed class Camera : Component
{
    private Transform transform;
    private Frame lastFrame;

    /// <summary>
    /// Initializes a new instance of the <see cref="Camera"/> class.
    /// </summary>
    public Camera()
    {
        this.Registered += () => this.transform = this.GetRequiredComponent<Transform>();
        this.Ticked += this.RenderView;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Camera should draw <see cref="Frame"/>s to the <see cref="Display"/>.
    /// </summary>
    public bool Draw { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Camera should render with its view size matching the size of the <see cref="Display"/>.
    /// </summary>
    public bool MatchDisplaySize { get; set; } = false;

    /// <summary>
    /// Gets or sets the size of view that the Camera should render.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a color that should fill the background <see cref="Content"/> for rendered <see cref="Frame"/>s.
    /// </summary>
    public Color BackgroundColor { get; set; }

    private Content Background
    {
        get
        {
            // Regenerate value if the ViewSize or BackgroundColor have changed
            if (ViewSize != field.Size || (field.Size != (0, 0) && field.At(0, 0).Color == BackgroundColor))
            {
                Image newBackground = new(ViewSize.X, ViewSize.Y);
                for (int x = 0; x < ViewSize.X; x++)
                {
                    for (int y = 0; y < ViewSize.Y; y++)
                    {
                        newBackground[x, y] = new Cell() { Color = BackgroundColor };
                    }
                }

                field = newBackground;
            }

            return field;
        }
    }

    = new(0, 0);

    /// <summary>
    /// Gets all renderers that contributed to any cell also contributed by the specified renderer.
    /// </summary>
    /// <param name="renderer">The renderer whose overlapping contributors to inspect.</param>
    /// <returns>An array of renderers that overlapped the specified renderer's contributions; empty if none or if no frame has been rendered yet.</returns>
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

    /// <summary>
    /// Converts a position from display space to game space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in display coordinates.</param>
    /// <returns>The corresponding position in game coordinates.</returns>
    public Vector DisplayToGamePos(Vector pos)
    {
        Vector relativeDisplayPos = pos - ((Vector)this.ViewSize / 2f);
        Vector relativePos = (relativeDisplayPos.X, -relativeDisplayPos.Y);
        return relativePos - this.transform.Pos;
    }

    /// <summary>
    /// Converts a position from game space to display space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in game coordinates.</param>
    /// <returns>The corresponding position in display coordinates.</returns>
    public Vector GameToDisplayPos(Vector pos)
    {
        Vector relativePos = pos - this.transform.Pos;
        Vector relativeDisplayPos = (relativePos.X, -relativePos.Y);
        return relativeDisplayPos - ((Vector)this.ViewSize / 2f);
    }

    /// <summary>
    /// Renders the camera's view into a frame and updates <see cref="lastFrame"/>.
    /// If <see cref="Draw"/> is true the frame is also drawn to the <see cref="Display"/> system.
    /// </summary>
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