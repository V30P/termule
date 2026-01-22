using SharpHook;

namespace Termule.Systems.Controller.Keyboard;

public sealed class KeyboardController : Controller<KeyboardBind>
{
    private static readonly TaskPoolGlobalHook _sharpHook;

    private readonly HashSet<Button> _pressedButtons = [];

    private event Action<Button> ButtonDown;
    private event Action<Button> ButtonUp;
    private event Action<char> CharacterTyped;

    static KeyboardController()
    {
        _sharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);
        _sharpHook.RunAsync();
    }

    public KeyboardController()
    {
        _sharpHook.MousePressed += (_, e) => OnButtonPressed(e.Data.Button.ToButton());
        _sharpHook.MouseReleased += (_, e) => OnButtonReleased(e.Data.Button.ToButton());

        _sharpHook.KeyPressed += (_, e) => OnButtonPressed(e.Data.KeyCode.ToButton());
        _sharpHook.KeyReleased += (_, e) => OnButtonReleased(e.Data.KeyCode.ToButton());
        _sharpHook.KeyTyped += (_, e) => CharacterTyped?.Invoke(e.Data.KeyChar);
    }

    private void OnButtonPressed(Button? button)
    {
        if (button is not Button pressedButton || !_pressedButtons.Add(pressedButton))
        {
            return;
        }

        ButtonDown?.Invoke(pressedButton);
    }

    private void OnButtonReleased(Button? button)
    {
        if (button is not Button releasedButton || !_pressedButtons.Remove(releasedButton))
        {
            return;
        }

        ButtonUp?.Invoke(releasedButton);
    }

    public abstract class KeyboardBind : Bind<KeyboardController>
    {
        internal override KeyboardController Controller
        {
            set
            {
                if (_controller != null)
                {
                    _controller.ButtonDown -= OnButtonDown;
                    _controller.ButtonUp -= OnButtonUp;
                    _controller.CharacterTyped -= OnCharacterTyped;
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.ButtonDown += OnButtonDown;
                    _controller.ButtonUp += OnButtonUp;
                    _controller.CharacterTyped += OnCharacterTyped;
                }
            }
        }
        private KeyboardController _controller;

        internal KeyboardBind() { }

        protected virtual void OnButtonDown(Button button) { }
        protected virtual void OnButtonUp(Button button) { }
        protected virtual void OnCharacterTyped(char character) { }
    }
}

public abstract class KeyboardBind : KeyboardController.KeyboardBind
{
    internal KeyboardBind() { }
}