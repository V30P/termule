using System.Diagnostics;

namespace Termule;

public static class Game
{
    private static readonly GameObject _root = [];

    public static float DeltaTime { get; private set; }

    private static bool _stop;
    public static event Action Stopped;

    internal static void Run()
    {
        Stopwatch frameStopWatch = new();

        while (!_stop)
        {
            Input.Controller.UpdateValues();

            foreach (Component component in _root.ToArray())
            {
                component.Tick();
            }

            DeltaTime = (float)frameStopWatch.Elapsed.TotalSeconds;
            frameStopWatch.Restart();
        }
    }

    public static void Add(Component component)
    {
        _root.Add(component);
        component.IsRooted = true;
    }

    public static void Add(params Component[] components)
    {
        foreach (Component component in components)
        {
            Add(component);
        }
    }

    public static void Remove(Component component)
    {
        _root.Remove(component);
    }

    public static T Get<T>() where T : Component
    {
        return _root.Get<T>();
    }

    public static void Stop()
    {
        _stop = true;
        Stopped?.Invoke();
    }
}