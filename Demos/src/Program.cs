using System.Reflection;
using Termule.Components;
using Termule.Core;
using Termule.Systems.RenderSystem;

internal static class Program
{
    internal static Layer UILayer { get; set; } = new SimpleLayer();

    public static void Main(string[] args)
    {
        Dictionary<string, Demo> demos = [];
        foreach
        (
            Type demoType in Assembly.GetExecutingAssembly()
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

            foreach (string demoName in demos.Keys)
            {
                Console.WriteLine($"- {demoName}");
            }

            return;
        }

        if (demos.TryGetValue(args[0], out Demo demo))
        {
            IConfigurableGame game = Game.Create();
            game.Systems.UseDefaults();
            game.Systems.Install(new RenderSystem() { Layers = [new SimpleLayer(), UILayer] });
            game.Systems.Install(demo);

#if DEBUG
            TPSIndicator tpsIndicator = [];
            tpsIndicator.Get<Renderer>().Layer = UILayer;
            game.Root.Add(tpsIndicator);
#endif

            game.Run();
            return;
        }

        Console.Error.WriteLine($"tm-demo: no demo '{args[0]}' found");
    }
}
