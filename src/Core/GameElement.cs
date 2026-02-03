namespace Termule.Core;

public abstract class GameElement : IHostedGameElement
{
    internal GameElement()
    {
    }

    protected event Action Registered;

    protected event Action Unregistered;

    Game IHostedGameElement.Game { get => this.Game; set => this.Game = value; }

    internal uint ElementID { get; private set; }

    protected Game Game { get; private set; }

    protected bool IsRegistered => this.Game != null;

    protected GameObject Root => this.Game?.Root;

    protected SystemManager Systems => this.Game?.Systems;

    public void InvokeRegistered(uint elementID)
    {
        this.ElementID = elementID;
        this.Registered?.Invoke();
    }

    public void InvokeUnregistered()
    {
        this.Unregistered?.Invoke();
    }

    protected TSystem GetRequiredSystem<TSystem>()
        where TSystem : System
    {
        return this.Systems.Get<TSystem>() is not TSystem system ?
            throw new MissingSystemException<TSystem>(this) : system;
    }
}

public interface IHostedGameElement
{
    Game Game { get; set; }

    void InvokeRegistered(uint elementID);

    void InvokeUnregistered();
}
