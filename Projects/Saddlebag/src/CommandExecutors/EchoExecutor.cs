namespace Termule.Saddlebag;

[Executor("echo")]
internal class EchoExecutor
{
    public EchoExecutor(params string[] args)
    {
        string message = null;
        foreach (string arg in args)
        {
            message += arg + " ";
        }

        Console.WriteLine(message);
    }
}