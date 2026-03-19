namespace Termule.Components;

using Core;
using Types;
using Systems.RenderSystem;
using Systems.Display;

/// <summary>
/// Component that gets rendered <see cref="FrameBuffer"/>s from the <see cref="RenderSystem"/> and draws them to the <see cref="Display"/>.
/// </summary>
public sealed class Camera : Component
{
    private Transform transform;

    /// <summary>
    /// Initializes a new instance of the <see cref="Camera"/> class.
    /// </summary>
    public Camera()
    {
        this.Registered += () => this.transform = this.GetRequiredComponent<Transform>();
        this.Ticked += this.RenderView;
    }

    /// <summary>
    /// Gets or sets a cell that should fill the background of rendered <see cref="FrameBuffer"/>s.
    /// </summary>
    public Cell BackgroundCell { get; set; }

    private Vector ViewSize => this.GetRequiredSystem<Display>().Size;

    /// <summary>
    /// Converts a position from display-space to game-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in display-space.</param>
    /// <returns>The corresponding position in game-space.</returns>
    public Vector DisplayToGamePos(Vector pos)
    {
        Vector relativeDisplayPos = pos - ((Vector)this.ViewSize / 2f);
        Vector relativePos = (relativeDisplayPos.X, -relativeDisplayPos.Y);
        return relativePos - this.transform.Pos;
    }

    /// <summary>
    /// Converts a position from game-space to display-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in game-space.</param>
    /// <returns>The corresponding position in display-space.</returns>
    public Vector GameToDisplayPos(Vector pos)
    {
        Vector relativePos = pos - this.transform.Pos;
        Vector relativeDisplayPos = (relativePos.X, -relativePos.Y);
        return relativeDisplayPos - ((Vector)this.ViewSize / 2f);
    }

    private void RenderView()
    {
        Display display = this.GetRequiredSystem<Display>();

        Vector viewOrigin = this.transform.Pos + (new Vector(-this.ViewSize.X, this.ViewSize.Y) / 2f);
        display.Buffer.Reset(this.BackgroundCell);
        this.Game.Systems.Get<RenderSystem>().Render(viewOrigin, display.Buffer);

        display.Draw();
    }
}