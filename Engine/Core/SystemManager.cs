using Termule.Systems.Controller.Keyboard;
using Termule.Systems.Display;
using Termule.Systems.RenderSystem;
using Termule.Systems.ResourceLoader;

namespace Termule.Core;

/// <summary>
///     Manages systems and provides an interface to install, uninstall, and retrieve systems during <see cref="Game" />
///     configuration.
/// </summary>
public class SystemManager : GameElement, IConfigurableSystemManager
{
    private readonly Dictionary<Type, System> systems = [];

    void IConfigurableSystemManager.Install<TSystem>(TSystem system)
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        ((IConfigurableSystemManager)this).Uninstall<TSystem>();

        systems[GetSystemType<TSystem>()] = system;
        Game.Register(system);
    }

    void IConfigurableSystemManager.Uninstall<TSystem>()
    {
        if (Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        var systemType = GetSystemType<TSystem>();
        if (systems.Remove(systemType, out var system))
        {
            Game.Unregister(system);
        }
    }

    void IConfigurableSystemManager.UseDefaults()
    {
        IConfigurableSystemManager self = this;
        self.Install(new KeyboardController());

        if (OperatingSystem.IsWindows())
        {
            self.Install(new WindowsDisplay());
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            self.Install(new UnixDisplay());
        }

        self.Install(new RenderSystem());
        self.Install(new ResourceLoader());
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
        foreach (var system in systems.Values)
        {
            system.Start();
        }
    }

    internal void Tick()
    {
        foreach (var system in systems.Values)
        {
            system.Tick();
        }
    }

    internal void Stop()
    {
        foreach (var system in systems.Values)
        {
            system.Stop();
        }
    }

    private static Type GetSystemType<TSystem>()
        where TSystem : System
    {
        var type = typeof(TSystem);
        while (type!.BaseType != typeof(System))
        {
            type = type.BaseType;
        }

        return type;
    }
}