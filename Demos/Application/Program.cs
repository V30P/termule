using System.Reflection;
using Termule.Components;
using Termule.Core;
using Termule.Systems.RenderSystem;

namespace Demos.Application;

internal static class Program
{
    internal static readonly Layer UiLayer = new SimpleLayer();

    public static void Main(string[] args)
    {
        Dictionary<string, Demo> demos = [];
        foreach
        (
            var demoType in Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(Demo))))
        {
            demos.Add(demoType.Name.ToLower(), (Demo)Activator.CreateInstance(demoType));
        }

        if (args.Length == 0)
        {
            Console.WriteLine(
                """
                Usage: tm-demo <demo>
                Demos:
                """);

            foreach (var demoName in demos.Keys)
            {
                Console.WriteLine($"- {demoName}");
            }

            return;
        }

        if (demos.TryGetValue(args[0], out var demo))
        {
            var game = Game.Create();
            game.Systems.UseDefaults();
            game.Systems.Install(new RenderSystem { Layers = [new SimpleLayer(), UiLayer] });
            game.Systems.Install(demo);

#if DEBUG
            TpsIndicator tpsIndicator = [];
            game.Root.Add(tpsIndicator);
            tpsIndicator.Get<Renderer>().Layer = UiLayer;
#endif

            game.Run();
            return;
        }

        Console.Error.WriteLine($"tm-demo: no demo '{args[0]}' found");
    }
}