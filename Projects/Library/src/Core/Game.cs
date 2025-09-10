using System.Diagnostics;
using Termule.Input;

namespace Termule;

public static class Game
{
    static readonly GameObject root = [];

    public static float deltaTime { get; private set; }

    static bool stop;
    public static event Action Stopped;

    internal static void Run()
    {
        Stopwatch frameStopWatch = new Stopwatch();

        while (!stop)
        {
            InputSystem.GetInputs();

            foreach (Component component in root.ToArray())
            {
                component.Tick();
            }

            deltaTime = (float) frameStopWatch.Elapsed.TotalSeconds;
            frameStopWatch.Restart();
        }
    }

    public static void Add(Component component) => root.Add(component);
    public static void Add(params Component[] components) => root.Add(components);
    public static void Remove(Component component) => root.Remove(component);
    public static T Get<T>() where T : Component => root.Get<T>();

    public static void Stop()
    {
        stop = true;
        Stopped?.Invoke();
    }
}