namespace Termule.Exceptions;

/// <summary>
///     Exception that is thrown when a resource fails to load.
/// </summary>
public class ResourceLoadException : Exception
{
    /// <summary>
    ///     Gets the full path of the resource that failed to load.
    /// </summary>
    public readonly string FullPath;

    internal ResourceLoadException(string fullPath, Exception inner = null)
        : base($"Could not load resource \"{Path.GetFileName(fullPath)}\"", inner)
    {
        FullPath = fullPath;
    }
}