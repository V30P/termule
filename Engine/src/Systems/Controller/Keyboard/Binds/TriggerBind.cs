namespace Termule.Systems.Controller.Keyboard;

/// <summary>
/// A <see cref="Bind"/> whose value is whether a <see cref="Button"/> was just pressed.
/// </summary>
/// <param name="button">The target <see cref="Button"/>.</param>
public sealed class TriggerBind(Button button) : KeyboardBind
{
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = this.triggeredSinceLastFrame;
        this.triggeredSinceLastFrame = false;
        return value;
    }

    /// <inheritdoc/>
    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            this.triggeredSinceLastFrame = true;
        }
    }
}