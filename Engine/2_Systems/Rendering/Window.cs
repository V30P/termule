using System.IO.Pipes;
using System.Diagnostics;
using System.Reflection;

namespace Termule;

public class Window : IDisposable
{
    public enum ReadMode
    {
        Standard, // The window displays every character as soon as it is read
        NewlineTerminated // The window will wait until it hits a '\0' to display 
    }

    static readonly string exePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\TermuleWindow.exe";

    readonly Process process;
    public readonly StreamWriter writer;

    bool hasClosed;
    public event Action Closed;

    public Window(ReadMode readMode)
    {
        AnonymousPipeServerStream toWindowStream = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
        writer = new StreamWriter(toWindowStream)
        {
            AutoFlush = true
        };

        process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = $"{Environment.ProcessId} {toWindowStream.GetClientHandleAsString()} {readMode}", // See WindowProcess.cs
            },
            EnableRaisingEvents = true
        };
        process.Start();

        process.Exited += (_, _) => Close();
    }

    public static ReadMode GetReadMode(string name)
    {
        foreach (ReadMode mode in Enum.GetValues<ReadMode>())
        {
            if (mode.ToString() == name)
            {
                return mode;
            }
        }

        return ReadMode.Standard;
    }

    public void Close()
    {
        if (!hasClosed)
        {
            process.Kill();
            Dispose();

            hasClosed = true;
            Closed?.Invoke();
        }
    }

    public void Dispose()
    {
        // The writer might still be in use, so keep trying to close it until it actually closes
        bool writerClosed = false;
        while (!writerClosed)
        {
            try
            {
                writer.Close();
                writerClosed = true;
            }
            catch (InvalidOperationException) { }

            Thread.Sleep(1);
        }

        process.Dispose();
        GC.SuppressFinalize(this);
    }
}