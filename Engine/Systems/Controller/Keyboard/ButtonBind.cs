namespace Termule.Systems.Controller.Keyboard;

/// <summary>
///     Bind that returns whether a button is currently down.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class ButtonBind(Button button) : KeyboardBind
{
    private bool pressed;

    internal override object GetValue()
    {
        return pressed;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            pressed = true;
        }
    }

    /// <inheritdoc />
    protected override void OnButtonUp(Button upBotton)
    {
        if (upBotton == button)
        {
            pressed = false;
        }
    }
}