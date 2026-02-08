namespace Termule.Core;

/// <summary>
/// A <see cref="GameElement"/> that can be added to a <see cref="SystemManager"/>s to provide global behavior every frame.
/// </summary>
public abstract class System : GameElement, IHostedSystem
{
    void IHostedSystem.Start()
    {
        this.Start();
    }

    void IHostedSystem.Update()
    {
        this.Update();
    }

    void IHostedSystem.Stop()
    {
        this.Stop();
    }

    /// <summary>
    /// The behavior to execute when the containing Game is run.
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// The behavior to execute every frame.
    /// </summary>
    protected virtual void Update()
    {
    }

    /// <summary>
    /// The behavior to execute when the containing Game is stopped.
    /// </summary>
    protected virtual void Stop()
    {
    }
}

internal interface IHostedSystem
{
    void Start();

    void Update();

    void Stop();
}