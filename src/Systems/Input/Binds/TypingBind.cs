namespace Termule.Input;

public sealed class TypingBind() : Bind
{
    private string _textSinceLastFrame = "";

    protected override void Hook()
    {
        InputHook.CharacterTyped += OnCharacterTyped;
    }

    private void OnCharacterTyped(char character)
    {
        _textSinceLastFrame += character;
    }

    internal override object GetValue()
    {
        string value = _textSinceLastFrame;
        _textSinceLastFrame = "";
        return value;
    }

    protected override void Unhook()
    {
        InputHook.CharacterTyped -= OnCharacterTyped;
    }
}