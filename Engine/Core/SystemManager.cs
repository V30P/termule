using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;

namespace Termule.Engine.Core;

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

        Type systemType = GetSystemType<TSystem>();
        if (systems.Remove(systemType, out System system))
        {
            Game.Unregister(system);
        }
    }

    void IConfigurableSystemManager.UseDefaults()
    {
        IConfigurableSystemManager self = this;
        self.Install(new Keyboard());

        if (OperatingSystem.IsWindows())
        {
            self.Install(new WindowsDisplaySystem());
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            self.Install(new UnixDisplaySystem());
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