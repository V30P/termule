using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is the position of the mouse in cells.
/// </summary>
public sealed class MouseControl() : Control<VectorInt>
{
    private protected override void OnMouseMoved(VectorInt pos)
    {
        Value = pos;
    }
}
