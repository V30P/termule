namespace Termule.Systems.Controller.Keyboard;

public sealed class TriggerBind(Button button) : KeyboardBind
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