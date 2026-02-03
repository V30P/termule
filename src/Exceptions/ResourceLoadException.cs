namespace Termule.Systems.ResourceLoader;

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

    public string FullPath { get; }
}