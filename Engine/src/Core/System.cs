namespace Termule.Core;

/// <summary>
/// Game element that can be added to the <see cref="SystemManager"/> to provide global behavior every tick.
/// Only one instance of each direct child class of System can be in a <see cref="Game"/> at a time.
/// </summary>
public abstract class System : GameElement, IHostedSystem
{
    void IHostedSystem.Start()
    {
        this.Start();
    }

    void IHostedSystem.Tick()
    {
        this.Tick();
    }

    void IHostedSystem.Stop()
    {
        this.Stop();
    }

    /// <summary>
    /// Behavior to execute when the containing <see cref="Game"/> is run.
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// Behavior to execute every tick.
    /// </summary>
    protected virtual void Tick()
    {
    }

    /// <summary>
    /// Behavior to execute when the containing <see cref="Game"/> is stopped.
    /// </summary>
    protected virtual void Stop()
    {
    }
}

internal interface IHostedSystem
{
    void Start();

    void Tick();

    void Stop();
}