using Termule.Exceptions;

namespace Termule.Core;

/// <summary>
///     Base class for elements that live within a <see cref="Game" />.
/// </summary>
public abstract class GameElement
{
    /// <summary>
    ///     Invoked when this element is added to a <see cref="Game" />.
    /// </summary>
    protected event Action Registered;

    /// <summary>
    ///     Invoked when this element is removed from a <see cref="Game" />.
    /// </summary>
    protected event Action Unregistered;

    internal uint ElementId { get; private set; }

    /// <summary>
    ///     Gets the game that this element is a part of.
    /// </summary>
    protected Game Game { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether this element is part of a game.
    /// </summary>
    protected bool IsRegistered => Game != null;

    /// <summary>
    ///     Gets the root of this element's game.
    /// </summary>
    protected GameObject Root => Game?.Root;

    /// <summary>
    ///     Gets the <see cref="SystemManager" /> of this element's game.
    /// </summary>
    protected SystemManager Systems => Game?.Systems;

    internal GameElement()
    {
    }

    internal void SetGame(Game game)
    {
        Game = game;
    }

    internal void InvokeRegistered(uint elementId)
    {
        ElementId = elementId;
        Registered?.Invoke();
    }

    internal void InvokeUnregistered()
    {
        Unregistered?.Invoke();
    }

    /// <summary>
    ///     Tries to get a system of type <typeparamref name="TSystem" /> from the containing game.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to look for.</typeparam>
    /// <returns>The game's instance of <typeparamref name="TSystem" />.</returns>
    /// <exception cref="MissingSystemException{TComponent}">Thrown if no matching system is found.</exception>
    protected TSystem GetRequiredSystem<TSystem>()
        where TSystem : System
    {
        var system = Systems.Get<TSystem>();
        return system ?? throw new MissingSystemException<TSystem>(this);
    }
}