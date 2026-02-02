using System.Diagnostics;
namespace Termule.Core;

public sealed class Game : IConfigurableGame
{
    private readonly List<IHostedGameElement> _elements = [];

    public readonly GameObject Root;
    GameObject IConfigurableGame.Root => Root;

    public readonly SystemManager Systems;
    IConfigurableSystemManager IConfigurableGame.Systems => Systems;

    private readonly Stopwatch _stopwatch = new();
    public float DeltaTime { get; private set; }

    private bool _stop = false;

    public Game()
    {
        Root = [];
        Register(Root);

        Systems = new SystemManager();
        Register(Systems);
    }

    public static IConfigurableGame Create()
    {
        return new Game();
    }

    void IConfigurableGame.Run()
    {
        IHostedSystemManager systems = Systems;
        IHostedComponent root = Root;

        systems.Start();
#if RELEASE
        try
        {
#endif

        while (!_stop)
        {
            DeltaTime = (float)_stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

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
        _stop = true;
    }

    internal void Register(IHostedGameElement element)
    {
        element.Game = this;
        uint id = (uint)_elements.Count;

        _elements.Add(element);
        element.InvokeRegistered(id);
    }

    internal void Unregister(IHostedGameElement element)
    {
        element.InvokeUnregistered();

        element.Game = null;
        _elements.Remove(element);
    }
}

public interface IConfigurableGame
{
    GameObject Root { get; }
    IConfigurableSystemManager Systems { get; }

    void Run();
}