using System.Runtime.InteropServices;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Terminal implementation for Windows.
/// </summary>
public sealed partial class WindowsTerminal : Terminal
{
    private readonly IntPtr handle = GetStdHandle(-10);

    private uint initialMode;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        _ = GetConsoleMode(handle, out initialMode);

        uint mode = initialMode;
        mode |= 0x0080; // Enable extended flags
        mode &= ~0x0040u; // Disable quick edit mode

        _ = SetConsoleMode(handle, mode);
    }

    /// <inheritdoc />
    protected internal override void CleanUp()
    {
        base.CleanUp();

        _ = SetConsoleMode(handle, initialMode);
    }

    private protected override string CollectInput()
    {
        // TODO: Implement
        throw new NotImplementedException();
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
