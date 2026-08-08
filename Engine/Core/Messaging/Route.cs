namespace Termule.Engine.Core;

/// <summary>
///     Represents a path for a message to be sent along.
/// </summary>
[Flags]
public enum Route
{
    /// <summary>
    ///     Send message only on the local <see cref="GameObject" />.
    /// </summary>
    /// <remarks>
    ///     This is the default behavior of <see cref="MessageBus.Broadcast" />.
    /// </remarks>
    Local = 1 << 0,

    /// <summary>
    ///     Send message on all ancestor <see cref="GameObject" /> busses.
    /// </summary>
    Upward = 1 << 1,

    /// <summary>
    ///     Send message on all descendant <see cref="GameObject" /> busses.
    /// </summary>
    Downward = 1 << 2
}
