namespace Termule.Core;

using Systems.Controller.Keyboard;
using Systems.Display;
using Systems.RenderSystem;
using Systems.ResourceLoader;

/// <summary>
/// Manages systems and provides an interface to install, uninstall and retrieve systems during <see cref="Game"/> configuration.
/// </summary>
public class SystemManager : GameElement, IHostedSystemManager, IConfigurableSystemManager
{
    private readonly Dictionary<Type, IHostedSystem> systems = [];

    void IHostedSystemManager.Start()
    {
        foreach (IHostedSystem system in this.systems.Values)
        {
            system.Start();
        }
    }

    void IHostedSystemManager.Tick()
    {
        foreach (IHostedSystem system in this.systems.Values)
        {
            system.Tick();
        }
    }

    void IHostedSystemManager.Stop()
    {
        foreach (IHostedSystem system in this.systems.Values)
        {
            system.Stop();
        }
    }

    void IConfigurableSystemManager.Install<TSystem>(TSystem system)
    {
        if (this.Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        ((IConfigurableSystemManager)this).Uninstall<TSystem>();

        this.systems[GetSystemType<TSystem>()] = system;
        this.Game.Register(system);
    }

    void IConfigurableSystemManager.Uninstall<TSystem>()
    {
        if (this.Game.Started)
        {
            throw new InvalidOperationException("Cannot change systems once the game is started.");
        }

        Type systemType = GetSystemType<TSystem>();
        if (this.systems.TryGetValue(systemType, out IHostedSystem system))
        {
            this.systems.Remove(systemType);
            this.Game.Unregister((System)system);
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
    /// Gets the installed system of type <typeparamref name="TSystem"/>, or <c>null</c> if none is installed.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to retrieve.</typeparam>
    /// <returns>The installed system or <c>null</c>.</returns>
    public TSystem Get<TSystem>()
        where TSystem : System
    {
        return (TSystem)(this.systems.TryGetValue(GetSystemType<TSystem>(), out IHostedSystem system) ? system : null);
    }

    private static Type GetSystemType<TSystem>()
        where TSystem : IHostedSystem
    {
        Type type = typeof(TSystem);
        while (type.BaseType != typeof(System))
        {
            type = type.BaseType;
        }

        return type;
    }
}

/// <summary>
/// Provides configuration operations for installing, uninstalling and retrieving <see cref="System"/>s.
/// </summary>
public interface IConfigurableSystemManager
{
    /// <summary>
    /// Installs the given <paramref name="system"/>, replacing any previously installed implementation of the same system.
    /// </summary>
    /// <param name="system">The system instance to install.</param>
    /// <typeparam name="TSystem">The type of the system to install.</typeparam>
    void Install<TSystem>(TSystem system)
        where TSystem : System;

    /// <summary>
    /// Uninstalls a previously installed system of the specified type, if present.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to uninstall.</typeparam>
    void Uninstall<TSystem>()
        where TSystem : System;

    /// <summary>
    /// Gets the installed <see cref="System"/> of the specified type, or <c>null</c> if none is installed.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to look for.</typeparam>
    /// <returns>The installed system or <c>null</c>.</returns>
    TSystem Get<TSystem>()
        where TSystem : System;

    /// <summary>
    /// Installs the default set of systems appropriate for the current operating system.
    /// </summary>
    void UseDefaults();
}

internal interface IHostedSystemManager
{
    void Start();

    void Tick();

    void Stop();
}