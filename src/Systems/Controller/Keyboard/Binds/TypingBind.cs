namespace Termule.Systems.Controller.Keyboard;

public sealed class TypingBind : KeyboardBind
{
    private string _textSinceLastFrame = "";

    protected override void OnCharacterTyped(char character)
    {
        _textSinceLastFrame += character;
    }

    internal override object GetValue()
    {
        string value = _textSinceLastFrame;
        _textSinceLastFrame = "";
        return value;
    }
}