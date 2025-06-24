namespace Termule.Editor;

internal class Echo : ICommandExecutor
{
    public static CommandInfo commandInfo => new CommandInfo
    {
        avaliableOutsidePen = true,
        avaliableInsidePen = true
    };

    public void Execute(string[] args, Pen pen)
    {
        foreach (string arg in args[..^1])
        {
            Console.Write($"{arg} ");
        }
        Console.WriteLine(args[^1]);
    }
}