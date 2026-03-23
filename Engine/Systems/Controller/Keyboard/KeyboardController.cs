using SharpHook;

namespace Termule.Engine.Systems.Controller.Keyboard;

/// <summary>
///     Controller that handles keyboard and mouse input.
/// </summary>
public sealed class KeyboardController : Controller
{
    private static readonly TaskPoolGlobalHook SharpHook;

    private readonly HashSet<Button> pressedButtons = [];

    private event Action<Button> ButtonDown;

    private event Action<Button> ButtonUp;

    private event Action<char> CharacterTyped;

    static KeyboardController()
    {
        SharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);
        SharpHook.RunAsync();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KeyboardController" /> class.
    /// </summary>
    public KeyboardController()
    {
        SharpHook.MousePressed += (_, e) => OnButtonPressed(e.Data.Button.ToButton());
        SharpHook.MouseReleased += (_, e) => OnButtonReleased(e.Data.Button.ToButton());

        SharpHook.KeyPressed += (_, e) => OnButtonPressed(e.Data.KeyCode.ToButton());
        SharpHook.KeyReleased += (_, e) => OnButtonReleased(e.Data.KeyCode.ToButton());
        SharpHook.KeyTyped += (_, e) => CharacterTyped?.Invoke(e.Data.KeyChar);
    }

    private void OnButtonPressed(Button? button)
    {
        if (button is not { } pressedButton || !pressedButtons.Add(pressedButton))
        {
            return;
        }

        ButtonDown?.Invoke(pressedButton);
    }

    private void OnButtonReleased(Button? button)
    {
        if (button is not { } releasedButton || !pressedButtons.Remove(releasedButton))
        {
            return;
        }

        ButtonUp?.Invoke(releasedButton);
    }

    /// <summary>
    ///     Base class for keyboard binds.
    /// </summary>
    public abstract class KeyboardBind : Bind<KeyboardController>
    {
        internal override KeyboardController Controller
        {
            set
            {
                if (field != null)
                {
                    field.ButtonDown -= OnButtonDown;
                    field.ButtonUp -= OnButtonUp;
                    field.CharacterTyped -= OnCharacterTyped;
                }

                field = value;

                if (field != null)
                {
                    field.ButtonDown += OnButtonDown;
                    field.ButtonUp += OnButtonUp;
                    field.CharacterTyped += OnCharacterTyped;
                }
            }
        }

        /// <summary>
        ///     Invoked when a button is first pressed.
        /// </summary>
        /// <param name="button">The button that was pressed.</param>
        protected virtual void OnButtonDown(Button button)
        {
        }

        /// <summary>
        ///     Invoked when a button is first released.
        /// </summary>
        /// <param name="button">The button that was released.</param>
        protected virtual void OnButtonUp(Button button)
        {
        }

        /// <summary>
        ///     Invoked when a valid character is produced from keyboard input.
        /// </summary>
        /// <param name="character">The produced character.</param>
        protected virtual void OnCharacterTyped(char character)
        {
        }
    }
}