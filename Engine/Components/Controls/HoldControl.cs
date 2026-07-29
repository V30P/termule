using Termule.Engine.Systems.Input;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value indicates if a button is currently down.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class HoldControl(Button button) : Control<bool>
{
    private protected override void OnButtonDown(Button heldButton)
    {
        if (heldButton == button)
        {
            Value = true;
        }
    }

    private protected override void OnButtonUp(Button heldButton)
    {
        if (heldButton == button)
        {
            Value = false;
        }
    }
}
