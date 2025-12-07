namespace Termule.Input;

public sealed class ButtonBind(Button button) : Bind
{
    private bool _pressed;

    protected override void Hook()
    {
        InputHook.ButtonDown += OnButtonDown;
        InputHook.ButtonUp += OnButtonUp;
    }

    private void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            _pressed = true;
        }
    }

    private void OnButtonUp(Button upBotton)
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

    protected override void Unhook()
    {
        InputHook.ButtonDown -= OnButtonDown;
    }
}