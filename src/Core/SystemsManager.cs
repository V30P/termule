namespace Termule.Core;

using Systems.Controller.Keyboard;
using Systems.Display;
using Systems.RenderSystem;
using Systems.ResourceLoader;

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

    void IHostedSystemManager.Update()
    {
        foreach (IHostedSystem system in this.systems.Values)
        {
            system.Update();
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
        ((IConfigurableSystemManager)this).Uninstall<TSystem>();

        this.systems[GetSystemType<TSystem>()] = system;
        this.Game.Register(system);
    }

    void IConfigurableSystemManager.Uninstall<TSystem>()
    {
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

public interface IConfigurableSystemManager
{
    void Install<TSystem>(TSystem system)
        where TSystem : System;

    void Uninstall<TSystem>()
        where TSystem : System;

    TSystem Get<TSystem>()
        where TSystem : System;

    void UseDefaults();
}

public interface IHostedSystemManager
{
    void Start();

    void Update();

    void Stop();
}