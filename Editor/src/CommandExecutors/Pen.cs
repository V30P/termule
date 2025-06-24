using System.Threading.Tasks;

namespace Termule.Editor;

internal class Pen : ICommandExecutor
{
    internal Game game;

    public static CommandInfo commandInfo => new CommandInfo
    {
        avaliableOutsidePen = true
    };

    public void Execute(string[] _, Pen __)
    {
        game = new Game(Editor.projectAssemblyPath);

        try
        {
            while (true)
            {
                game.cancellationToken.ThrowIfCancellationRequested();
                RunCommandPrompt(game.cancellationToken);
            }
        }
        catch (OperationCanceledException) { }
    }

    void RunCommandPrompt(CancellationToken ct)
    {
        Console.Write($"\n{Editor.projectName} (PEN)>");

        string input = Console.ReadLine();
        string[] cleanedInput = input.TrimStart().TrimEnd().Split(' ');

        ct.ThrowIfCancellationRequested();
        ICommandExecutor commandExecutor = ICommandExecutor.GetExecutor(cleanedInput[0]);
        commandExecutor?.ValidateAndExecute(cleanedInput[1..], this);
    }
}
