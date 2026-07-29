namespace Termule.Engine.Components;

/// <summary>
///     Control whose value is all of the characters typed in the last tick.
/// </summary>
public sealed class CharControl : Control<string>
{
    private string textSinceLastTick = string.Empty;

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        Value = textSinceLastTick;
        textSinceLastTick = string.Empty;
    }

    /// <inheritdoc />
    private protected override void OnCharTyped(char character)
    {
        textSinceLastTick += character;
    }
}
