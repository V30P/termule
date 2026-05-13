namespace Termule.Engine.Systems.Input;

/// <summary>
///     Bind whose value is true for the first tick where all provided buttons are pressed.
/// </summary>
/// <param name="buttons">The buttons to target.</param>
public sealed class ComboBind(HashSet<Button> buttons) : Bind
{
    private readonly HashSet<Button> heldButtons = [];

    private bool triggeredSinceLastTick;

    internal override object GetValue()
    {
        bool value = triggeredSinceLastTick;
        triggeredSinceLastTick = false;
        return value;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button button)
    {
        if (!buttons.Contains(button))
        {
            return;
        }

        _ = heldButtons.Add(button);
        if (heldButtons.SetEquals(buttons))
        {
            triggeredSinceLastTick = true;
        }
    }

    /// <inheritdoc />
    protected override void OnButtonUp(Button button)
    {
        _ = heldButtons.Remove(button);
    }
}