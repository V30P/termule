namespace Termule.Engine.Systems.Controller.Keyboard;

/// <summary>
///     Bind whose value is true for the first tick where all provided buttons are pressed.
/// </summary>
/// <param name="buttons">The buttons to target.</param>
public sealed class ComboBind(HashSet<Button> buttons) : KeyboardBind
{
    private readonly HashSet<Button> heldButtons = [];

    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = triggeredSinceLastFrame;
        triggeredSinceLastFrame = false;
        return value;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button button)
    {
        if (!buttons.Contains(button))
        {
            return;
        }

        heldButtons.Add(button);
        if (heldButtons.SetEquals(buttons))
        {
            triggeredSinceLastFrame = true;
        }
    }

    /// <inheritdoc />
    protected override void OnButtonUp(Button button)
    {
        heldButtons.Remove(button);
    }
}