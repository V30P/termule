namespace Termule.Systems.Controller.Keyboard;

public sealed class ComboBind(HashSet<Button> buttons) : KeyboardBind
{
    private readonly HashSet<Button> _heldButtons = [];
    private bool _triggeredSinceLastFrame;

    protected override void OnButtonDown(Button button)
    {
        if (buttons.Contains(button))
        {
            _heldButtons.Add(button);
            if (_heldButtons.SetEquals(buttons))
            {
                _triggeredSinceLastFrame = true;
            }
        }
    }

    protected override void OnButtonUp(Button button)
    {
        _heldButtons.Remove(button);
    }

    internal override object GetValue()
    {
        bool value = _triggeredSinceLastFrame;
        _triggeredSinceLastFrame = false;
        return value;
    }
}