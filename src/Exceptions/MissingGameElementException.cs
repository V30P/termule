namespace Termule.Core;

public abstract class MissingGameElementException<TMissing> : Exception
    where TMissing : GameElement
{
    private protected MissingGameElementException(GameElement dependent)
    {
        this.Dependent = dependent;
    }

    public GameElement Dependent { get; }

    public Type MissingElementType { get; } = typeof(TMissing);
}

public class MissingComponentException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : Component
{
    internal MissingComponentException(GameElement dependent)
        : base(dependent)
    {
    }

    public override string Message => $"'{this.Dependent.GetType().Name}' is missing required component '{this.MissingElementType.Name}'";
}

public class MissingSystemException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : System
{
    internal MissingSystemException(GameElement dependent)
        : base(dependent)
    {
    }

    public override string Message => $"'{this.Dependent.GetType().Name}' is missing required system '{this.MissingElementType.Name}'";
}