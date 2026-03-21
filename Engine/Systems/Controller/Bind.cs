namespace Termule.Engine.Systems.Controller;

/// <summary>
///     Non-generic base class for binds.
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
///     Generic base class that lives on a host controller and convert user input to usable values.
/// </summary>
/// <typeparam name="TController">The controller type this bind can be used in.</typeparam>
public abstract class Bind<TController> : Bind
    where TController : Controller
{
    internal abstract TController Controller { set; }

    internal Bind()
    {
    }

    internal override void SetController(Controller controller)
    {
        if (controller is null or TController)
        {
            Controller = (TController)controller;
        }
        else
        {
            throw new InvalidOperationException(
                $"Bind of Type {GetType().Name} is incompatible with a Controller of type {controller.GetType().Name}");
        }
    }
}