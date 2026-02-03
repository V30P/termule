namespace Termule.Systems.Controller.Keyboard;

public sealed class ButtonBind(Button button) : KeyboardBind
{
    private bool pressed;

    internal override object GetValue()
    {
        return this.pressed;
    }

    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            this.pressed = true;
        }
    }

    protected override void OnButtonUp(Button upBotton)
    {
        if (upBotton == button)
        {
            this.pressed = false;
        }
    }
}