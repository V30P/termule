namespace Termule.Core;

public abstract class GameElement : IHostedGameElement
{
    protected Game Game { get; private set; }
    Game IHostedGameElement.Game { get => Game; set => Game = value; }
    internal uint ElementID { get; private set; }

    protected event Action Registered;
    protected event Action Unregistered;

    protected GameObject Root => Game.Root;
    protected SystemManager Systems => Game.Systems;

    internal GameElement() { }

    public void InvokeRegistered(uint elementID)
    {
        ElementID = elementID;
        Registered?.Invoke();
    }

    public void InvokeUnregistered()
    {
        Unregistered?.Invoke();
    }

    protected TSystem GetRequiredSystem<TSystem>() where TSystem : System
    {
        return Systems.Get<TSystem>() is not TSystem system ?
            throw new MissingSystemException<TSystem>(this) : system;
    }
}

public interface IHostedGameElement
{
    Game Game { get; set; }

    void InvokeRegistered(uint elementID);
    void InvokeUnregistered();
}
