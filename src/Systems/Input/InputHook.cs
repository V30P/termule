using SharpHook;
using SharpHook.Data;

namespace Termule.Input;

internal static class InputHook
{
    private static readonly TaskPoolGlobalHook _sharpHook;

    private static readonly HashSet<MouseButton> _pressedMouseButtons = [];
    private static readonly HashSet<KeyCode> _pressedKeys = [];

    internal static event Action<Button> ButtonDown;
    internal static event Action<Button> ButtonUp;
    internal static event Action<char> CharacterTyped;

    static InputHook()
    {
        _sharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);

        _sharpHook.MousePressed += OnMousePressed;
        _sharpHook.MouseReleased += OnMouseReleased;

        _sharpHook.KeyPressed += OnKeyPressed;
        _sharpHook.KeyReleased += OnKeyReleased;
        _sharpHook.KeyTyped += OnKeyTyped;

        _sharpHook.RunAsync();
    }

    private static void OnKeyPressed(object _, KeyboardHookEventArgs e)
    {
        if (_pressedKeys.Add(e.Data.KeyCode))
        {
            ButtonDown?.Invoke(e.Data.KeyCode.ToButton());
        }
    }

    private static void OnKeyReleased(object _, KeyboardHookEventArgs e)
    {
        _pressedKeys.Remove(e.Data.KeyCode);
        ButtonUp?.Invoke(e.Data.KeyCode.ToButton());
    }

    private static void OnMousePressed(object _, MouseHookEventArgs e)
    {
        if (_pressedMouseButtons.Add(e.Data.Button))
        {
            ButtonDown?.Invoke(e.Data.Button.ToButton());
        }
    }

    private static void OnMouseReleased(object _, MouseHookEventArgs e)
    {
        _pressedMouseButtons.Remove(e.Data.Button);
    }

    private static void OnKeyTyped(object sender, KeyboardHookEventArgs e)
    {
        CharacterTyped.Invoke(e.Data.KeyChar);
    }
}