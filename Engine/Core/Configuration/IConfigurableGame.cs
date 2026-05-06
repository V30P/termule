using Termule.Engine.Core.Messaging;

namespace Termule.Engine.Core;

/// <summary>
///     Provides access to <see cref="Game" /> methods used for configuration.
/// </summary>
public interface IConfigurableGame
{
    /// <summary>
    ///     Gets the root game object.
    /// </summary>
    public GameObject Root { get; }

    /// <summary>
    ///     Gets the <see cref="SystemManager" /> in configurable form.
    /// </summary>
    public IConfigurableSystemManager Systems { get; }

    public MessageBus Bus { get; }

    /// <summary>
    ///     Runs the game.
    /// </summary>
    public void Run();

    // Use these for manual lifecycle control
    internal void Start();

    internal void RunFrame();

    internal void RunForFrames(int frames);

    internal void CleanUp();
}