namespace Termule.Engine.Systems.Input;

internal abstract class ButtonMessage(Button button) : InputMessage
{
    internal Button Button { get; init; } = button;
}
