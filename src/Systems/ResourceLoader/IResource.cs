namespace Termule.Systems.ResourceLoader;

public interface IResourceBase { }

public interface IResource : IResourceBase
{
    internal static abstract string FileExtension { get; }
}