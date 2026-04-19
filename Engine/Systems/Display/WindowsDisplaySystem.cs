using System.Runtime.InteropServices;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display system implementation for Windows.
/// </summary>
public sealed partial class WindowsDisplaySystem : TerminalDisplaySystem
{
    private readonly IntPtr handle = GetStdHandle(-10);
    private readonly INPUT_RECORD[] eventBuffer = new INPUT_RECORD[64];

    private uint initialMode;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        _ = GetConsoleMode(handle, out initialMode);

        uint mode = initialMode;
        mode |= 0x0080; // Enable extended flags
        mode &= ~0x0040u; // Disable quick edit mode
        mode |= 0x0010; // Enable mouse input

        SetConsoleMode(handle, mode);
    }

    /// <inheritdoc />
    protected internal override void Stop()
    {
        base.Stop();

        _ = SetConsoleMode(handle, initialMode);
    }

    /// <inheritdoc />
    protected internal override void Tick()
    {
        if (!GetNumberOfConsoleInputEvents(handle, out uint numEvents) || numEvents == 0)
        {
            return;
        }

        _ = ReadConsoleInput(handle, eventBuffer, (uint)eventBuffer.Length, out uint eventsCount);
        for (int i = (int)eventsCount - 1; i >= 0; i--)
        {
            // ReSharper disable InconsistentNaming
            const uint MOUSE_EVENT = 0x0002;
            const uint MOUSE_MOVED = 0x0001;
            // ReSharper restore InconsistentNaming

            if (eventBuffer[i].EventType == MOUSE_EVENT &&
                (eventBuffer[i].Event.MouseEvent.dwEventFlags & MOUSE_MOVED) != 0)
            {
                MOUSE_EVENT_RECORD mouseEvent = eventBuffer[i].Event.MouseEvent;
                MousePos = (mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);

                return;
            }
        }
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
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetNumberOfConsoleInputEvents(IntPtr hConsoleHandle, out uint lpcNumberOfEvents);

    [LibraryImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ReadConsoleInput(
        IntPtr hConsoleInput,
        [Out] INPUT_RECORD[] lpBuffer,
        uint nLength,
        out uint lpNumberOfEventsRead);

    // ReSharper disable InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct INPUT_RECORD
    {
        public readonly ushort EventType;
        public readonly EVENT Event;
    }

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct EVENT
    {
        [FieldOffset(0)] public readonly MOUSE_EVENT_RECORD MouseEvent;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct MOUSE_EVENT_RECORD
    {
        public readonly COORD dwMousePosition;
        public readonly uint dwButtonState;
        public readonly uint dwControlKeyState;
        public readonly uint dwEventFlags;

        [StructLayout(LayoutKind.Sequential)]
        public readonly struct COORD
        {
            public readonly short X, Y;
        }
    }
    // ReSharper restore InconsistentNaming
}