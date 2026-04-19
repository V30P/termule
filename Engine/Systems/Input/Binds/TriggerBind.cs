namespace Termule.Engine.Systems.Input;

/// <summary>
///     Bind whose value is true when a button is first pressed.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class TriggerBind(Button button) : Bind
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