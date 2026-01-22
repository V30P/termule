namespace Termule.Core;

public abstract class System : GameElement, IHostedSystem
{
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void Stop() { }

    void IHostedSystem.Start()
    {
        Start();
    }

    void IHostedSystem.Update()
    {
        Update();
    }

    void IHostedSystem.Stop()
    {
        Stop();
    }
}

public interface IHostedSystem
{
    void Start();
    void Update();
    void Stop();
}