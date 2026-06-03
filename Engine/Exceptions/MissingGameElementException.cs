using Termule.Engine.Core;

namespace Termule.Engine.Exceptions;

/// <summary>
///     Base exception type for cases where a required <typeparamref name="TMissing" /> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of the missing <see cref="GameElement" />.</typeparam>
public abstract class MissingGameElementException<TMissing> : Exception
{
    private protected MissingGameElementException(GameElement dependent)
    {
        Dependent = dependent;
    }

    /// <summary>
    ///     Gets the element that requested the missing element.
    /// </summary>
    public GameElement Dependent { get; private init; }

    /// <summary>
    ///     Gets the <see cref="Type" /> of element that is missing.
    /// </summary>
    public Type MissingElementType { get; } = typeof(TMissing);
}
