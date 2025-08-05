using System.Collections.Concurrent;
namespace Termule;



public class Logger : Component
{
    readonly List<BlockingCollection<string>> distributors = [];

    public BlockingCollection<string> GetDistributor()
    {
        BlockingCollection<string> distributor = [];
        distributors.Add(distributor);

        return distributor;
    }

    public void Log(object message)
    {
        for (int i = distributors.Count - 1; i >= 0; i--)
        {
            if (!distributors[i].IsAddingCompleted)
            {
                distributors[i].Add(message.ToString() + '\n');
            }
            else
            {
                distributors.RemoveAt(i);
            }
        }
    }
}