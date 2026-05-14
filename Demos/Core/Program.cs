using System.Globalization;
using System.Reflection;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Demos.Core;

internal static class Program
{
    internal static readonly Layer UiLayer = new SimpleLayer();

    private const string ErrorTemplate = "Error: {0}.\n";

    private static readonly List<string> DemoNames = [];
    private static readonly Dictionary<string, Demo> Demos = [];

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

    static Program()
    {
        IEnumerable<Type> demoTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(Demo)));
        foreach (Type demoType in demoTypes)
        {
            DemoNames.Add(demoType.Name);
            Demos.Add(
                demoType.Name.ToLower(CultureInfo.CurrentCulture),
                (Demo) Activator.CreateInstance(demoType)
            );
        }
    }

    private enum Flag
    {
        Help,
        Interactive,
        Stats
    }

    private static string HelpText => $"""
                                       Usage: [OPTIONS] DEMO

                                       Options:
                                       --help, -h         Show this message and exit.
                                       --interactive, -i  Run in interactive mode.
                                       --stats, -s        Enable TPS indicator.

                                       Demos:
                                       {string.Join("\n", DemoNames)}
                                       """;

    public static void Main(string[] args)
    {
        ParseArgs(args, out HashSet<Flag> flags, out List<string> arguments);

        // Handle help flag
        if (flags.Contains(Flag.Help))
        {
            Console.WriteLine(HelpText);
            Environment.Exit(0);
        }

        // Error out if the argument combination is invalid
        bool interactive = flags.Contains(Flag.Interactive);
        switch (arguments.Count)
        {
            case 0 when !interactive:
                if (flags.Count == 0)
                {
                    ExitWithHelpText();
                }
                else
                {
                    ExitWithError("Must provide a demo to run");
                }

                return;
            case > 0 when interactive:
                ExitWithError("Interactive mode does not accept demo arguments");
                return;
            case > 1:
                ExitWithError("Too many arguments");
                return;
            default:
                break;
        }

        // Run the demo
        bool showStats = flags.Contains(Flag.Stats);
        if (interactive)
        {
            Console.Write("Run demo: ");
            string demoName = Console.ReadLine();
            RunDemo(demoName, showStats);
        }
        else
        {
            RunDemo(arguments[0], showStats);
        }
    }

    private static void ParseArgs(
        string[] args,
        out HashSet<Flag> flags,
        out List<string> arguments)
    {
        flags = [];
        arguments = [];
        foreach (string arg in args)
        {
            if (arg.StartsWith("--", StringComparison.CurrentCulture))
            {
                if (FlagLongNames.TryGetValue(arg[2..], out Flag flag))
                {
                    _ = flags.Add(flag);
                }
                else
                {
                    ExitWithError($"Unknown flag: '{arg}'");
                }
            }
            else if (arg.StartsWith('-'))
            {
                foreach (char flag in arg[1..])
                {
                    if (FlagShortNames.TryGetValue(flag, out Flag flagValue))
                    {
                        _ = flags.Add(flagValue);
                    }
                    else
                    {
                        ExitWithError($"Unknown flag: '{arg}'");
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

    private static void ExitWithHelpText()
    {
        Console.WriteLine(HelpText);
        Environment.Exit(0);
    }

    private static void RunDemo(string name, bool showStats = false)
    {
        if (!Demos.TryGetValue(name.ToLower(CultureInfo.CurrentCulture), out Demo demo))
        {
            ExitWithError($"No demo '{name}' found");
            return;
        }

        Game game = new();
        game.Systems.InstallDefaults();
        game.Systems.Install(new RenderSystem { Layers = [new SimpleLayer(), UiLayer] });
        game.Systems.Install(demo);

        if (showStats)
        {
            game.World.Add(
                new GameObject(
                    new Transform(),
                    new ContentRenderer<Text> { TargetSpace = true, Layer = UiLayer },
                    new TpsIndicator()
                )
            );
        }

        game.Run();
    }
}
