namespace Termule.Resources;

// ! Do not implement this interface, it is used to get around static 
// ! abstract member limitations when working with IResources
public interface IResourceBase { }

public interface IResource : IResourceBase
{
    internal static abstract string FileExtension { get; }
}