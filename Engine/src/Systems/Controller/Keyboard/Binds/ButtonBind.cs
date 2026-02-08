namespace Termule.Systems.Controller.Keyboard;

/// <summary>
/// A <see cref="Bind"/> whose value is whether a <see cref="Button"/> is currently down.
/// </summary>
/// <param name="button">The target <see cref="Button"/>.</param>
public sealed class ButtonBind(Button button) : KeyboardBind
{
    private bool pressed;

    internal override object GetValue()
    {
        return this.pressed;
    }

    /// <inheritdoc/>
    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            this.pressed = true;
        }
    }

    /// <inheritdoc/>
    protected override void OnButtonUp(Button upBotton)
    {
        if (upBotton == button)
        {
            this.pressed = false;
        }
    }
}