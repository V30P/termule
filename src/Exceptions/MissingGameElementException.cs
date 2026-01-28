namespace Termule.Core;

public abstract class MissingGameElementException<TMissing> : Exception where TMissing : GameElement
{
    public readonly GameElement Dependent;
    public readonly Type MissingElementType = typeof(TMissing);

    private protected MissingGameElementException(GameElement dependent)
    {
        Dependent = dependent;
    }
}

public class MissingComponentException<TMissing>
    : MissingGameElementException<TMissing> where TMissing : Component
{
    public override string Message => $"'{Dependent.GetType().Name}' is missing required component '{MissingElementType.Name}'";

    internal MissingComponentException(GameElement dependent) : base(dependent) { }
}

public class MissingSystemException<TMissing>
    : MissingGameElementException<TMissing> where TMissing : System
{
    public override string Message => $"'{Dependent.GetType().Name}' is missing required system '{MissingElementType.Name}'";

    internal MissingSystemException(GameElement dependent) : base(dependent) { }
}