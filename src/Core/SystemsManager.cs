using Termule.Systems.Controller.Keyboard;
using Termule.Systems.Display;
using Termule.Systems.RenderSystem;
using Termule.Systems.ResourceLoader;

namespace Termule.Core;

public class SystemManager : GameElement, IHostedSystemManager, IConfigurableSystemManager
{
    private readonly Dictionary<Type, IHostedSystem> _systems = [];

    void IHostedSystemManager.Start()
    {
        foreach (IHostedSystem system in _systems.Values)
        {
            system.Start();
        }
    }

    void IHostedSystemManager.Update()
    {
        foreach (IHostedSystem system in _systems.Values)
        {
            system.Update();
        }
    }

    void IHostedSystemManager.Stop()
    {
        foreach (IHostedSystem system in _systems.Values)
        {
            system.Stop();
        }
    }

    void IConfigurableSystemManager.Install<TSystem>(TSystem system)
    {
        ((IConfigurableSystemManager)this).Uninstall<TSystem>();

        _systems[GetSystemType<TSystem>()] = system;
        Game.Register(system);
    }

    void IConfigurableSystemManager.Uninstall<TSystem>()
    {
        Type systemType = GetSystemType<TSystem>();
        if (_systems.TryGetValue(systemType, out IHostedSystem system))
        {
            _systems.Remove(systemType);
            Game.Unregister((System)system);
        }
    }

    public TSystem Get<TSystem>() where TSystem : System
    {
        return (TSystem)(_systems.TryGetValue(GetSystemType<TSystem>(), out IHostedSystem system) ? system : null);
    }

    private static Type GetSystemType<TSystem>() where TSystem : IHostedSystem
    {
        Type type = typeof(TSystem);
        while (type.BaseType != typeof(System))
        {
            type = type.BaseType;
        }

        return type;
    }

    void IConfigurableSystemManager.UseDefaults()
    {
        IConfigurableSystemManager self = this;
        self.Install(new KeyboardController());
        self.Install(new UnixDisplay());
        self.Install(new BaseRenderSystem());
        self.Install(new BaseResourceLoader());
    }
}

public interface IConfigurableSystemManager
{
    void Install<TSystem>(TSystem system) where TSystem : System;
    void Uninstall<TSystem>() where TSystem : System;
    TSystem Get<TSystem>() where TSystem : System;

    void UseDefaults();
}

public interface IHostedSystemManager
{
    void Start();
    void Update();
    void Stop();
}