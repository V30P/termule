namespace Termule.Engine.Systems.Input;

internal sealed class HoldStopped(Button Button) : InputMessage
{
    internal Button Button { get; init; } = Button;
}
