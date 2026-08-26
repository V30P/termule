namespace Termule.Engine.Systems.Resources;

/// <summary>
///     Denotes a type that sources resources for a <see cref="ResourceLoader"/> .
/// </summary>
public interface IResourceProvider
{
    /// <summary>
    ///     Checks if a resource is available.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    /// <returns>Whether the resource was found.</returns>
    public bool ResourceExists(string name);

    /// <summary>
    ///     Gets a stream of a resource's data.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    /// <returns>The resource stream.</returns>
    public Stream GetResourceStream(string name);
}