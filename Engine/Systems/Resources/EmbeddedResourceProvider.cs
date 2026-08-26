using System.Reflection;

namespace Termule.Engine.Systems.Resources;

/// <summary>
///     Loads resources that are embedded in the entry assembly.
/// </summary>
/// <param name="baseName">A value to prepend to the provided name when accessing a resource.</param>
public sealed class EmbeddedResourceProvider(string baseName = "res/") : IResourceProvider
{
    private readonly Assembly assembly = Assembly.GetEntryAssembly();

    /// <inheritdoc/>
    public bool ResourceExists(string name)
    {
        return assembly.GetManifestResourceNames().Contains(baseName + name);
    }

    /// <inheritdoc/>
    public Stream GetResourceStream(string name)
    {
        return assembly.GetManifestResourceStream(baseName + name);
    }
}