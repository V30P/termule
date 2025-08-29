using System.Diagnostics;
using Termule.Input;
using Termule.Rendering;

namespace Termule;

public static class Game
{
    static readonly GameObject root = [];

    public static float deltaTime { get; private set; }
    static bool stop;

    internal static void Run()
    {
        Stopwatch frameStopWatch = new Stopwatch();

        while (!stop)
        {
            RenderSystem.DrawFrame((0, 0), (Console.WindowWidth, Console.WindowHeight));
            InputSystem.GetInputs();

            foreach (Component component in root.ToArray())
            {
                component.Tick();
            }

            deltaTime = (float) frameStopWatch.Elapsed.TotalSeconds;
            frameStopWatch.Restart();
        }

        foreach (Component component in root.ToArray())
        {
            component.Destroy();
        }
    }

    public static void Add(Component component) => root.Add(component);
    public static void Add(params Component[] components) => root.Add(components);
    public static void Remove(Component component) => root.Remove(component);
    public static T Get<T>() where T : Component => root.Get<T>();

    public static void Stop() => stop = true;
}