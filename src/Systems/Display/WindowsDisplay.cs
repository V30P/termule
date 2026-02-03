namespace Termule.Systems.Display;

using System.Runtime.InteropServices;

public sealed partial class WindowsDisplay : TerminalDisplay
{
#pragma warning disable SA1310 // Field names should not contain underscore
    private const int STD_INPUT_HANDLE = -10;
    private const uint ENABLE_ECHO_INPUT = 0x0004;
    private const uint ENABLE_LINE_INPUT = 0x0002;
#pragma warning restore SA1310 // Field names should not contain underscore

    protected override void Start()
    {
        base.Start();

        IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        mode &= ~(ENABLE_ECHO_INPUT | ENABLE_LINE_INPUT);
        SetConsoleMode(handle, mode);
    }

    protected override void Stop()
    {
        base.Stop();

        IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        mode |= ENABLE_ECHO_INPUT | ENABLE_LINE_INPUT;
        SetConsoleMode(handle, mode);
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
}
