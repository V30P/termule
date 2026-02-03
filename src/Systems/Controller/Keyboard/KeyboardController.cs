namespace Termule.Systems.Controller.Keyboard;

using SharpHook;

public sealed class KeyboardController : Controller<KeyboardBind>
{
    private static readonly TaskPoolGlobalHook SharpHook;

    private readonly HashSet<Button> pressedButtons = [];

    static KeyboardController()
    {
        SharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);
        SharpHook.RunAsync();
    }

    public KeyboardController()
    {
        SharpHook.MousePressed += (_, e) => this.OnButtonPressed(e.Data.Button.ToButton());
        SharpHook.MouseReleased += (_, e) => this.OnButtonReleased(e.Data.Button.ToButton());

        SharpHook.KeyPressed += (_, e) => this.OnButtonPressed(e.Data.KeyCode.ToButton());
        SharpHook.KeyReleased += (_, e) => this.OnButtonReleased(e.Data.KeyCode.ToButton());
        SharpHook.KeyTyped += (_, e) => this.CharacterTyped?.Invoke(e.Data.KeyChar);
    }

    private event Action<Button> ButtonDown;

    private event Action<Button> ButtonUp;

    private event Action<char> CharacterTyped;

    private void OnButtonPressed(Button? button)
    {
        if (button is not Button pressedButton || !this.pressedButtons.Add(pressedButton))
        {
            return;
        }

        this.ButtonDown?.Invoke(pressedButton);
    }

    private void OnButtonReleased(Button? button)
    {
        if (button is not Button releasedButton || !this.pressedButtons.Remove(releasedButton))
        {
            return;
        }

        this.ButtonUp?.Invoke(releasedButton);
    }

    public abstract class KeyboardBind : Bind<KeyboardController>
    {
        private KeyboardController controller;

        internal override KeyboardController Controller
        {
            set
            {
                if (this.controller != null)
                {
                    this.controller.ButtonDown -= this.OnButtonDown;
                    this.controller.ButtonUp -= this.OnButtonUp;
                    this.controller.CharacterTyped -= this.OnCharacterTyped;
                }

                this.controller = value;

                if (this.controller != null)
                {
                    this.controller.ButtonDown += this.OnButtonDown;
                    this.controller.ButtonUp += this.OnButtonUp;
                    this.controller.CharacterTyped += this.OnCharacterTyped;
                }
            }
        }

        protected virtual void OnButtonDown(Button button)
        {
        }

        protected virtual void OnButtonUp(Button button)
        {
        }

        protected virtual void OnCharacterTyped(char character)
        {
        }
    }
}

public abstract class KeyboardBind : KeyboardController.KeyboardBind
{
    internal KeyboardBind()
    {
    }
}