using SharpHook;
using SharpHook.Data;

namespace Termule.Input;

internal static class InputHook
{
    private static readonly TaskPoolGlobalHook _sharpHook;

    private static readonly HashSet<KeyCode> _pressedKeys = [];

    internal static event Action<Button> ButtonDown;
    internal static event Action<Button> ButtonUp;

    static InputHook()
    {
        _sharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);

        _sharpHook.KeyPressed += OnKeyPressed;
        _sharpHook.KeyReleased += OnKeyReleased;

        _sharpHook.RunAsync();
    }

    private static void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        if (_pressedKeys.Add(e.Data.KeyCode))
        {
            ButtonDown?.Invoke(e.Data.KeyCode.ToButton());
        }
    }

    private static void OnKeyReleased(object sender, KeyboardHookEventArgs e)
    {
        _pressedKeys.Remove(e.Data.KeyCode);
        ButtonUp?.Invoke(e.Data.KeyCode.ToButton());
    }
}