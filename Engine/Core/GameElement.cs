using Termule.Engine.Exceptions;

namespace Termule.Engine.Core;

/// <summary>
///     Base class for elements that live within a <see cref="Game" />.
/// </summary>
public abstract class GameElement
{
    internal GameElement()
    {
    }

    internal uint ElementId { get; set; }

    /// <summary>
    ///     Gets the game that this element is a part of.
    /// </summary>
    protected Game Game { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether this element is part of a game.
    /// </summary>
    protected bool Activated => Game != null;

    /// <summary>
    ///     Gets the world <see cref="GameObject"/>  of this element's <see cref="Game"/>.
    /// </summary>
    protected GameObject World => Game?.World;

    /// <summary>
    ///     Gets the <see cref="SystemManager" /> of this element's <see cref="Game"/>.
    /// </summary>
    protected SystemManager Systems => Game?.Systems;

    internal void SetGame(Game value)
    {
        if (value != null)
        {
            Game = value;
            Activate();
        }
        else if (Game != null)
        {
            Deactivate();
            Game = null;
        }
    }

    /// <summary>
    ///     Called when this element is added to a <see cref="Game" />.
    /// </summary>
    protected virtual void Activate()
    {
    }

    /// <summary>
    ///     Called when this element is removed from its <see cref="Game" />.
    /// </summary>
    protected virtual void Deactivate()
    {
    }

    /// <summary>
    ///     Tries to get a system of type <typeparamref name="TSystem" /> from the containing game,
    ///     throwing if it cannot be found.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to look for.</typeparam>
    /// <returns>The game's instance of <typeparamref name="TSystem" />.</returns>
    /// <exception cref="MissingSystemException{TSystem}">
    ///     Thrown if no matching system is found.
    /// </exception>
    protected TSystem GetRequiredSystem<TSystem>() where TSystem : System
    {
        TSystem system = Systems.Get<TSystem>();
        return system ?? throw new MissingSystemException<TSystem>(this);
    }
}
