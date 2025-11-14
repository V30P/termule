namespace Termule.Input;

public class VectorBind(Button posY, Button negX, Button negY, Button posX) : Bind
{
    private static readonly Dictionary<int, Vector> _directionVectors = new()
    {
        { 0, (0, 1) },
        { 1, (-1, 0) },
        { 2, (0, -1) },
        { 3, (1, 0) }
    };

    private readonly Button[] _buttons = [posY, negX, negY, posX];

    private Vector _vector = new();

    protected override void Hook()
    {
        InputHook.ButtonDown += (button) => OnButtonAction(button, true);
        InputHook.ButtonUp += (button) => OnButtonAction(button, false);
    }

    private void OnButtonAction(Button button, bool keyDown)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_buttons[i] == button)
            {
                _vector += keyDown ? _directionVectors[i] : -_directionVectors[i];
            }
        }
    }

    internal override object GetValue()
    {
        return _vector;
    }

    protected override void Unhook()
    {
        InputHook.ButtonDown += (keyCode) => OnButtonAction(keyCode, true);
        InputHook.ButtonUp += (keyCode) => OnButtonAction(keyCode, false);
    }
}