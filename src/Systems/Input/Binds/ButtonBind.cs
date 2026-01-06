namespace Termule.Input;

public sealed class ButtonBind(Button button) : Bind
{
    private bool _pressed;

    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            _pressed = true;
        }
    }

    protected override void OnButtonUp(Button upBotton)
    {
        if (upBotton == button)
        {
            _pressed = false;
        }
    }

    internal override object GetValue()
    {
        return _pressed;
    }
}