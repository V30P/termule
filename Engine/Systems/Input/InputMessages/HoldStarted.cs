namespace Termule.Engine.Systems.Input;

internal sealed class HoldStarted(Button Button) : InputMessage
{
    internal Button Button { get; init; } = Button;
}
