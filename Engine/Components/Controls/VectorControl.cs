using Termule.Engine.Systems.Input;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is a <see cref="Vector" /> based on the provided buttons.
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
    private protected override void OnHoldStarted(Button button)
    {
        OnHold(button, true);
    }

    /// <inheritdoc />
    private protected override void OnHoldStopped(Button button)
    {
        OnHold(button, false);
    }

    private void OnHold(Button button, bool isStarted)
    {
        for (int i = 0; i < 4; i++)
        {
            // We need to keep track of direction state because HoldStarted and HoldStopped
            // messages are not guaranteed to be 1-to-1
            if (buttons[i] == button && directionStates[i] != isStarted)
            {
                Value += isStarted ? DirectionVectors[i] : -DirectionVectors[i];
                directionStates[i] = isStarted;
            }
        }
    }
}
