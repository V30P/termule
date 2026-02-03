namespace Termule.Systems.ResourceLoader;

// ! Do not implement this directly, instead implement IResource
public interface IResourceBase
{
}

public interface IResource : IResourceBase
{
    internal static abstract string FileExtension { get; }
}