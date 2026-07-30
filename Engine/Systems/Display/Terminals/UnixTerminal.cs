using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Terminal implementation for Unix-like operating systems.
/// </summary>
public sealed partial class UnixTerminal : Terminal
{
#pragma warning disable SA1310 // Field names should not contain underscore
    private const int F_GETFL = 3;
    private const int F_SETFL = 4;
    private const int STDIN_FILENO = 0;

    private static readonly int O_NONBLOCK = OperatingSystem.IsMacOS() ? 0x0004 : 0x800;
#pragma warning restore SA1310 // Field names should not contain underscore

    private static readonly ProcessStartInfo SttyStartInfo = new()
    {
        FileName = "stty",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    private readonly byte[] inputBuffer = new byte[1024];
    private readonly StringBuilder inputBuilder = new();

    private string initialSttyConfig;

    /// <inheritdoc />
    protected internal override void Start()
    {
        base.Start();

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
    protected internal override void CleanUp()
    {
        base.CleanUp();

        // Reset stty config
        SttyStartInfo.Arguments = initialSttyConfig;
        Process.Start(SttyStartInfo)?.WaitForExit();
    }

    private protected override string CollectInput()
    {
        _ = inputBuilder.Clear();
        while (true)
        {
            int bytes = read(STDIN_FILENO, inputBuffer, inputBuffer.Length);
            if (bytes <= 0)
            {
                break;
            }

            _ = inputBuilder.Append(Encoding.UTF8.GetChars(inputBuffer, 0, bytes));
        }

        return inputBuilder.ToString();
    }

    [LibraryImport("libc", SetLastError = true)]
    private static partial int fcntl(int fd, int cmd, int arg);

    [LibraryImport("libc", SetLastError = true)]
    private static partial int read(int fd, [Out] byte[] buf, int count);
}
