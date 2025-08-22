using System.Text.Json;
using Microsoft.Build.Evaluation;

namespace Termule.Saddlebag;

internal class Environment
{
    internal static string currentPath { get; private set; }
    internal static string dataPath { get; private set; }
    internal static string configFilePath { get; private set; }
    internal static Configuration config { get; private set; }
    internal static Project project { get; private set; }

    internal static void Initialize()
    {
        currentPath = System.Environment.CurrentDirectory;
        dataPath = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)}\\Veop\\Saddlebag";

        configFilePath = dataPath + "\\config.json";
        config = File.Exists(configFilePath) ?
        JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configFilePath)) : new Configuration();

        DirectoryInfo currentDirectory = new DirectoryInfo(System.Environment.CurrentDirectory);
        if (currentDirectory.GetFiles().FirstOrDefault(file => file.Extension == ".csproj")?.FullName is string projectFileFullName)
        {
            project = new Project(projectFileFullName);
        }
    }
}