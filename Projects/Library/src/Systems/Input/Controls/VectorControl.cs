namespace Termule.Input;

public class VectorControl(Button posY, Button negX, Button negY, Button posX) : Control
{
    readonly Dictionary<int, Vector> directionVectors = new Dictionary<int, Vector>()
    {
        { 0, (0, 1) },
        { 1, (-1, 0) },
        { 2, (0, -1) },
        { 3, (1, 0) }
    };

    readonly Button[] buttons = [posY, negX, negY, posX];

    Vector vector = new Vector();

    protected override void Hook()
    {
        InputHook.ButtonDown += (button) => OnButtonAction(button, true);
        InputHook.ButtonUp += (button) => OnButtonAction(button, false);
    }

    void OnButtonAction(Button button, bool keyDown)
    {
        for (int i = 0; i < 4; i++)
        {
            if (buttons[i] == button)
            {
                vector += keyDown ? directionVectors[i] : -directionVectors[i];
            }
        }
    }

    internal override object GetValue() => vector;

    protected override void Unhook()
    {
        InputHook.ButtonDown += (keyCode) => OnButtonAction(keyCode, true);
        InputHook.ButtonUp += (keyCode) => OnButtonAction(keyCode, false);
    }
}