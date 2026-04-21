// ReSharper disable InconsistentNaming

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display system implementation for Unix-like operating systems.
/// </summary>
public sealed partial class UnixDisplaySystem : TerminalDisplaySystem
{
    private const int F_GETFL = 3;
    private const int F_SETFL = 4;
    private const int STDIN_FILENO = 0;

    private static readonly ProcessStartInfo SttyStartInfo = new()
    {
        FileName = "stty",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    private readonly byte[] inputBuffer = new byte[1024];
    private readonly StringBuilder inputBuilder = new();
    private static readonly int O_NONBLOCK = OperatingSystem.IsMacOS() ? 0x0004 : 0x800;

    private string initialSttyConfig;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking

        // Configure stdin
        int flags = fcntl(STDIN_FILENO, F_GETFL, 0);
        _ = fcntl(STDIN_FILENO, F_SETFL, flags | O_NONBLOCK);

        // Configure stty
        SttyStartInfo.Arguments = "-g";
        using (Process getCurrentConfigProcess = Process.Start(SttyStartInfo))
        {
            initialSttyConfig = getCurrentConfigProcess?.StandardOutput.ReadLine();
        }

        SttyStartInfo.Arguments = "-echo -icanon min 0 time 0";
        Process.Start(SttyStartInfo)?.WaitForExit();
    }

    /// <inheritdoc />
    protected internal override void Stop()
    {
        base.Stop();

        Console.Write("\e[?1003l"); // Disable any-motion mouse tracking
        Console.Write("\e[?1006l"); // Disable SGR coordinates for mouse tracking

        // Reset stty config
        SttyStartInfo.Arguments = initialSttyConfig;
        Process.Start(SttyStartInfo)?.WaitForExit();
    }

    /// <inheritdoc />
    protected internal override void Tick()
    {
        // Get everything in STDIN
        inputBuilder.Clear();
        while (true)
        {
            int bytes = read(STDIN_FILENO, inputBuffer, inputBuffer.Length);
            if (bytes <= 0)
            {
                break;
            }

            inputBuilder.Append(Encoding.UTF8.GetChars(inputBuffer, 0, bytes));
        }

        // Parse out SGR events
        MatchCollection sgrEvents = sgrRegex().Matches(inputBuilder.ToString());
        if (sgrEvents.Count <= 0)
        {
            return;
        }

        Match lastSGREvent = sgrEvents[^1];
        MousePos =
        (
            int.Parse(lastSGREvent.Groups[1].Value) - 1,
            int.Parse(lastSGREvent.Groups[2].Value) - 1
        );
    }

    [LibraryImport("libc", SetLastError = true)]
    private static partial int fcntl(int fd, int cmd, int arg);

    [LibraryImport("libc", SetLastError = true)]
    private static partial int read(int fd, [Out] byte[] buf, int count);

    [GeneratedRegex(@"\x1b\[<\d+;(\d+);(\d+)[Mm]")]
    private static partial Regex sgrRegex();
}