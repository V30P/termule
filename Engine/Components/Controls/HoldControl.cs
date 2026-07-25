using Termule.Engine.Systems.Input;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is whether a button is currently down.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class HoldControl(Button button) : Control<bool>
{
    private protected override void OnHoldStarted(Button heldButton)
    {
        if (heldButton == button)
        {
            Value = true;
        }
    }

    private protected override void OnHoldStopped(Button heldButton)
    {
        if (heldButton == button)
        {
            Value = false;
        }
    }
}
