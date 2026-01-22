namespace Termule.Systems.ResourceLoader;

public abstract class ResourceLoader : Core.System
{
    public abstract TResource Load<TResource>(string path) where TResource : IResource;
}