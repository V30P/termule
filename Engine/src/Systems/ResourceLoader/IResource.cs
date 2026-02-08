namespace Termule.Systems.ResourceLoader;

/// <summary>
/// Marker base interface for types that represent loadable resources.
/// </summary>
/// <remarks>
/// This interface is used internally by the <see cref="ResourceLoader"/> cache.
/// Consumers should typically implement <see cref="IResource"/> for concrete resources.
/// </remarks>
public interface IResourceBase
{
}

/// <summary>
/// Denotes a resource that can be loaded by the <see cref="ResourceLoader"/>.
/// </summary>
public interface IResource : IResourceBase
{
    /// <summary>
    /// Gets the file extension that will be appended to load paths if none is provided.
    /// </summary>
    public static abstract string FileExtension { get; }
}