using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;

namespace Termule.Engine.Core;

/// <summary>
///     Manages systems and provides an interface to install, uninstall, and retrieve systems
///     during <see cref="Game" /> configuration.
/// </summary>
public sealed class SystemManager : GameElement
{
    private readonly Dictionary<Type, System> systems = [];

    internal SystemManager()
    {
    }

    /// <summary>
    ///     Installs the provided <paramref name="systems" />, replacing the existing instance of
    ///     that system base class (if any).
    /// </summary>
    /// <param name="systems">The systems to install.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when trying to install systems when the <see cref="Game" /> is already started.
    /// </exception>
    public void Install(params System[] systems)
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        foreach (System system in systems)
        {
            Uninstall(system.GetType());
            this.systems[GetSystemType(system.GetType())] = system;
        }

        // Activate systems simultaneously
        foreach (System system in systems)
        {
            Game.Activate(system);
        }
    }

    /// <summary>
    ///     Uninstalls the system of type <typeparamref name="TSystem" /> (if any).
    /// </summary>
    /// <typeparam name="TSystem">The type of system to uninstall.</typeparam>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when trying to uninstall systems when the <see cref="Game" /> is already started.
    /// </exception>
    public void Uninstall<TSystem>() where TSystem : System
    {
        Uninstall(typeof(TSystem));
    }

    /// <summary>
    ///     Gets the installed system of type <typeparamref name="TSystem" />, or <c>null</c> if
    ///     none is installed.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to retrieve.</typeparam>
    /// <returns>The installed system or <c>null</c>.</returns>
    public TSystem Get<TSystem>() where TSystem : System
    {
        return (TSystem) systems.GetValueOrDefault(GetSystemType(typeof(TSystem)));
    }

    /// <summary>
    ///     Installs the operating-system-specific default <see cref="Systems" />s.
    /// </summary>
    public void InstallDefaults()
    {
        Install(new Keyboard());
        Install(new RenderSystem());
        Install(new ResourceLoader());

        // Stop if no console is available
        try
        {
            _ = Console.WindowWidth;
        }
        catch (IOException)
        {
            return;
        }

        if (OperatingSystem.IsWindows())
        {
            Install(new WindowsDisplaySystem());
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            Install(new UnixDisplaySystem());
        }
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
            system.CleanUp();
        }
    }

    private static Type GetSystemType(Type type)
    {
        while (type.BaseType != typeof(System))
        {
            type = type.BaseType;
        }

        return type;
    }

    private void Uninstall(Type type)
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        Type systemType = GetSystemType(type);
        if (systems.Remove(systemType, out System system))
        {
            Game.Deactivate(system);
        }
    }
}
