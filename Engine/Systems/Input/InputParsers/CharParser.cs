namespace Termule.Engine.Systems.Input;

internal sealed partial class CharParser : ASCIIParser
{
    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (char character in input)
        {
            yield return new CharTyped(character);

            if (TryConvertASCIIToButton(character, out Button key))
            {
                yield return new ButtonPressed(key);
            }
        }

        Remainder = string.Empty;
    }
}
