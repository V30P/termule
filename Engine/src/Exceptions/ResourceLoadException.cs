namespace Termule.Systems.ResourceLoader;

/// <summary>
/// The exception that is thrown when a resource fails to load.
/// </summary>
public class ResourceLoadException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceLoadException"/> class with an inner exception.
    /// </summary>
    /// <param name="fullPath">The full path of the resource that failed to load.</param>
    /// <param name="inner">The exception that caused the load to fail, if any.</param>
    internal ResourceLoadException(string fullPath, Exception inner)
    : base($"Could not load resource \"{Path.GetFileName(fullPath)}\"", inner)
    {
        this.FullPath = fullPath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceLoadException"/> class.
    /// </summary>
    /// <param name="fullPath">The full path of the resource that failed to load.</param>
    internal ResourceLoadException(string fullPath)
        : this(fullPath, null)
    {
    }

    /// <summary>
    /// Gets the full path of the resource that failed to load.
    /// </summary>
    public string FullPath { get; }
}