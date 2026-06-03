using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Component that renders its surroundings to an <see cref="ICameraTarget"/>.
/// </summary>
public sealed class Camera : Component
{
    /// <summary>
    ///     Gets or sets the target that this camera should render to.
    /// </summary>
    public ICameraTarget Target
    {
        get => field ?? GetRequiredSystem<DisplaySystem>();

        set;
    }

    /// <summary>
    ///     Converts a position from target-space to world-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in target space.</param>
    /// <returns>The corresponding position in world space.</returns>
    public Vector TargetToGamePos(Vector pos)
    {
        Vector relativeTargetPos = pos - ((Vector) Target.Size / 2f);
        Vector relativePos = (relativeTargetPos.X, -relativeTargetPos.Y);
        return relativePos - GetRequiredComponent<IPositionProvider>().Pos;
    }

    /// <summary>
    ///     Converts a position from world-space to target-space relative to this camera.
    /// </summary>
    /// <param name="pos">The position in world space.</param>
    /// <returns>The corresponding position in target space.</returns>
    public Vector GameToTargetPos(Vector pos)
    {
        Vector relativePos = pos + GetRequiredComponent<IPositionProvider>().Pos;
        Vector relativeTargetPos = (relativePos.X, -relativePos.Y);
        return relativeTargetPos + ((Vector) Target.Size / 2f);
    }

    /// <inheritdoc />
    protected internal override void Tick()
    {
        GetRequiredSystem<RenderSystem>().Render(
            Target.GetRenderTarget(),
            TargetToGamePos((0, 0))
        );

        Target.Update();
    }
}
