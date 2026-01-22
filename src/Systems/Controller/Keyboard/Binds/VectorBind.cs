using Termule.Types;

namespace Termule.Systems.Controller.Keyboard;

public sealed class VectorBind(Button posY, Button negX, Button negY, Button posX) : KeyboardBind
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

    protected override void OnButtonDown(Button button)
    {
        OnButtonAction(button, true);
    }

    protected override void OnButtonUp(Button button)
    {
        OnButtonAction(button, false);
    }


    private void OnButtonAction(Button button, bool isDown)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_buttons[i] == button)
            {
                _vector += isDown ? _directionVectors[i] : -_directionVectors[i];
            }
        }
    }

    internal override object GetValue()
    {
        return _vector;
    }
}