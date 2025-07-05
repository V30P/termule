using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;

using Termule;

static partial class WindowProcess
{
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FreeConsole();

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    static StreamReader inReader = StreamReader.Null;

    static readonly Dictionary<Window.ReadMode, Func<string>> readFuncs = new Dictionary<Window.ReadMode, Func<string>>
    {
        { Window.ReadMode.Standard, () => ((char) inReader.Read()).ToString() },
        { Window.ReadMode.NewlineTerminated, () => inReader.ReadLine() }
    };

    /*  
    args[0]: engine process PID
    args[1]: input pipe handle (from Game)
    args[2]: readMode
    */
    static void Main(string[] args)
    {
        FreeConsole();
        AllocConsole();
        Console.CursorVisible = false;

        //Close this window if the engine process exits
        Process engineProcess = Process.GetProcessById(int.Parse(args[0]));
        engineProcess.EnableRaisingEvents = true;
        engineProcess.Exited += (_, _) => Environment.Exit(0);

        AnonymousPipeClientStream inStream = new AnonymousPipeClientStream(PipeDirection.In, args[1]);
        inReader = new StreamReader(inStream);

        //Read from the input pipe and write to the console until the process exits
        Func<string> readFunc = readFuncs[Window.GetReadMode(args[2])];
        while (true)
        {
            Console.Write(readFunc.Invoke());
        }
    }
}