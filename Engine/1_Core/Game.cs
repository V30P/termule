using Termule.Rendering;
using System.Reflection;
using System.Diagnostics;

namespace Termule;

public class Game
{
    readonly CancellationTokenSource cancellationTokenSource;
    public readonly CancellationToken cancellationToken;

    public readonly GameObject root;

    public readonly Logger logger = new Logger();
    internal readonly Window window = new Window(Window.ReadMode.NewlineTerminated);
    internal readonly RenderSystem renderSystem = new RenderSystem();

    public float deltaTime { get; private set; }

    public Game(string assemblyLocation)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;

        root = Component.Spawn<GameObject>(this, "Root");
        GameLoop();

        //Try to find and execute the entry point in the game assembly
        //Entry point format: Start(Game)
        foreach (Type type in Assembly.LoadFrom(assemblyLocation).GetTypes())
        {
            if (type.GetMethod("Start", BindingFlags.Public | BindingFlags.Static) is MethodInfo startMethod)
            {
                ParameterInfo[] parameters = startMethod.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Game))
                {
                    startMethod?.Invoke(null, [this]);
                    break;
                }
            }
        }
    }

    async void GameLoop()
    {
        Stopwatch frameStopWatch = new Stopwatch();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
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