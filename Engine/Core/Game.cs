using System.Diagnostics;
using Termule.Engine.Core.Messaging;

namespace Termule.Engine.Core;

/// <summary>
///     Central environment that manages <see cref="Component" />s and <see cref="System" />s.
///     Also controls the main game loop.
/// </summary>
public sealed class Game
{
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
    ///     Gets the system manager.
    /// </summary>
    public readonly SystemManager Systems = new();

    /// <summary>
    ///     Gets the root game object.
    /// </summary>
    public readonly GameObject Root = [];

    /// <summary>
    ///     Gets the global message bus.
    /// </summary>
    public readonly MessageBus Bus = new();

    private readonly HashSet<GameElement> elements = [];
    private readonly Stopwatch stopwatch = new();

    private bool stop;
    private uint registerCount;

    /// <summary>
    ///     Gets the length of the last game loop iteration in seconds.
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    ///     Gets or sets a value indicating how many ticks the game should aim to perform per
    ///     second.
    /// </summary>
    public int TargetTps
    {
        get;

        set
        {
            if (field <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(TargetTps),
                    "Target TPS must be greater than 0."
                );
            }

            field = value;
        }
    } = int.MaxValue;

    internal bool Started { get; private set; }

    /// <summary>
    ///     Run the game, blocking until it is stopped.
    /// </summary>
    public void Run()
    {
        Start();

#if RELEASE
        try
        {
#endif
        while (!stop)
        {
            RunTick();
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
        if (stop)
        {
            return;
        }

        stop = true;
        Bus.Broadcast(new StoppedMessage());
    }

    internal void Start()
    {
        if (Started)
        {
            return;
        }

        Systems.Start();
        Started = true;
        Bus.Broadcast(new StartedMessage());
    }

    internal void RunTick()
    {
        stopwatch.Restart();

        Systems.Tick();
        Root.Tick();

        // Cap tickrate
        int requiredDelay = (int) ((float) 1 / TargetTps * 1000)
                            - (int) stopwatch.ElapsedMilliseconds;
        if (requiredDelay > 0)
        {
            Thread.Sleep(requiredDelay);
        }

        DeltaTime = (float) stopwatch.Elapsed.TotalSeconds;
    }

    internal void RunTicks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RunTick();
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

    internal void Register(GameElement element)
    {
        if (elements.Add(element))
        {
            element.ElementId = registerCount++;
            element.SetGame(this);

            Bus.Broadcast(new ElementRegisteredMessage(element));
        }
    }

    internal void Unregister(GameElement element)
    {
        if (elements.Remove(element))
        {
            element.ElementId = 0;
            element.SetGame(null);

            Bus.Broadcast(new ElementUnregisteredMessage(element));
        }
    }

    internal record struct ElementRegisteredMessage(GameElement Element);

    internal record struct ElementUnregisteredMessage(GameElement Element);

    internal struct StartedMessage
    {
    }

    internal struct StoppedMessage
    {
    }
}