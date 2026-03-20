using Termule.Core;
using Termule.Systems.Display;
using Termule.Systems.RenderSystem;
using Termule.Types.Content;
using Termule.Types.Vectors;

namespace Termule.Components;

/// <summary>
///     Component that gets rendered <see cref="FrameBuffer" />s from the <see cref="RenderSystem" /> and draws them to the
///     <see cref="Display" />.
/// </summary>
public sealed class Camera : Component
{
    private Transform transform;

    /// <summary>
    ///     Gets or sets a cell that should make up the background of rendered <see cref="FrameBuffer" />s.
    /// </summary>
    public Cell BackgroundCell { get; set; }

    private Vector ViewSize => GetRequiredSystem<Display>().Size;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Camera" /> class.
    /// </summary>
    public Camera()
    {
        Registered += () => transform = GetRequiredComponent<Transform>();
        Ticked += RenderView;
    }

    /// <summary>
    ///     Converts a position from display-space to game-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in display-space.</param>
    /// <returns>The corresponding position in game-space.</returns>
    public Vector DisplayToGamePos(Vector pos)
    {
        var relativeDisplayPos = pos - ViewSize / 2f;
        Vector relativePos = (relativeDisplayPos.X, -relativeDisplayPos.Y);
        return relativePos - transform.Pos;
    }

    /// <summary>
    ///     Converts a position from game-space to display-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in game-space.</param>
    /// <returns>The corresponding position in display-space.</returns>
    public Vector GameToDisplayPos(Vector pos)
    {
        var relativePos = pos - transform.Pos;
        Vector relativeDisplayPos = (relativePos.X, -relativePos.Y);
        return relativeDisplayPos - ViewSize / 2f;
    }

    private void RenderView()
    {
        var display = GetRequiredSystem<Display>();

        var viewOrigin = transform.Pos + new Vector(-ViewSize.X, ViewSize.Y) / 2f;
        display.Buffer.Reset(BackgroundCell);
        Game.Systems.Get<RenderSystem>().Render(viewOrigin, display.Buffer);

        display.Draw();
    }
}