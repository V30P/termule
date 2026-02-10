namespace Termule.Core;

/// <summary>
/// Base exception type for cases where a required <see cref="GameElement"/> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of the missing <see cref="GameElement"/>.</typeparam>
public abstract class MissingGameElementException<TMissing> : Exception
    where TMissing : GameElement
{
    private protected MissingGameElementException(GameElement dependent)
    {
        this.Dependent = dependent;
    }

    /// <summary>
    /// Gets the <see cref="GameElement"/> that needed the missing element.
    /// </summary>
    public GameElement Dependent { get; }

    /// <summary>
    /// Gets the <see cref="Type"/> of element that is missing.
    /// </summary>
    public Type MissingElementType { get; } = typeof(TMissing);
}

/// <summary>
/// Exception that is thrown when a required <see cref="Component"/> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of <see cref="Component"/> that is missing.</typeparam>
public class MissingComponentException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : Component
{
    internal MissingComponentException(GameElement dependent)
        : base(dependent)
    {
    }

    /// <inheritdoc/>
    public override string Message => $"'{this.Dependent.GetType().Name}' is missing required component '{this.MissingElementType.Name}'";
}

/// <summary>
/// Exception that is thrown when a required <see cref="System"/> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of <see cref="System"/> that is missing.</typeparam>
public class MissingSystemException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : System
{
    internal MissingSystemException(GameElement dependent)
        : base(dependent)
    {
    }

    /// <inheritdoc/>
    public override string Message => $"'{this.Dependent.GetType().Name}' is missing required system '{this.MissingElementType.Name}'";
}