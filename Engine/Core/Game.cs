using System.Diagnostics;

namespace Termule.Core;

/// <summary>
///     Central environment that manages <see cref="Component" />s and <see cref="System" />s.
///     Also controls the main game loop.
/// </summary>
public sealed class Game : IConfigurableGame
{
    private readonly List<GameElement> elements = [];
    private readonly Stopwatch stopwatch = new();

    private bool stop;

    /// <summary>
    ///     Gets the root game object.
    /// </summary>
    public GameObject Root { get; }

    /// <summary>
    ///     Gets the system manager.
    /// </summary>
    public SystemManager Systems { get; }

    /// <summary>
    ///     Gets the length of the last game loop iteration in seconds.
    /// </summary>
    public float DeltaTime { get; private set; }

    internal bool Started { get; private set; }

    private Game()
    {
        Root = [];
        Register(Root);

        Systems = new SystemManager();
        Register(Systems);
    }

    GameObject IConfigurableGame.Root => Root;

    IConfigurableSystemManager IConfigurableGame.Systems => Systems;

    void IConfigurableGame.Run()
    {
        Prepare();
#if RELEASE
        try
        {
#endif
        while (!stop)
        {
            RunFrame();
        }
#if RELEASE
        }
        finally
        {
#endif
        CleanUp();
#if RELEASE
        }
#endif
    }

    void IConfigurableGame.Prepare()
    {
        Prepare();
    }

    void IConfigurableGame.RunForFrames(int frames)
    {
        for (var i = 0; i < frames; i++)
        {
            RunFrame();
        }
    }

    void IConfigurableGame.CleanUp()
    {
        CleanUp();
    }

    /// <summary>
    ///     Creates a configurable <see cref="Game" /> instance.
    /// </summary>
    /// <returns>A new <see cref="Game" /> open for configuration.</returns>
    public static IConfigurableGame Create()
    {
        return new Game();
    }

    /// <summary>
    ///     Request this game to stop the game loop and clean up.
    /// </summary>
    public void Stop()
    {
        stop = true;
    }

    internal void Register(GameElement element)
    {
        element.SetGame(this);
        var id = (uint)elements.Count;

        elements.Add(element);
        element.InvokeRegistered(id);
    }

    internal void Unregister(GameElement element)
    {
        element.InvokeUnregistered();

        element.SetGame(null);
        elements.Remove(element);
    }

    private void Prepare()
    {
        Systems.Start();
        Started = true;
    }

    private void RunFrame()
    {
        DeltaTime = (float)stopwatch.Elapsed.TotalSeconds;
        stopwatch.Restart();

        Systems.Tick();
        Root.Tick();
    }

    private void CleanUp()
    {
        Systems.Stop();
    }
}