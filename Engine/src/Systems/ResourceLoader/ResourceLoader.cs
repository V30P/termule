namespace Termule.Systems.ResourceLoader;

using System.Reflection;

/// <summary>
/// The <see cref="System"/> responsible for loading resources from disk.
/// </summary>
public sealed class ResourceLoader : Core.System
{
    private readonly Dictionary<string, IResourceBase> cache = [];
    private string resourcesDir;

    /// <summary>
    /// Loads the resource of provided type <typeparamref name="TResource"/> at provided <paramref name="path"/>.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to load.</typeparam>
    /// <param name="path">The path to look for the resource at.</param>
    /// <returns>The loaded resource.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the resource does not exist at the provided path.</exception>
    /// <exception cref="ResourceLoadException">Thrown if the resource is found, but fails to load.</exception>
    /// <remarks>If no extension is provided in the path, the default extension set in the Resource class will be used.</remarks>
    public TResource Load<TResource>(string path)
    where TResource : IResource
    {
        string extendedPath = Path.GetExtension(path) == TResource.FileExtension ? path : path + TResource.FileExtension;
        if (this.cache.TryGetValue(extendedPath, out IResourceBase resource))
        {
            return Serializer.Deserialize<TResource>(Serializer.Serialize(resource));
        }
        else
        {
            string fullPath = Path.Combine(this.resourcesDir, extendedPath);
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

    /// <summary>
    /// Determines the directory that resources will be loaded from.
    /// </summary>
    protected override void Start()
    {
        this.resourcesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "res");
    }
}
