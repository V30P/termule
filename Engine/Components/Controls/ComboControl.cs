using Termule.Engine.Systems.Input;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is true for the first tick where several buttons are down.
/// </summary>
/// <param name="buttons">The buttons to target.</param>
public sealed class ComboControl(HashSet<Button> buttons) : Control<bool>
{
    private readonly HashSet<Button> heldButtons = [];

    private bool valueForTick;

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        Value = valueForTick;
        valueForTick = false;
    }

    /// <inheritdoc />
    private protected override void OnButtonDown(Button button)
    {
        if (!buttons.Contains(button))
        {
            return;
        }

        _ = heldButtons.Add(button);
        if (heldButtons.SetEquals(buttons))
        {
            valueForTick = true;
        }
    }

    /// <inheritdoc />
    private protected override void OnButtonUp(Button button)
    {
        _ = heldButtons.Remove(button);
    }
}
