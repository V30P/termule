namespace Termule.Systems.Controller.Keyboard;

/// <summary>
///     Bind that returns whether a <see cref="Button" /> was just pressed.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class TriggerBind(Button button) : KeyboardBind
{
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        var value = triggeredSinceLastFrame;
        triggeredSinceLastFrame = false;
        return value;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            triggeredSinceLastFrame = true;
        }
    }
}