using Termule.Engine.Exceptions;

namespace Termule.Engine.Systems.Resources;

/// <summary>
///     System responsible for loading resources from a <see cref="IResourceProvider"/> .
/// </summary>
public sealed class ResourceLoader(IResourceProvider provider = null) : Core.System
{
    private readonly Dictionary<string, IResource> cache = [];

    private readonly IResourceProvider provider = provider ?? new EmbeddedResourceProvider();

    /// <summary>
    ///     Loads a resource from the provider or gets a copy of a cached version.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to load.</typeparam>
    /// <param name="name">The name of the resource.</param>
    /// <returns>The resulting resource object.</returns>
    /// <exception cref="ResourceLoadException">Thrown when the resource cannot be loaded.</exception>
    public TResource Load<TResource>(string name) where TResource : IResource
    {
        if (cache.TryGetValue(name, out IResource cachedResource))
        {
            return Serializer.Deserialize<TResource>(Serializer.Serialize(cachedResource));
        }

        if (!provider.ResourceExists(name))
        {
            throw new ResourceLoadException(name, "Resource could not be found.");
        }

        Stream stream = provider.GetResourceStream(name);
        string text = new StreamReader(stream).ReadToEnd();

        TResource resource;
        try
        {
            resource = Serializer.Deserialize<TResource>(text);
        }
        catch (Exception e)
        {
            throw new ResourceLoadException(name, e);
        }

        cache.Add(name, resource);
        return resource;
    }
}
