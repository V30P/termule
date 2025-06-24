namespace Termule.Editor;

internal class Stop : ICommandExecutor
{
    public static CommandInfo commandInfo => new CommandInfo
    {
        avaliableInsidePen = true
    };

    public void Execute(string[] _, Pen pen) => pen.game.Stop();
}