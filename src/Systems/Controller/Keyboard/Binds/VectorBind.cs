namespace Termule.Systems.Controller.Keyboard;

using Types;

public sealed class VectorBind(Button posY, Button negX, Button negY, Button posX) : KeyboardBind
{
    private static readonly Dictionary<int, Vector> DirectionVectors = new()
    {
        { 0, (0, 1) },
        { 1, (-1, 0) },
        { 2, (0, -1) },
        { 3, (1, 0) },
    };

    private readonly Button[] buttons = [posY, negX, negY, posX];

    private Vector vector = new();

    internal override object GetValue()
    {
        return this.vector;
    }

    protected override void OnButtonDown(Button button)
    {
        this.OnButtonAction(button, true);
    }

    protected override void OnButtonUp(Button button)
    {
        this.OnButtonAction(button, false);
    }

    private void OnButtonAction(Button button, bool isDown)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.buttons[i] == button)
            {
                this.vector += isDown ? DirectionVectors[i] : -DirectionVectors[i];
            }
        }
    }
}