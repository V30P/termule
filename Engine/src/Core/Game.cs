using System.Diagnostics;
using System.IO.Pipes;
using Termule.Rendering;
using System.Reflection;

namespace Termule;

public class Game
{
    const BindingFlags entryMethodFlags = BindingFlags.Static | BindingFlags.NonPublic;

    readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    public CancellationToken cancellationToken;

    internal readonly StreamWriter windowWriter;
    internal readonly RenderSystem renderSystem;
    internal readonly GameObject root;

    public float deltaTime { get; private set; }

    public Game(string assemblyLocation)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;

        //Set up the window
        AnonymousPipeServerStream toWindowStream = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
        windowWriter = new StreamWriter(toWindowStream)
        {
            AutoFlush = true
        };

        Process windowProcess = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "C:/Users/NHenn/Projects/Termule/Window/bin/Debug/net9.0/TermuleWindow.exe",
                Arguments = $"{Environment.ProcessId} {toWindowStream.GetClientHandleAsString()}"
            },
            EnableRaisingEvents = true
        };
        windowProcess.Start();

        AppDomain.CurrentDomain.ProcessExit += (object _, EventArgs _) => Stop();
        windowProcess.Exited += (_, _) => Stop();

        //Create the renderSysten
        renderSystem = new RenderSystem();

        //Create the root GameObject and start the game loop
        root = Component.Spawn<GameObject>(this, "Root");
        GameLoop();

        //Start the game assembly
        Assembly gameAssembly = Assembly.LoadFrom(assemblyLocation);
        MethodInfo entryMethod = gameAssembly.GetTypes().Select(x => x.GetMethod("Entry", entryMethodFlags)).FirstOrDefault();
        entryMethod.Invoke(null, [this]);
    }

    async void GameLoop()
    {
        Stopwatch frameStopWatch = new Stopwatch();

        try
        {
            while (true)
            {
                //Update GameObjects
                root.Update();

                //Render to the window
                Frame frame = renderSystem.GetFrame();
                await windowWriter.WriteLineAsync(frame.ToString().AsMemory(), cancellationToken);

                //Update frame time measurement
                deltaTime = frameStopWatch.ElapsedMilliseconds / 1000f;
                frameStopWatch.Restart();
            }
        }
        catch (OperationCanceledException) { }

        //Clean up
        windowWriter.Dispose();
    }

    public void Stop() => cancellationTokenSource.Cancel();
}