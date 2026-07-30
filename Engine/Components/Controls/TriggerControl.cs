using Termule.Engine.Systems.Input;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is true for the first tick where a button is down.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class TriggerControl(Button button) : Control<bool>
{
    private bool valueForTick;

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        Value = valueForTick;
        valueForTick = false;
    }

    /// <inheritdoc />
    private protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            valueForTick = true;
        }
    }
}
