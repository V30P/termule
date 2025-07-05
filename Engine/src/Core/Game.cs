using Termule.Rendering;
using System.Reflection;
using System.Diagnostics;

namespace Termule;

public class Game
{
    const BindingFlags entryMethodFlags = BindingFlags.Static | BindingFlags.NonPublic;

    readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    public readonly CancellationToken cancellationToken;

    public readonly Logger logger;
    internal readonly Window window;
    internal readonly RenderSystem renderSystem;
    internal readonly GameObject root;

    public float deltaTime { get; private set; }

    public Game(string assemblyLocation)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;
        
        logger = new Logger();

        //Set up rendering to a window
        window = new Window(Window.ReadMode.NewlineTerminated);
        window.Closed += Stop;
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
                await window.writer.WriteLineAsync(frame.ToString().AsMemory(), cancellationToken);

                //Record deltaTime
                deltaTime = (float) frameStopWatch.Elapsed.TotalSeconds;
                frameStopWatch.Restart();
            }
        }
        catch (OperationCanceledException) { }

        //Clean up
        window.Close();
    }

    public void Stop() => cancellationTokenSource.Cancel();
}