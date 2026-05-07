using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;

namespace Termule.Engine.Core;

/// <summary>
///     Manages systems and provides an interface to install, uninstall, and retrieve systems during <see cref="Game" />
///     configuration.
/// </summary>
public class SystemManager : GameElement
{
    private readonly Dictionary<Type, System> systems = [];

    /// <summary>
    ///     Installs the provided <paramref name="system"/>, replacing the existing instance of that
    ///     system base class (if any).
    /// </summary>
    /// <typeparam name="TSystem">The type of system being installed.</typeparam>
    /// <param name="system">The system to install.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when trying to install systems when the <see cref="Game"/> is already started.
    /// </exception>
    public void Install<TSystem>(TSystem system) where TSystem : System
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        Uninstall<TSystem>();

        systems[GetSystemType<TSystem>()] = system;
        Game.Register(system);
    }

    /// <summary>
    ///     Uninstalls the system of type <typeparamref name="TSystem"/> (if any).
    /// </summary>
    /// <typeparam name="TSystem">The type of system to uninstall.</typeparam>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when trying to uninstall systems when the <see cref="Game"/> is already started.
    /// </exception>
    public void Uninstall<TSystem>() where TSystem : System
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        Type systemType = GetSystemType<TSystem>();
        if (systems.Remove(systemType, out System system))
        {
            Game.Unregister(system);
        }
    }

    /// <summary>
    /// Installs the system-specific default <see cref="Systems"/>s. 
    /// </summary>
    public void UseDefaults()
    {
        Install(new Keyboard());

        if (OperatingSystem.IsWindows())
        {
            Install(new WindowsDisplaySystem());
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            Install(new UnixDisplaySystem());
        }

        Install(new RenderSystem());
        Install(new ResourceLoader());
    }

    /// <summary>
    ///     Gets the installed system of type <typeparamref name="TSystem" />, or <c>null</c> if none is installed.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to retrieve.</typeparam>
    /// <returns>The installed system or <c>null</c>.</returns>
    public TSystem Get<TSystem>()
        where TSystem : System
    {
        return (TSystem)systems.GetValueOrDefault(GetSystemType<TSystem>());
    }

    internal void Start()
    {
        foreach (System system in systems.Values)
        {
            system.Start();
        }
    }

    internal void Tick()
    {
        foreach (System system in systems.Values)
        {
            system.Tick();
        }
    }

    internal void Stop()
    {
        foreach (System system in systems.Values)
        {
            system.Stop();
        }
    }

    private static Type GetSystemType<TSystem>()
        where TSystem : System
    {
        Type type = typeof(TSystem);
        while (type!.BaseType != typeof(System))
        {
            type = type.BaseType;
        }

        return type;
    }
}
