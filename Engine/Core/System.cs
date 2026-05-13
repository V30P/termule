namespace Termule.Engine.Core;

/// <summary>
///     Game element that can be installed on the <see cref="SystemManager" /> to provide global
///     behavior or data. Only one instance of each direct child class of System can be installed
///     at a time.
/// </summary>
public abstract class System : GameElement
{
    /// <summary>
    ///     Called when the <see cref="Game" /> is run.
    /// </summary>
    protected internal virtual void Start()
    {
    }

    /// <summary>
    ///     Called once per tick.
    /// </summary>
    protected internal virtual void Tick()
    {
    }

    /// <summary>
    ///     Called when the <see cref="Game" /> is stopped.
    /// </summary>
    protected internal virtual void CleanUp()
    {
    }
}
