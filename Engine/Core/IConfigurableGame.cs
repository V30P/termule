namespace Termule.Engine.Core;

/// <summary>
///     Provides access to <see cref="Game" /> methods used for configuration.
/// </summary>
public interface IConfigurableGame
{
    /// <summary>
    ///     Gets the root game object.
    /// </summary>
    GameObject Root { get; }

    /// <summary>
    ///     Gets the <see cref="SystemManager" /> in configurable form.
    /// </summary>
    IConfigurableSystemManager Systems { get; }

    /// <summary>
    ///     Runs the game.
    /// </summary>
    void Run();

    internal void Prepare();

    internal void RunForFrames(int frames);

    internal void CleanUp();
}