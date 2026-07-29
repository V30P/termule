using Termule.Engine.Systems.Input;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is a <see cref="Vector" /> derived from the state of four buttons.
/// </summary>
/// <param name="posY">The button for the positive Y direction.</param>
/// <param name="negX">The button for the negative X direction.</param>
/// <param name="negY">The button for the negative Y direction.</param>
/// <param name="posX">The button for the positive X direction.</param>
public sealed class VectorControl(Button posY, Button negX, Button negY, Button posX) : Control<Vector>
{
    private static readonly Vector[] DirectionVectors = [(0, 1), (-1, 0), (0, -1), (1, 0)];

    private readonly Button[] buttons = [posY, negX, negY, posX];
    private readonly bool[] directionStates = new bool[4];

    /// <inheritdoc />
    private protected override void OnButtonDown(Button button)
    {
        OnButtonStateChange(button, true);
    }

    /// <inheritdoc />
    private protected override void OnButtonUp(Button button)
    {
        OnButtonStateChange(button, false);
    }

    private void OnButtonStateChange(Button button, bool isDown)
    {
        for (int i = 0; i < 4; i++)
        {
            // We need to keep track of direction state because ButtonDown and ButtonUp
            // messages are not guaranteed to be 1-to-1
            if (buttons[i] == button && directionStates[i] != isDown)
            {
                Value += isDown ? DirectionVectors[i] : -DirectionVectors[i];
                directionStates[i] = isDown;
            }
        }
    }
}
