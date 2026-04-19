using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Types.Content;

/// <summary>
///     Rectangular collection of terminal cells.
/// </summary>
public interface IContent
{
    internal VectorInt Size { get; }
    internal Cell this[int x, int y] { get; }
}