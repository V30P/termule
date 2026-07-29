namespace Termule.Engine.Systems.Input;

internal sealed class CharTyped(char character) : InputMessage
{
    internal char Char { get; init; } = character;
}
