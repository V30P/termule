namespace Termule.Engine.Systems.Input.Keyboard;

/// <summary>
///     Bind that returns whether a <see cref="Button" /> was just pressed.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class TriggerBind(Button button) : KeyboardController.Bind
{
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = triggeredSinceLastFrame;
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