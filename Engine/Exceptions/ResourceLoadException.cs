namespace Termule.Engine.Exceptions;

/// <summary>
///     Exception that is thrown when a resource fails to load.
/// </summary>
public sealed class ResourceLoadException : Exception
{
    internal ResourceLoadException(string name, string reason)
        : base($"{GetMessage(name)} {reason}")
    {
        Name = name;
    }

    internal ResourceLoadException(string name, Exception inner)
        : base(GetMessage(name), inner)
    {
        Name = name;
    }

    /// <summary>
    ///     Gets the name of the resource that failed to load.
    /// </summary>
    public string Name { get; }

    private static string GetMessage(string name)
    {
        return $"Could not load resource \"{name}\".";
    }
}
