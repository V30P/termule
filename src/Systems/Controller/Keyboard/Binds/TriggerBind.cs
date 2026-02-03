namespace Termule.Systems.Controller.Keyboard;

public sealed class TriggerBind(Button button) : KeyboardBind
{
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = this.triggeredSinceLastFrame;
        this.triggeredSinceLastFrame = false;
        return value;
    }

    protected override void OnButtonDown(Button downButton)
    {
        if (downButton == button)
        {
            this.triggeredSinceLastFrame = true;
        }
    }
}