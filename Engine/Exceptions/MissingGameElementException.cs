using Termule.Core;

namespace Termule.Exceptions;

/// <summary>
///     Base exception type for cases where a required <see cref="GameElement" /> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of the missing <see cref="GameElement" />.</typeparam>
public abstract class MissingGameElementException<TMissing> : Exception
    where TMissing : GameElement
{
    /// <summary>
    ///     Gets the <see cref="GameElement" /> that needed the missing element.
    /// </summary>
    public readonly GameElement Dependent;

    /// <summary>
    ///     Gets the <see cref="Type" /> of element that is missing.
    /// </summary>
    public readonly Type MissingElementType = typeof(TMissing);

    private protected MissingGameElementException(GameElement dependent)
    {
        Dependent = dependent;
    }
}