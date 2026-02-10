namespace Termule.Systems.Controller;

/// <summary>
/// Non-generic base class for binds.
/// </summary>
public abstract class Bind
{
    internal Bind()
    {
    }

    internal abstract void SetController(Controller controller);

    internal abstract object GetValue();
}

/// <summary>
/// Generic base class that lives on a host controller and convert user input to usable values.
/// </summary>
/// <typeparam name="TController">The controller type this bind can be used in.</typeparam>
public abstract class Bind<TController> : Bind
    where TController : Controller
{
    internal Bind()
    {
    }

    internal abstract TController Controller { set; }

    internal override void SetController(Controller controller)
    {
        if (controller is null or TController)
        {
            this.Controller = (TController)controller;
        }
        else
        {
            throw new InvalidOperationException(
                $"Bind of Type {this.GetType().Name} is incompatible with a Controller of type {controller.GetType().Name}");
        }
    }
}