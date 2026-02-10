namespace Termule.Systems.ResourceLoader;

/// <summary>
/// Marker base interface for types that represent loadable resources.
/// </summary>
/// <remarks>
/// Do not implement this directly, instead implement <see cref="IResource"/>.
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