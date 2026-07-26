namespace Termule.Engine.Systems.Input;

internal sealed class ButtonPressed(Button Button) : InputMessage
{
    internal Button Button { get; init; } = Button;
}
