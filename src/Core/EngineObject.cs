namespace Termule.Core;

public abstract class GameElement : IHostedGameElement
{
    protected Game Game { get; private set; }
    Game IHostedGameElement.Game { get => Game; set => Game = value; }

    protected event Action Registered;
    protected event Action Unregistered;

    protected GameObject Root => Game.Root;
    protected SystemManager Systems => Game.Systems;

    internal GameElement() { }

    public void InvokeRegistered()
    {
        Registered?.Invoke();
    }

    public void InvokeUnregistered()
    {
        Unregistered?.Invoke();
    }
}

public interface IHostedGameElement
{
    Game Game { get; set; }

    void InvokeRegistered();
    void InvokeUnregistered();
}
