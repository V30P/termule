namespace Termule.Core;

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

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void Stop()
    {
    }
}

public interface IHostedSystem
{
    void Start();

    void Update();

    void Stop();
}