using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.Input;

/// <summary>
///     Bind whose value is a <see cref="Vector" /> based on the provided buttons.
/// </summary>
/// <param name="posY">The button for the positive Y direction.</param>
/// <param name="negX">The button for the negative X direction.</param>
/// <param name="negY">The button for the negative Y direction.</param>
/// <param name="posX">The button for the positive X direction.</param>
public sealed class VectorBind(Button posY, Button negX, Button negY, Button posX) : Bind
{
    private static readonly Dictionary<int, Vector> DirectionVectors = new()
    {
        { 0, (0, 1) }, { 1, (-1, 0) }, { 2, (0, -1) }, { 3, (1, 0) }
    };

    private readonly Button[] buttons = [posY, negX, negY, posX];

    private Vector vector;

    internal override object GetValue()
    {
        return vector;
    }

    /// <inheritdoc />
    protected override void OnButtonDown(Button button)
    {
        OnButtonAction(button, true);
    }

    /// <inheritdoc />
    protected override void OnButtonUp(Button button)
    {
        OnButtonAction(button, false);
    }

    private void OnButtonAction(Button button, bool isDown)
    {
        for (int i = 0; i < 4; i++)
        {
            if (buttons[i] == button)
            {
                vector += isDown ? DirectionVectors[i] : -DirectionVectors[i];
            }
        }
    }
}