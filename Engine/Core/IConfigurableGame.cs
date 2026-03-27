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
    
    // Use these for manual lifecycle control
    internal void Prepare();
    
    internal void RunFrame();
    
    internal void RunForFrames(int frames);

    internal void CleanUp();
}