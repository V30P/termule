using Termule.Engine.Core;

namespace Termule.Engine.Exceptions;

/// <summary>
///     Exception that is thrown when a required <see cref="System" /> is missing.
/// </summary>
/// <typeparam name="TMissing">The type of <see cref="System" /> that is missing.</typeparam>
public sealed class MissingSystemException<TMissing> : MissingGameElementException<TMissing>
    where TMissing : Core.System
{
    internal MissingSystemException(GameElement dependent) : base(dependent)
    {
    }

    /// <inheritdoc />
    public override string Message =>
        $"'{Dependent.GetType().Name}' is missing required system '{MissingElementType.Name}'";
}
