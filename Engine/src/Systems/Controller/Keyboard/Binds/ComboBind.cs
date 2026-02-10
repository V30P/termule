namespace Termule.Systems.Controller.Keyboard;

/// <summary>
/// Bind whose value is true for the first tick where all provided buttons are pressed.
/// </summary>
/// <param name="buttons">The buttons to target.</param>
public sealed class ComboBind(HashSet<Button> buttons) : KeyboardBind
{
    private readonly HashSet<Button> heldButtons = [];
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = this.triggeredSinceLastFrame;
        this.triggeredSinceLastFrame = false;
        return value;
    }

    /// <inheritdoc/>
    protected override void OnButtonDown(Button button)
    {
        if (buttons.Contains(button))
        {
            this.heldButtons.Add(button);
            if (this.heldButtons.SetEquals(buttons))
            {
                this.triggeredSinceLastFrame = true;
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnButtonUp(Button button)
    {
        this.heldButtons.Remove(button);
    }
}