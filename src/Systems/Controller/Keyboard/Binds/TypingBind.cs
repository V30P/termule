namespace Termule.Systems.Controller.Keyboard;

public sealed class TypingBind : KeyboardBind
{
    private string textSinceLastFrame = string.Empty;

    internal override object GetValue()
    {
        string value = this.textSinceLastFrame;
        this.textSinceLastFrame = string.Empty;
        return value;
    }

    protected override void OnCharacterTyped(char character)
    {
        this.textSinceLastFrame += character;
    }
}