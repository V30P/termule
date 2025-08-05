using System.Reflection;
using System.Diagnostics;
using Termule.Rendering;

namespace Termule;

public sealed class Game : IComposite
{
    public Game game => this;
    public Dictionary<string, Component> components => _components;
    readonly Dictionary<string, Component> _components = [];

    readonly CancellationTokenSource cancellationTokenSource;
    public readonly CancellationToken cancellationToken;

    public float deltaTime { get; private set; }

    public Game(string assemblyLocation)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;

        this.Add(new Logger());
        this.Add(new RenderSystem());

        // Try to execute a found entry point (if any) in the game assembly
        // FORMAT: public static void Start(Game)
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

        Task.Run(GameLoop);
    }

    void GameLoop()
    {
        Stopwatch frameStopWatch = new Stopwatch();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (Component component in this.ToArray())
            {
                component.Tick();
            }

            deltaTime = (float) frameStopWatch.Elapsed.TotalSeconds;
            frameStopWatch.Restart();
        }

        foreach (Component component in this)
        {
            component.Destroy();
        }
    }

    public void Stop() => cancellationTokenSource.Cancel();
}