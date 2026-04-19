namespace Termule.Engine.Systems.Input;

/// <summary>
///     Bind whose value is all the characters typed in the last tick.
/// </summary>
public sealed class TypingBind : Bind
{
    private string textSinceLastFrame = string.Empty;

    internal override object GetValue()
    {
        string value = textSinceLastFrame;
        textSinceLastFrame = string.Empty;
        return value;
    }

    /// <inheritdoc />
    protected override void OnCharacterTyped(char character)
    {
        textSinceLastFrame += character;
    }
}