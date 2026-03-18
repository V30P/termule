namespace Termule.Core;

using global::System.Diagnostics;

/// <summary>
/// Central environment that manages <see cref="Component"/>s and <see cref="System"/>s.
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
    /// Gets the root game object.
    /// </summary>
    public GameObject Root { get; }

    GameObject IConfigurableGame.Root => this.Root;

    /// <summary>
    /// Gets the system manager.
    /// </summary>
    public SystemManager Systems { get; }

    IConfigurableSystemManager IConfigurableGame.Systems => this.Systems;

    /// <summary>
    /// Gets the length of the last game loop iteration in seconds.
    /// </summary>
    public float DeltaTime { get; private set; }

    internal bool Started { get; private set; }

    /// <summary>
    /// Creates a configurable <see cref="Game"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Game"/> open for configuration.</returns>
    public static IConfigurableGame Create()
    {
        return new Game();
    }

    void IConfigurableGame.Run()
    {
        this.Prepare();
#if RELEASE
        try
        {
#endif
        while (!this.stop)
        {
            this.RunFrame();
        }
#if RELEASE
        }
        finally
        {
#endif
        this.CleanUp();
#if RELEASE
        }
#endif
    }

    void IConfigurableGame.Prepare()
    {
        this.Prepare();
    }

    void IConfigurableGame.RunForFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            this.RunFrame();
        }
    }

    void IConfigurableGame.CleanUp()
    {
        this.CleanUp();
    }

    /// <summary>
    /// Request this game to stop the game loop and clean up.
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

    private void Prepare()
    {
        ((IHostedSystemManager)this.Systems).Start();

        this.Started = true;
    }

    private void RunFrame()
    {
        this.DeltaTime = (float)this.stopwatch.Elapsed.TotalSeconds;
        this.stopwatch.Restart();

        ((IHostedSystemManager)this.Systems).Tick();
        ((IHostedComponent)this.Root).Tick();
    }

    private void CleanUp()
    {
        ((IHostedSystemManager)this.Systems).Stop();
    }
}

/// <summary>
/// Provides access to <see cref="Game"/> methods used for configuration.
/// </summary>
public interface IConfigurableGame
{
    /// <summary>
    /// Gets the root game object.
    /// </summary>
    GameObject Root { get; }

    /// <summary>
    /// Gets the <see cref="SystemManager"/> in configurable form.
    /// </summary>
    IConfigurableSystemManager Systems { get; }

    /// <summary>
    /// Runs the game.
    /// </summary>
    void Run();

    internal void Prepare();

    internal void RunForFrames(int frames);

    internal void CleanUp();
}