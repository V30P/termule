using Termule.Engine.Types;

namespace Termule.Engine.Types;

/// <summary>
///     Rectangular collection of terminal cells.
/// </summary>
public interface IContent
{
    internal VectorInt Size { get; }
    internal Cell this[int x, int y] { get; }
}