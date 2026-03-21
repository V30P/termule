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

        var handle = GetStdHandle(StdInputHandle);
        _ = GetConsoleMode(handle, out var mode);
        mode &= ~(EnableEchoInput | EnableLineInput);
        SetConsoleMode(handle, mode);
    }

    /// <inheritdoc />
    protected internal override void Stop()
    {
        base.Stop();

        var handle = GetStdHandle(StdInputHandle);
        GetConsoleMode(handle, out var mode);
        mode |= EnableEchoInput | EnableLineInput;
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

#pragma warning disable SA1310 // Field names should not contain underscore
    private const int StdInputHandle = -10;
    private const uint EnableEchoInput = 0x0004;
    private const uint EnableLineInput = 0x0002;
#pragma warning restore SA1310 // Field names should not contain underscore
}