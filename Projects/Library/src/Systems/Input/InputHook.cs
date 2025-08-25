using SharpHook;
using SharpHook.Data;

namespace Termule.Input;

internal static class InputHook
{
    static readonly TaskPoolGlobalHook sharpHook;

    static readonly HashSet<KeyCode> pressedKeys = [];

    internal static event Action<Button> ButtonDown;
    internal static event Action<Button> ButtonUp;

    static InputHook()
    {
        sharpHook = new TaskPoolGlobalHook(runAsyncOnBackgroundThread: true);

        sharpHook.KeyPressed += OnKeyPressed;
        sharpHook.KeyReleased += OnKeyReleased;     

        sharpHook.RunAsync();
    }

    static void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        if (pressedKeys.Add(e.Data.KeyCode))
        {
            ButtonDown?.Invoke(e.Data.KeyCode.ToButton());
        }
    }

    static void OnKeyReleased(object sender, KeyboardHookEventArgs e)
    {
        pressedKeys.Remove(e.Data.KeyCode);
        ButtonUp?.Invoke(e.Data.KeyCode.ToButton());
    }
}