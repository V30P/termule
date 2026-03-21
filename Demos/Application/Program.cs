using System.Reflection;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.RenderSystem;

namespace Demos.Application;

internal static class Program
{
    private const string ErrorTemplate = "Error: {0}.\n";
    private static readonly Dictionary<string, Demo> Demos = [];
    internal static readonly Layer UiLayer = new SimpleLayer();

    private static readonly Dictionary<char, Flag> FlagShortNames = new()
    {
        ['h'] = Flag.Help,
        ['i'] = Flag.Interactive,
        ['s'] = Flag.Stats
    };

    private static readonly Dictionary<string, Flag> FlagLongNames = new()
    {
        ["help"] = Flag.Help,
        ["interactive"] = Flag.Interactive,
        ["stats"] = Flag.Stats
    };

    private enum Flag
    {
        Help,
        Interactive,
        Stats
    }

    private static string HelpText => $"""
                                       Usage: tm-demo [OPTIONS] DEMO

                                       Options:
                                       --help, -h         Show this message and exit.
                                       --interactive, -i  Run in interactive mode.
                                       --stats, -s        Show TPS indicator.

                                       Demos: 
                                       {string.Join('\n', Demos.Keys)}
                                       """;

    static Program()
    {
        foreach
        (
            var demoType in Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(Demo))))
        {
            Demos.Add(demoType.Name.ToLower(), (Demo)Activator.CreateInstance(demoType));
        }
    }

    public static void Main(string[] args)
    {
        ParseArgs(args, out var flags, out var arguments);

        // Handle help flag
        if (flags.Contains(Flag.Help))
        {
            Console.WriteLine(HelpText);
            Environment.Exit(0);
        }

        // Error out if the argument combination is invalid
        var interactive = flags.Contains(Flag.Interactive);
        switch (arguments.Count)
        {
            case 0 when !interactive:
                ExitWithError("Must provide a demo to run or launch in interactive mode");
                return;
            case > 0 when interactive:
                ExitWithError("Interactive mode does not accept demo arguments");
                return;
            case > 1:
                ExitWithError("Too many arguments");
                return;
        }

        // Run the demo
        var showStats = flags.Contains(Flag.Stats);
        if (interactive)
        {
            Console.Write("Run demo: ");
            var demoName = Console.ReadLine();
            RunDemo(demoName, showStats);
        }
        else
        {
            RunDemo(arguments[0], showStats);
        }
    }

    private static void ParseArgs(string[] args, out HashSet<Flag> flags, out List<string> arguments)
    {
        flags = [];
        arguments = [];
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                if (FlagLongNames.TryGetValue(arg[2..], out var flag))
                {
                    flags.Add(flag);
                }
                else
                {
                    ExitWithError($"Unknown flag: '{flag}'");
                }
            }
            else if (arg.StartsWith('-'))
            {
                foreach (var flag in arg[1..])
                {
                    if (FlagShortNames.TryGetValue(flag, out var flagValue))
                    {
                        flags.Add(flagValue);
                    }
                    else
                    {
                        ExitWithError($"Unknown flag: '{flag}'");
                    }
                }
            }
            else
            {
                arguments.Add(arg);
            }
        }
    }

    private static void ExitWithError(string message)
    {
        Console.Error.Write(ErrorTemplate, message);
        Environment.Exit(1);
    }

    private static void RunDemo(string name, bool showStats = false)
    {
        if (!Demos.TryGetValue(name, out var demo))
        {
            ExitWithError($"No demo '{name}' found");
            return;
        }

        var game = Game.Create();
        game.Systems.UseDefaults();
        game.Systems.Install(new RenderSystem { Layers = [new SimpleLayer(), UiLayer] });
        game.Systems.Install(demo);

        if (showStats)
        {
            TpsIndicator tpsIndicator = [];
            game.Root.Add(tpsIndicator);
            tpsIndicator.Get<Renderer>().Layer = UiLayer;
        }

        game.Run();
    }
}