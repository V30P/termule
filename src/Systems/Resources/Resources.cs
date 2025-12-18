using System.Text.Json;

namespace Termule.Resources;

public static class Resources
{
    private static readonly string _resourcesDir;
    private static readonly Dictionary<string, IResourceBase> _cache = [];

    static Resources()
    {
        _resourcesDir = Path.Combine(Program.GameDir, "res");
    }

    public static T Load<T>(string path) where T : IResource
    {
        string extendedPath = Path.GetExtension(path) == T.FileExtension ? path : path + T.FileExtension;
        if (_cache.TryGetValue(extendedPath, out IResourceBase resource))
        {
            return JsonSerializer.Deserialize<T>(Serializer.Serialize(resource));
        }
        else
        {
            string fullPath = Path.Combine(_resourcesDir, extendedPath);
            return Serializer.Deserialize<T>(File.ReadAllText(fullPath));
        }
    }
}