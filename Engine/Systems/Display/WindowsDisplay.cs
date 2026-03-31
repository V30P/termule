using System.Runtime.InteropServices;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display implementation for Windows.
/// </summary>
public sealed partial class WindowsDisplay : TerminalDisplay
{
    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);
        _ = GetConsoleMode(handle, out uint mode);
        mode &= ~(ENABLE_ECHO_INPUT | ENABLE_LINE_INPUT);
        SetConsoleMode(handle, mode);
    }

    /// <inheritdoc />
    protected internal override void Stop()
    {
        base.Stop();

        IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        mode |= ENABLE_ECHO_INPUT | ENABLE_LINE_INPUT;
        _ = SetConsoleMode(handle, mode);
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    // ReSharper disable InconsistentNaming
    private const int STD_INPUT_HANDLE = -10;
    private const uint ENABLE_ECHO_INPUT = 0x0004;

    private const uint ENABLE_LINE_INPUT = 0x0002;
    // ReSharper restore InconsistentNaming
}