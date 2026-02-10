namespace Termule.Systems.ResourceLoader;

/// <summary>
/// Exception that is thrown when a resource fails to load.
/// </summary>
public class ResourceLoadException : Exception
{
    internal ResourceLoadException(string fullPath, Exception inner)
    : base($"Could not load resource \"{Path.GetFileName(fullPath)}\"", inner)
    {
        this.FullPath = fullPath;
    }

    internal ResourceLoadException(string fullPath)
        : this(fullPath, null)
    {
    }

    /// <summary>
    /// Gets the full path of the resource that failed to load.
    /// </summary>
    public string FullPath { get; }
}