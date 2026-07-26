namespace Termule.Engine.Systems.Input;

internal sealed class CharTyped(char Char) : InputMessage
{
    internal char Char { get; init; } = Char;
}
