using SharpHook;

namespace Termule.Engine.Systems.Input;

/// <summary>
///     System for collecting keyboard input according to a <see cref="BindMap" />.
/// </summary>
public sealed class Keyboard : Core.System
{
    private static readonly TaskPoolGlobalHook SharpHook;

    private readonly HashSet<Button> pressedButtons = [];

    internal event Action<Button> ButtonDown;
    internal event Action<Button> ButtonUp;
    internal event Action<char> CharacterTyped;

    /// <summary>
    ///     Sets the bind map that this keyboard should use.
    /// </summary>
    public BindMap Binds
    {
        get;

        set
        {
            field = value ?? throw new ArgumentNullException(nameof(Binds));
            field.Keyboard = this;
        }
    } = [];

    static Keyboard()
    {
        SharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);
        SharpHook.RunAsync();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Keyboard" /> class.
    /// </summary>
    internal Keyboard()
    {
        SharpHook.MousePressed += (_, e) => OnButtonPressed(e.Data.Button.ToButton());
        SharpHook.MouseReleased += (_, e) => OnButtonReleased(e.Data.Button.ToButton());

        SharpHook.KeyPressed += (_, e) => OnButtonPressed(e.Data.KeyCode.ToButton());
        SharpHook.KeyReleased += (_, e) => OnButtonReleased(e.Data.KeyCode.ToButton());
        SharpHook.KeyTyped += (_, e) => CharacterTyped?.Invoke(e.Data.KeyChar);
    }

    /// <summary>
    ///     Gets the value of the bind with given <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of value to get.</typeparam>
    /// <param name="name">The name of the bind to get the value for.</param>
    /// <returns> The value of the specified bind. </returns>
    public TValue Get<TValue>(string name)
    {
        if (!Binds.TryGetValue(name, out object value))
        {
            throw new ArgumentException($"No value exists for '{name}'.");
        }

        return value is not TValue typedValue
            ? throw new ArgumentException($"A bind named '{name}' exists, but it is not of type '{typeof(TValue)}'.")
            : typedValue;
    }


    /// <inheritdoc />
    protected internal override void Tick()
    {
        Binds.PollValues();
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
}