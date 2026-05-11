namespace Termule.Engine.Core;

/// <summary>
///     Game element that can be added to the <see cref="SystemManager" /> to provide global
///     behavior every tick. Only one instance of each direct child class of System can be in a
///     <see cref="Game" /> at a time.
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
    protected internal virtual void Stop()
    {
    }
}