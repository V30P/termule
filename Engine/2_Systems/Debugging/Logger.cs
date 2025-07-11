using System.Collections.Concurrent;
namespace Termule;



public class Logger
{
    readonly List<BlockingCollection<string>> distributors = [];

    public BlockingCollection<string> GetDistributor()
    {
        BlockingCollection<string> distributor = [];
        distributors.Add(distributor);

        return distributor;
    }

    public void Log(string message)
    {
        for (int i = distributors.Count - 1; i >= 0; i--)
        {
            if (!distributors[i].IsAddingCompleted)
            {
                distributors[i].Add(message + '\n');
            }
            else
            {
                distributors.RemoveAt(i);
            }
        }
    }

    public void Log(object message) => Log(message.ToString());
    public void Log(Component source, object message) => Log($"{(source != null ? $"{source.path}: " : "")}{message}");
}