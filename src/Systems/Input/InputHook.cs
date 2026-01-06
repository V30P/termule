using SharpHook;

namespace Termule.Input;

public static class InputHook
{
    private static readonly TaskPoolGlobalHook _sharpHook;

    private static readonly HashSet<Button> _pressedButtons = [];

    private static event Action<Button> ButtonDown;
    private static event Action<Button> ButtonUp;
    private static event Action<char> CharacterTyped;

    static InputHook()
    {
        _sharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);

        _sharpHook.MousePressed += (_, e) => OnButtonPressed(e.Data.Button.ToButton());
        _sharpHook.MouseReleased += (_, e) => OnButtonReleased(e.Data.Button.ToButton());

        _sharpHook.KeyPressed += (_, e) => OnButtonPressed(e.Data.KeyCode.ToButton());
        _sharpHook.KeyReleased += (_, e) => OnButtonReleased(e.Data.KeyCode.ToButton());
        _sharpHook.KeyTyped += (_, e) => CharacterTyped?.Invoke(e.Data.KeyChar);

        _sharpHook.RunAsync();
    }

    private static void OnButtonPressed(Button? button)
    {
        if (button is not Button pressedButton || !_pressedButtons.Add(pressedButton))
        {
            return;
        }

        ButtonDown?.Invoke(pressedButton);
    }

    private static void OnButtonReleased(Button? button)
    {
        if (button is not Button releasedButton || !_pressedButtons.Remove(releasedButton))
        {
            return;
        }

        ButtonUp?.Invoke(releasedButton);
    }

    public abstract class Bind
    {
        internal bool Active
        {
            set
            {
                if (_active == value)
                {
                    return;
                }
                _active = value;

                if (_active)
                {
                    ButtonDown += OnButtonDown;
                    ButtonUp += OnButtonUp;
                    CharacterTyped += OnCharacterTyped;
                }
                else
                {
                    ButtonDown -= OnButtonDown;
                    ButtonUp -= OnButtonUp;
                    CharacterTyped -= OnCharacterTyped;
                }
            }
        }

        private bool _active;

        internal Bind() { }

        protected virtual void OnButtonDown(Button button) { }
        protected virtual void OnButtonUp(Button button) { }
        protected virtual void OnCharacterTyped(char character) { }

        internal abstract object GetValue();
    }
}

public abstract class Bind : InputHook.Bind
{
    public Bind() { }
}