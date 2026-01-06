using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Termule.Rendering;

public static partial class Display
{
    public static VectorInt Size { get; private set; }
    public static VectorInt MousePos { get; private set; }

    private static Content _state;

    private static readonly Dictionary<Color?, int> _backgroundColorCodes = new()
    {
        [Color.Default] = 49,
        [Color.Black] = 40,
        [Color.Red] = 41,
        [Color.Green] = 42,
        [Color.Yellow] = 43,
        [Color.Blue] = 44,
        [Color.Magenta] = 45,
        [Color.Cyan] = 46,
        [Color.White] = 47,
        [Color.BrightBlack] = 100,
        [Color.BrightRed] = 101,
        [Color.BrightGreen] = 102,
        [Color.BrightYellow] = 103,
        [Color.BrightBlue] = 104,
        [Color.BrightMagenta] = 105,
        [Color.BrightCyan] = 106,
        [Color.BrightWhite] = 107
    };

    private static readonly Dictionary<Color?, int> _foregroundColorCodes = new()
    {
        [Color.Default] = 39,
        [Color.Black] = 30,
        [Color.Red] = 31,
        [Color.Green] = 32,
        [Color.Yellow] = 33,
        [Color.Blue] = 34,
        [Color.Magenta] = 35,
        [Color.Cyan] = 36,
        [Color.White] = 37,
        [Color.BrightBlack] = 90,
        [Color.BrightRed] = 91,
        [Color.BrightGreen] = 92,
        [Color.BrightYellow] = 93,
        [Color.BrightBlue] = 94,
        [Color.BrightMagenta] = 95,
        [Color.BrightCyan] = 96,
        [Color.BrightWhite] = 97
    };

    static Display()
    {
        Console.CursorVisible = false;

        Console.Write("\x1b[?1049h"); // Alternate buffer
        Console.Write("\x1b[?1003h"); // Any-motion mouse tracking
        Console.Write("\x1b[?1006h"); // SGR coordinates for mouse tracking

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Configure the terminal driver
            Process.Start("stty", "-echo -icanon min 0 time 0").WaitForExit();
        }

        Game.Stopped += Reset;
    }

    private static void Reset()
    {
        Console.ResetColor();
        Console.CursorVisible = true;

        Console.Write("\x1b[?1049l");
        Console.Write("\x1b[?1003l");
        Console.Write("\x1b[?1006l");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Reset terminal driver configuration
            Process.Start("stty", "sane").WaitForExit();
        }
    }

    internal static void Draw(Content content)
    {
        if
        (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth || Console.BufferHeight != Console.WindowHeight
            || content.Size != _state?.Size
            || Console.WindowWidth != Size.X || Console.WindowHeight != Size.Y
        )
        {
            Console.ResetColor();
            Console.Clear();

            Size = (Console.WindowWidth, Console.WindowHeight);
            _state = null;
        }

        StringBuilder output = new();
        for (int x = 0; x < content.Size.X; x++)
        {
            for (int y = 0; y < content.Size.Y; y++)
            {
                if (_state?.EqualsAt(content, (x, y)) == true)
                {
                    continue;
                }

                Cell cell = content.At(x, y);
                output.Append
                (
                    $"\x1b[{y + 1};{x + 1}H" + // Go to the position
                    $"\x1b[{GetBackgroundColorCode(cell.Color)}m" + // Apply the background color 
                    $"\x1b[{GetForegroundColorCode(cell.CharColor)}m" + // Apply the foreground color 
                    (cell.Char != default(char) ? cell.Char : ' ') // Write the character 
                );
            }
        }

        Console.Write(output);
        _state = content;
    }

    private static int GetBackgroundColorCode(Color? color)
    {
        return color != null ? _backgroundColorCodes[color] : 49;
    }

    private static int GetForegroundColorCode(Color? color)
    {
        return color != null ? _foregroundColorCodes[color] : 39;
    }

    internal static void ParseStandardInput()
    {
        // Get mouse position from the most recent SGR event
        MatchCollection sgrEvents = sgrRegex().Matches(StdinReader.ReadAll().ToString());
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

    [GeneratedRegex(@"\x1b\[<\d+;(\d+);(\d+)[Mm]")]
    private static partial Regex sgrRegex();

    // ! This should be used for linux only
    internal static partial class StdinReader
    {
        private const int O_NONBLOCK = 0x800;
        private const int F_GETFL = 3;
        private const int F_SETFL = 4;
        private static readonly int _stdinFd = 0;

        [LibraryImport("libc", SetLastError = true)]
        private static partial int fcntl(int fd, int cmd, int arg);

        [LibraryImport("libc", SetLastError = true)]
        private static partial int read(int fd, byte[] buf, int count);

        static StdinReader()
        {
            int flags = fcntl(_stdinFd, F_GETFL, 0);
            _ = fcntl(_stdinFd, F_SETFL, flags | O_NONBLOCK);
        }

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
    }
}
