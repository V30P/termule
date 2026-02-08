namespace Termule.Systems.Controller.Keyboard;

/// <summary>
/// A binds whose values is all of the characters that have been typed in the last frame.
/// </summary>
public sealed class TypingBind : KeyboardBind
{
    private string textSinceLastFrame = string.Empty;

    internal override object GetValue()
    {
        string value = this.textSinceLastFrame;
        this.textSinceLastFrame = string.Empty;
        return value;
    }

    /// <inheritdoc/>
    protected override void OnCharacterTyped(char character)
    {
        this.textSinceLastFrame += character;
    }
}