namespace Termule.Core;

using global::System.Diagnostics;

public sealed class Game : IConfigurableGame
{
    private readonly List<IHostedGameElement> elements = [];
    private readonly Stopwatch stopwatch = new();
    private bool stop = false;

    public Game()
    {
        this.Root = [];
        this.Register(this.Root);

        this.Systems = new SystemManager();
        this.Register(this.Systems);
    }

    public GameObject Root { get; }

    GameObject IConfigurableGame.Root => this.Root;

    public SystemManager Systems { get; }

    IConfigurableSystemManager IConfigurableGame.Systems => this.Systems;

    public float DeltaTime { get; private set; }

    public static IConfigurableGame Create()
    {
        return new Game();
    }

    void IConfigurableGame.Run()
    {
        IHostedSystemManager systems = this.Systems;
        IHostedComponent root = this.Root;

        systems.Start();
#if RELEASE
        try
        {
#endif

        while (!this.stop)
        {
            this.DeltaTime = (float)this.stopwatch.Elapsed.TotalSeconds;
            this.stopwatch.Restart();

            systems.Update();
            root.Tick();
        }
#if RELEASE
        }
        finally
        {
#endif
        systems.Stop();
#if RELEASE
        }
#endif
    }

    public void Stop()
    {
        this.stop = true;
    }

    internal void Register(IHostedGameElement element)
    {
        element.Game = this;
        uint id = (uint)this.elements.Count;

        this.elements.Add(element);
        element.InvokeRegistered(id);
    }

    internal void Unregister(IHostedGameElement element)
    {
        element.InvokeUnregistered();

        element.Game = null;
        this.elements.Remove(element);
    }
}

public interface IConfigurableGame
{
    GameObject Root { get; }

    IConfigurableSystemManager Systems { get; }

    void Run();
}