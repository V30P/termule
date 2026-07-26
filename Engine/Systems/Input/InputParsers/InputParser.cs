namespace Termule.Engine.Systems.Input;

internal abstract class InputParser
{
    internal string Remainder { get; private protected set; }

    internal abstract IEnumerable<InputMessage> Parse(string input);
}
