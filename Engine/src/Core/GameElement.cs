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
    /// Gets the game that this element is a part of.
    /// </summary>
    protected Game Game { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this element is part of a game.
    /// </summary>
    protected bool IsRegistered => this.Game != null;

    /// <summary>
    /// Gets the root of this element's game.
    /// </summary>
    protected GameObject Root => this.Game?.Root;

    /// <summary>
    /// Gets the <see cref="SystemManager"/> of this element's game.
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
    /// Tries to get a system of type <typeparamref name="TSystem"/> from the containing game.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to look for.</typeparam>
    /// <returns>The games's instance of <typeparamref name="TSystem"/>.</returns>
    /// <exception cref="MissingSystemException{TComponent}">Thrown if no matching system is found.</exception>
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
