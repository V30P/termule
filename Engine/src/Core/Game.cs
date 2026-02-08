namespace Termule.Core;

using global::System.Diagnostics;

/// <summary>
/// The primary class that contains and manages <see cref="Component"/>s and <see cref="System"/>s.
/// Also controls the main game loop.
/// </summary>
public sealed class Game : IConfigurableGame
{
    private readonly List<IHostedGameElement> elements = [];
    private readonly Stopwatch stopwatch = new();
    private bool stop = false;

    private Game()
    {
        this.Root = [];
        this.Register(this.Root);

        this.Systems = new SystemManager();
        this.Register(this.Systems);
    }

    /// <summary>
    /// Gets the root GameObject.
    /// </summary>
    public GameObject Root { get; }

    GameObject IConfigurableGame.Root => this.Root;

    /// <summary>
    /// Gets the SystemManager.
    /// </summary>
    public SystemManager Systems { get; }

    IConfigurableSystemManager IConfigurableGame.Systems => this.Systems;

    /// <summary>
    /// Gets the length of the last frame in seconds.
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    /// Creates a configurable instance of <see cref="Game"/>.
    /// </summary>
    /// <returns>A new <see cref="Game"/> open for configuration.</returns>
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

    /// <summary>
    /// Request the Game to halt the game loop and stop running.
    /// </summary>
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

/// <summary>
/// Provides access to <see cref="Game"/> methods used for configuration.
/// </summary>
public interface IConfigurableGame
{
    /// <summary>
    /// Gets the root GameObject.
    /// </summary>
    GameObject Root { get; }

    /// <summary>
    /// Gets the <see cref="SystemManager"/> in configurable form.
    /// </summary>
    IConfigurableSystemManager Systems { get; }

    /// <summary>
    /// Runs the Game.
    /// </summary>
    void Run();
}