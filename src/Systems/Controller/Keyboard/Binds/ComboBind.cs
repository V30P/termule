namespace Termule.Systems.Controller.Keyboard;

public sealed class ComboBind(HashSet<Button> buttons) : KeyboardBind
{
    private readonly HashSet<Button> heldButtons = [];
    private bool triggeredSinceLastFrame;

    internal override object GetValue()
    {
        bool value = this.triggeredSinceLastFrame;
        this.triggeredSinceLastFrame = false;
        return value;
    }

    protected override void OnButtonDown(Button button)
    {
        if (buttons.Contains(button))
        {
            this.heldButtons.Add(button);
            if (this.heldButtons.SetEquals(buttons))
            {
                this.triggeredSinceLastFrame = true;
            }
        }
    }

    protected override void OnButtonUp(Button button)
    {
        this.heldButtons.Remove(button);
    }
}