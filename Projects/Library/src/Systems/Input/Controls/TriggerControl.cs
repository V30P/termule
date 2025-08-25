namespace Termule.Input;

public class TriggerControl(Button button) : Control
{
    bool triggeredSinceLastFrame;

    protected override void Hook() => InputHook.ButtonDown += OnButtonDown;

    void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            triggeredSinceLastFrame = true;
        }
    }

    internal override object GetValue()
    {
        if (triggeredSinceLastFrame)
        {
            triggeredSinceLastFrame = false;
            return true;
        }

        return false;
    }

    protected override void Unhook() => InputHook.ButtonDown -= OnButtonDown;
}