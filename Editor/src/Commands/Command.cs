namespace Termule.Editor;

abstract class CommandExecutor : Producible<CommandExecutorInfo>
{
    protected PenExecutor pen;

    internal void StartExecute(PenExecutor pen)
    {
        this.pen = pen;

        if (pen == null && !info.avaliableOutsidePen)
        {
            Console.WriteLine($"Cannot run command \"{info.name}\" outside of a pen");
        }
        else if (pen != null && !info.avaliableInsidePen)
        {
            Console.WriteLine($"Cannot run command \"{info.name}\" inside of a pen");
        }
        else
        {
            try
            {
                Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occured while executing the command \"{info.name}\":\n{e}");
            }
        }
    }

    protected abstract void Execute();
}
