namespace Termule.Systems.Controller.Keyboard;

/// <summary>
///     Bind whose value is all of the characters that have been typed in the last tick.
/// </summary>
public sealed class TypingBind : KeyboardBind
{
    private string textSinceLastFrame = string.Empty;

    internal override object GetValue()
    {
        var value = textSinceLastFrame;
        textSinceLastFrame = string.Empty;
        return value;
    }

    /// <inheritdoc />
    protected override void OnCharacterTyped(char character)
    {
        textSinceLastFrame += character;
    }
}