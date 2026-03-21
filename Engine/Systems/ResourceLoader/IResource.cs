namespace Termule.Engine.Systems.ResourceLoader;

/// <summary>
///     Denotes a resource that can be loaded by the <see cref="ResourceLoader" />.
/// </summary>
public interface IResource : IResourceBase
{
    /// <summary>
    ///     Gets the file extension that will be appended to load paths if none is provided.
    /// </summary>
    public static abstract string FileExtension { get; }
}