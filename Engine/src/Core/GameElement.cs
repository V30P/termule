namespace Termule.Core;

/// <summary>
/// Base class for elements that live within a <see cref="Game"/>.
/// </summary>
public abstract class GameElement : IHostedGameElement
{
    internal GameElement()
    {
    }

    /// <summary>
    /// Invoked when this element is added to a <see cref="Game"/>.
    /// </summary>
    protected event Action Registered;

    /// <summary>
    /// Invoked when this element is removed from a <see cref="Game"/>.
    /// </summary>
    protected event Action Unregistered;

    Game IHostedGameElement.Game { get => this.Game; set => this.Game = value; }

    internal uint ElementID { get; private set; }

    /// <summary>
    /// Gets the <see cref="Game"/> that this element is a part of.
    /// </summary>
    protected Game Game { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this element is part of a Game.
    /// </summary>
    protected bool IsRegistered => this.Game != null;

    /// <summary>
    /// Gets the root GameObject of this element's Game.
    /// </summary>
    protected GameObject Root => this.Game?.Root;

    /// <summary>
    /// Gets the SystemManager of this element's Game.
    /// </summary>
    protected SystemManager Systems => this.Game?.Systems;

    void IHostedGameElement.InvokeRegistered(uint elementID)
    {
        this.ElementID = elementID;
        this.Registered?.Invoke();
    }

    void IHostedGameElement.InvokeUnregistered()
    {
        this.Unregistered?.Invoke();
    }

    /// <summary>
    /// Tries to get a System of type <typeparamref name="TSystem"/> from the containing Game.
    /// </summary>
    /// <typeparam name="TSystem"> The type of System to look for. </typeparam>
    /// <returns> The Game's instance of <typeparamref name="TSystem"/>. </returns>
    /// <exception cref="MissingSystemException{TSystem}"> Thrown if no matching System is found.</exception>
    protected TSystem GetRequiredSystem<TSystem>()
        where TSystem : System
    {
        return this.Systems.Get<TSystem>() is not TSystem system ?
            throw new MissingSystemException<TSystem>(this) : system;
    }
}

internal interface IHostedGameElement
{
    Game Game { get; set; }

    void InvokeRegistered(uint elementID);

    void InvokeUnregistered();
}
