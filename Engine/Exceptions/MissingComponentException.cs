using Termule.Engine.Core;

namespace Termule.Engine.Exceptions;

/// <summary>
///     Exception that is thrown when a required <see cref="Component" /> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of <see cref="Component" /> that is missing.</typeparam>
public class MissingComponentException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : Component
{
    /// <inheritdoc />
    public override string Message =>
        $"'{Dependent.GetType().Name}' is missing required component '{MissingElementType.Name}'";

    internal MissingComponentException(GameElement dependent)
        : base(dependent)
    {
    }
}