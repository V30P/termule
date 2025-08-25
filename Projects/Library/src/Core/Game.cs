using System.Diagnostics;

namespace Termule;

public sealed class Game : IComposite
{
    public Game game => this;
    public Dictionary<string, Component> components => _components;
    readonly Dictionary<string, Component> _components = [];

    bool stop;

    public float deltaTime { get; private set; }

    internal void Run()
    {
        Stopwatch frameStopWatch = new Stopwatch();

        while (!stop)
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

    public void Stop() => stop = true;
}