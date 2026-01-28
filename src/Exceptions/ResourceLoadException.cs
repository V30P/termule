namespace Termule.Systems.ResourceLoader;

public class ResourceLoadException : Exception
{
    public readonly string FullPath;

    internal ResourceLoadException(string fullPath, Exception inner)
        : base($"Could not load resource \"{Path.GetFileName(fullPath)}\"", inner)
    {
        FullPath = fullPath;
    }

    internal ResourceLoadException(string fullPath) : this(fullPath, null) { }
}