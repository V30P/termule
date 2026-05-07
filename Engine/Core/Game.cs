using System.Diagnostics;
using Termule.Engine.Core.Messaging;

namespace Termule.Engine.Core;

/// <summary>
///     Central environment that manages <see cref="Component" />s and <see cref="System" />s.
///     Also controls the main game loop.
/// </summary>
public sealed class Game
{
    private readonly Stopwatch stopwatch = new();

    private bool stop;

    internal uint registerCount;

    /// <summary>
    ///     Gets the system manager.
    /// </summary>
    public SystemManager Systems { get; } = new();

    /// <summary>
    ///     Gets the root game object.
    /// </summary>
    public GameObject Root { get; } = [];

    /// <summary>
    ///     Gets the global message bus.
    /// </summary>
    public MessageBus Bus { get; } = new();

    /// <summary>
    ///     Gets the length of the last game loop iteration in seconds.
    /// </summary>
    public float DeltaTime { get; private set; }

    internal bool Started { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Game" /> class.
    /// </summary>
    public Game()
    {
        Register(Root);
        Register(Systems);
        Register(Bus);
    }

    /// <summary>
    ///     Run the game, blocking until it is stopped.
    /// </summary>
    public void Run()
    {
        if (!Started)
        {
            Start();
        }

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

    /// <summary>
    ///     Request this game to stop the game loop and clean up.
    /// </summary>
    public void Stop()
    {
        stop = true;
    }

    internal void Register(GameElement element)
    {
        element.ElementId = registerCount++;
        element.SetGame(this);
    }

    internal void Unregister(GameElement element)
    {
        element.SetGame(null);
    }

    // Manual lifecycle control
    internal void Start()
    {
        if (Started)
        {
            return;
        }

        Systems.Start();
        Started = true;
    }

    internal void RunFrame()
    {
        DeltaTime = (float)stopwatch.Elapsed.TotalSeconds;
        stopwatch.Restart();

        Systems.Tick();
        Root.Tick();
    }

    internal void RunForFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            RunFrame();
        }
    }

    internal void CleanUp()
    {
        if (!Started)
        {
            return;
        }

        Systems.Stop();
        Started = false;
    }
}
