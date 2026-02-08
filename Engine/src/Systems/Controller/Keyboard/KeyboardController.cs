namespace Termule.Systems.Controller.Keyboard;

using SharpHook;

/// <summary>
/// A <see cref="Controller"/> that handles input from a keyboard (and mouse).
/// </summary>
public sealed class KeyboardController : Controller<KeyboardBind>
{
    private static readonly TaskPoolGlobalHook SharpHook;

    private readonly HashSet<Button> pressedButtons = [];

    static KeyboardController()
    {
        SharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);
        SharpHook.RunAsync();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardController"/> class.
    /// </summary>
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

    /// <summary>
    /// The base class for Keyboard binds.
    /// </summary>
    public abstract class KeyboardBindBase : Bind<KeyboardController>
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

        /// <summary>
        /// Invoked when a Button is first pressed.
        /// </summary>
        /// <param name="button">The button that was pressed.</param>
        protected virtual void OnButtonDown(Button button)
        {
        }

        /// <summary>
        /// Invoked when a button is first released.
        /// </summary>
        /// <param name="button">The button that was released.</param>
        protected virtual void OnButtonUp(Button button)
        {
        }

        /// <summary>
        /// Invoked when a valid character is produced from keyboard input.
        /// </summary>
        /// <param name="character">The produced character.</param>
        protected virtual void OnCharacterTyped(char character)
        {
        }
    }
}

/// <summary>
/// The base class of Binds that exist on a KeyboardController and consume its events.
/// </summary>
public abstract class KeyboardBind : KeyboardController.KeyboardBindBase
{
    internal KeyboardBind()
    {
    }
}