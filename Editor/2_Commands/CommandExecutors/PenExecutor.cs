namespace Termule.Editor;

internal class PenExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "pen",
        avaliableOutsidePen = true
    };

    internal Game game;
    internal readonly Dictionary<string, Window> windows = [];

    protected override void Execute()
    {
        game = new Game(Editor.projectAssemblyPath);

        try
        {
            while (true)
            {
                RunCommandPrompt(game.cancellationToken);
            }
        }
        catch (OperationCanceledException) { }

        //Clean up
        foreach (Window window in windows.Values)
        {
            window.Close();
        }
    }

    void RunCommandPrompt(CancellationToken ct)
    {
        game.cancellationToken.ThrowIfCancellationRequested();
        Console.Write($"\n{Editor.projectName} (PEN)>");

        string input = Console.ReadLine();

        ct.ThrowIfCancellationRequested();
        Factory.Make<CommandExecutor>(input)?.StartExecute(this);
    }
}
