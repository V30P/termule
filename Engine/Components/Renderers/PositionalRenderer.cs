using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Base component for renderers that draw relative to their local <see cref="Transform"/>'s
///     position.
/// </summary>
public abstract class PositionalRenderer : Renderer
{
    internal PositionalRenderer()
    {
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="Transform"/>'s position should
    ///     be treated as target-space during rendering.
    /// </summary>
    public bool RenderInTargetSpace { get; set; }

    /// <summary>
    ///     Gets an offset applied to the transform position before rendering.
    /// </summary>
    protected virtual Vector Offset { get; }

    /// <inheritdoc />
    protected internal sealed override void Render(IRenderTarget target, Vector viewOrigin)
    {
        Vector origin = GetRequiredComponent<Transform>().Pos + Offset;

        // Transform to world space
        if (!RenderInTargetSpace)
        {
            origin -= viewOrigin;
            origin = (origin.X, -origin.Y);
        }

        VectorInt originCell = origin.FloorToInt();
        RenderPositionally(
            new PositionalRenderTarget(target, originCell, RenderInTargetSpace),
            origin - originCell
        );
    }

    private protected abstract void RenderPositionally(
        IRenderTarget target,
        Vector subPixelOffset
    );

    private protected class PositionalRenderTarget(
        IRenderTarget target,
        VectorInt origin,
        bool renderInTargetSpace) : IRenderTarget
    {
        VectorInt IRenderTarget.LowerBound { get; } = (
            target.LowerBound.X - origin.X,
            renderInTargetSpace
                ? target.LowerBound.Y - origin.Y
                : origin.Y - (target.UpperBound.Y - 1)
        );

        VectorInt IRenderTarget.UpperBound { get; } = (
            target.UpperBound.X - origin.X,
            renderInTargetSpace
                ? target.UpperBound.Y - origin.Y
                : origin.Y - (target.LowerBound.Y - 1)
        );

        ref Cell IRenderTarget.GetCellRef(int x, int y)
        {
            VectorInt targetPos = LocalToTargetPos((x, y));
            return ref target.GetCellRef(targetPos.X, targetPos.Y);
        }

        private VectorInt LocalToTargetPos(VectorInt pos)
        {
            return origin + (renderInTargetSpace ? pos : (pos.X, -pos.Y));
        }
    }
}
