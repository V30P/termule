namespace Termule.Systems.Controller;

public abstract class Bind
{
    private Bind() { }

    internal abstract void SetController(Controller controller);

    internal abstract object GetValue();

    public abstract class GenericBind<TController> : Bind where TController : Controller
    {
        internal GenericBind() { }

        internal abstract TController Controller { set; }

        internal override void SetController(Controller controller)
        {
            Controller = (TController)controller;
        }
    }
}

public abstract class Bind<TController> : Bind.GenericBind<TController> where TController : Controller
{
    internal Bind() { }
}