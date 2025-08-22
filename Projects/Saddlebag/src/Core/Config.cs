using System.Text.Json;

namespace Termule.Saddlebag;

internal class Configuration
{
    public string bootstrapperPath { get; set; }

    internal void Save()
    {
        Directory.CreateDirectory(Environment.dataPath);
        string data = JsonSerializer.Serialize(this);
        File.WriteAllText(Environment.configFilePath, data);
    }
}