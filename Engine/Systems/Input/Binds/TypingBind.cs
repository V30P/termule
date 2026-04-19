namespace Termule.Engine.Systems.Input.Keyboard;

/// <summary>
///     Bind whose value is all of the characters that have been typed in the last tick.
/// </summary>
public sealed class TypingBind : KeyboardController.Bind
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