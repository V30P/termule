using Termule.Engine.Core;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Denotes that a component provides a position for its <see cref="GameObject"/>.
/// </summary>
public interface IPositionProvider
{
    /// <summary>
    ///     Gets the position.
    /// </summary>
    public Vector Pos { get; }
}
