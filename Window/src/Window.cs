using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;

static partial class Window
{
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FreeConsole();

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    //args[0] = engine process PID
    //args[1] = input pipe handle (from Game)
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
        StreamReader inReader = new StreamReader(inStream);

        while (true)
        {
            Console.Write(inReader.ReadLine());
        }
    }
}