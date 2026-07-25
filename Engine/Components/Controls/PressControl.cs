using Termule.Engine.Systems.Input;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is true when a button is first pressed.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class PressControl(Button button) : Control<bool>
{
    private bool valueForTick;

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        Value = valueForTick;
        valueForTick = false;
    }

    /// <inheritdoc />
    private protected override void OnButtonPressed(Button pressedButton)
    {
        if (pressedButton == button)
        {
            valueForTick = true;
        }
    }
}
