using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Termule.Systems.Display;

public sealed partial class UnixDisplay : TerminalDisplay
{
    private const int O_NONBLOCK = 0x800;
    private const int F_GETFL = 3;
    private const int F_SETFL = 4;
    private static readonly int _stdinFd = 0;

    protected override void Start()
    {
        base.Start();

        // Configure stdin
        int flags = fcntl(_stdinFd, F_GETFL, 0);
        _ = fcntl(_stdinFd, F_SETFL, flags | O_NONBLOCK);

        // Configure the terminal driver
        Process.Start("stty", "-echo -icanon min 0 time 0").WaitForExit();
    }

    protected override void Stop()
    {
        base.Stop();
        Process.Start("stty", "sane").WaitForExit();
    }

    [LibraryImport("libc", SetLastError = true)]
    private static partial int fcntl(int fd, int cmd, int arg);

    [LibraryImport("libc", SetLastError = true)]
    private static partial int read(int fd, byte[] buf, int count);

    internal static string ReadAll()
    {
        byte[] buffer = new byte[1024];
        StringBuilder input = new();

        int bytesRead;
        while ((bytesRead = read(_stdinFd, buffer, buffer.Length)) > 0)
        {
            input.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        }

        return input.ToString();
    }

    internal override string CollectStandardInput()
    {
        byte[] buffer = new byte[1024];
        StringBuilder input = new();
        while (read(_stdinFd, buffer, buffer.Length) is int bytesRead and > 0)
        {
            input.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        }

        return input.ToString();
    }
}