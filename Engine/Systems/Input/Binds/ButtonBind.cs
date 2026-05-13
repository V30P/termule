namespace Termule.Engine.Systems.Input;

/// <summary>
///     Bind whose value is whether a button is currently down.
/// </summary>
/// <param name="button">The target button.</param>
public sealed class ButtonBind(Button button) : Bind
{
    private readonly Button button = button;
    private bool pressed;

    internal override object GetValue()
    {
        return pressed;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button button)
    {
        if (button == this.button)
        {
            pressed = true;
        }
    }

    /// <inheritdoc />
    protected override void OnButtonUp(Button button)
    {
        if (button == this.button)
        {
            pressed = false;
        }
    }
}
