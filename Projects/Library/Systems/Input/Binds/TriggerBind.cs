namespace Termule.Input;

public class TriggerBind(Button button) : Bind
{
    private bool _triggeredSinceLastFrame;

    protected override void Hook()
    {
        InputHook.ButtonDown += OnButtonDown;
    }

    private void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            _triggeredSinceLastFrame = true;
        }
    }

    internal override object GetValue()
    {
        if (_triggeredSinceLastFrame)
        {
            _triggeredSinceLastFrame = false;
            return true;
        }

        return false;
    }

    protected override void Unhook()
    {
        InputHook.ButtonDown -= OnButtonDown;
    }
}