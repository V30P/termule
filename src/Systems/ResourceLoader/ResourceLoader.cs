using System.Reflection;

namespace Termule.Systems.ResourceLoader;

public sealed class ResourceLoader : Core.System
{
    private string _resourcesDir;
    private readonly Dictionary<string, IResourceBase> _cache = [];

    protected override void Start()
    {
        _resourcesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "res");
    }

    public TResource Load<TResource>(string path) where TResource : IResource
    {
        string extendedPath = Path.GetExtension(path) == TResource.FileExtension ? path : path + TResource.FileExtension;
        if (_cache.TryGetValue(extendedPath, out IResourceBase resource))
        {
            return Serializer.Deserialize<TResource>(Serializer.Serialize(resource));
        }
        else
        {
            string fullPath = Path.Combine(_resourcesDir, extendedPath);
            if (!Path.Exists(fullPath))
            {
                throw new FileNotFoundException("Resource file could not be found", fullPath);
            }

            string text;
            try
            {
                text = File.ReadAllText(fullPath);
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException)
            {
                throw new ResourceLoadException(fullPath, e);
            }

            return Serializer.Deserialize<TResource>(text);
        }
    }
}
