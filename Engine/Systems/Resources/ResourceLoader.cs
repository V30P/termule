using System.IO.Abstractions;
using System.Reflection;
using Termule.Engine.Exceptions;

namespace Termule.Engine.Systems.Resources;

/// <summary>
///     System responsible for loading resources from disk.
/// </summary>
public sealed class ResourceLoader : Core.System
{
    private readonly Dictionary<string, IResourceBase> cache = [];

    private readonly IFileSystem fileSystem;
    private readonly string resourceDir;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ResourceLoader" /> class.
    /// </summary>
    /// <param name="fileSystem"> The file system to load from, defaults to the actual file system. </param>
    /// <param name="resourceDir">
    ///     The directory to load resources from, defaults to "res". Use
    ///     <paramref name="dirIsRelative" /> to
    /// </param>
    /// <param name="dirIsRelative">
    ///     If <paramref name="resourceDir" /> should be treated as relative to the assembly location,
    ///     defaults to true.
    /// </param>
    public ResourceLoader(
        IFileSystem fileSystem = null,
        string resourceDir = "res",
        bool dirIsRelative = true)
    {
        this.fileSystem = fileSystem ?? new FileSystem();

        string resourceDirBase =
            dirIsRelative ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) : string.Empty;
        this.resourceDir = Path.Combine(resourceDirBase, resourceDir);
    }

    /// <summary>
    ///     Loads the resource of provided type <typeparamref name="TResource" /> at <paramref name="path" />.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to load.</typeparam>
    /// <param name="path">The path to look for the resource at.</param>
    /// <returns>The loaded resource.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the resource does not exist at the provided path.</exception>
    /// <exception cref="ResourceLoadException">Thrown if the resource is found but fails to load.</exception>
    /// <remarks>If no extension is provided in the path, the default extension set by the resource will be used.</remarks>
    public TResource Load<TResource>(string path) where TResource : IResource
    {
        string extendedPath = fileSystem.Path.GetExtension(path) == TResource.FileExtension
            ? path
            : path + TResource.FileExtension;
        if (cache.TryGetValue(extendedPath, out IResourceBase cachedResource))
        {
            return Serializer.Deserialize<TResource>(Serializer.Serialize(cachedResource));
        }

        string fullPath = Path.Combine(resourceDir, extendedPath);
        if (!fileSystem.Path.Exists(fullPath))
        {
            throw new ResourceLoadException(fullPath, new FileNotFoundException());
        }

        string text;
        try
        {
            text = fileSystem.File.ReadAllText(fullPath);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            throw new ResourceLoadException(fullPath, e);
        }

        TResource resource = Serializer.Deserialize<TResource>(text);
        cache.Add(extendedPath, resource);
        return Serializer.Deserialize<TResource>(text);
    }
}