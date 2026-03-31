// ReSharper disable InconsistentNaming

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display implementation for Unix-like operating systems.
/// </summary>
public sealed partial class UnixDisplay : TerminalDisplay
{
    private const int F_GETFL = 3;
    private const int F_SETFL = 4;
    private static readonly int O_NONBLOCK = OperatingSystem.IsMacOS() ? 0x0004 : 0x800;
    private static readonly int STDIN_FILENO = 0;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking

        // Configure stdin
        int flags = fcntl(STDIN_FILENO, F_GETFL, 0);
        _ = fcntl(STDIN_FILENO, F_SETFL, flags | O_NONBLOCK);

        // Configure the terminal driver
        Process.Start("stty", "-echo -icanon min 0 time 0")?.WaitForExit();
    }

    /// <summary>
    ///     Parses SGR events to determine mouse position.
    /// </summary>
    protected internal override void Tick()
    {
        // Get everything in STDIN
        byte[] buffer = new byte[1024];
        StringBuilder input = new();
        while (read(STDIN_FILENO, buffer, buffer.Length) is int bytesRead and > 0)
        {
            input.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
        }

        // Parse out the SGR events
        MatchCollection sgrEvents = sgrRegex().Matches(input.ToString());
        if (sgrEvents.Count > 0)
        {
            Match lastSGREvent = sgrEvents[^1];
            MousePos =
            (
                int.Parse(lastSGREvent.Groups[1].Value) - 1,
                int.Parse(lastSGREvent.Groups[2].Value) - 1
            );
        }
    }

    /// <inheritdoc />
    protected internal override void Stop()
    {
        base.Stop();

        Console.Write("\e[?1003l");
        Console.Write("\e[?1006l");

        Process.Start("stty", "sane")?.WaitForExit();
    }

    [LibraryImport("libc", SetLastError = true)]
    private static partial int fcntl(int fd, int cmd, int arg);

    [LibraryImport("libc", SetLastError = true)]
    private static partial int read(int fd, [Out] byte[] buf, int count);

    [GeneratedRegex(@"\x1b\[<\d+;(\d+);(\d+)[Mm]")]
    private static partial Regex sgrRegex();
}