using System.Runtime.InteropServices;
using System.Text;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Terminal implementation for Windows.
/// </summary>
public sealed partial class WindowsTerminal : Terminal
{
    private readonly IntPtr handle = GetStdHandle(-10);
    private readonly byte[] buffer = new byte[4096];
    private readonly StringBuilder inputBuilder = new();

    private uint initialMode;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        _ = GetConsoleMode(handle, out initialMode);

        uint mode = initialMode;
        mode |= 0x0080u // Enable extended flags (needed for some of the other flags)
            | 0x0200u; // Enable virtual terminal input (sending input as escape sequences)
        mode &= ~(
            0x0001u // Disable processed input
            | 0x0002u // Disable line input
            | 0x0004u // Disable echo input
            | 0x0040u // Disable quick edit mode (mouse selection)
        );

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
        _ = inputBuilder.Clear();

        // We have to use ReadFile() here because the normal Console.Read() does some internal
        // buffering/parsing that can break up escape sequences
        if (WaitForSingleObject(handle, 0) == 0
            && ReadFile(handle, buffer, (uint) buffer.Length, out uint count, IntPtr.Zero))
        {
            for (int i = 0; i < count; i++)
            {
                _ = inputBuilder.Append((char) buffer[i]);
            }
        }

        return inputBuilder.ToString();
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ReadFile(
        IntPtr hFile,
        [Out] byte[] lpBuffer,
        uint nNumberOfBytesToRead,
        out uint lpNumberOfBytesRead,
        IntPtr lpOverlapped
    );
}
