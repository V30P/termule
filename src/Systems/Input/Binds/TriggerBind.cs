namespace Termule.Input;

public sealed class TriggerBind(Button button) : Bind
{
    private bool _triggeredSinceLastFrame;

    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            _triggeredSinceLastFrame = true;
        }
    }

    internal override object GetValue()
    {
        bool value = _triggeredSinceLastFrame;
        _triggeredSinceLastFrame = false;
        return value;
    }
}