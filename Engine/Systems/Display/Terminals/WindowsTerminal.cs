using System.Runtime.InteropServices;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display system implementation for Windows.
/// </summary>
public sealed partial class WindowsTerminal : Terminal
{
    private readonly IntPtr handle = GetStdHandle(-10);
    private readonly INPUT_RECORD[] eventBuffer = new INPUT_RECORD[64];

    private uint initialMode;

    internal override string CollectInput()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        _ = GetConsoleMode(handle, out initialMode);

        uint mode = initialMode;
        mode |= 0x0080; // Enable extended flags
        mode &= ~0x0040u; // Disable quick edit mode
        mode |= 0x0010; // Enable mouse input

        _ = SetConsoleMode(handle, mode);
    }

    /// <inheritdoc />
    protected internal override void CleanUp()
    {
        base.CleanUp();

        _ = SetConsoleMode(handle, initialMode);
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct INPUT_RECORD
    {
        public readonly ushort EventType;
        public readonly EVENT Event;
    }

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct EVENT
    {
        [FieldOffset(0)]
        public readonly MOUSE_EVENT_RECORD MouseEvent;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct MOUSE_EVENT_RECORD
    {
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
        public readonly COORD dwMousePosition;
        public readonly uint dwButtonState;
        public readonly uint dwControlKeyState;
        public readonly uint dwEventFlags;
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter

        [StructLayout(LayoutKind.Sequential)]
        public readonly struct COORD
        {
            public readonly short X;
            public readonly short Y;
        }
    }
}
