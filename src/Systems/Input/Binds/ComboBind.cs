namespace Termule.Input;

public class ComboBind(HashSet<Button> buttons) : Bind
{
    private readonly HashSet<Button> _heldButtons = [];
    private bool _triggeredSinceLastFrame;


    protected override void Hook()
    {
        InputHook.ButtonDown += OnButtonDown;
        InputHook.ButtonUp += OnButtonUp;
    }

    private void OnButtonDown(Button button)
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

    private void OnButtonUp(Button button)
    {
        _heldButtons.Remove(button);
    }

    protected override void Unhook()
    {
        InputHook.ButtonDown -= OnButtonDown;
        InputHook.ButtonUp -= OnButtonUp;
    }

    internal override object GetValue()
    {
        bool value = _triggeredSinceLastFrame;
        _triggeredSinceLastFrame = false;
        return value;
    }
}