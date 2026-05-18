using System.Text.Json.Serialization;

namespace Termule.Engine.Types;

/// <summary>
///     Full RGB color.
/// </summary>
public readonly struct FullColor
{
    [JsonConstructor]
    internal FullColor(float r, float g, float b)
    {
        ThrowIfOutOfRange(r, nameof(r));
        ThrowIfOutOfRange(g, nameof(g));
        ThrowIfOutOfRange(b, nameof(b));

        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    ///     Gets the blue component.
    /// </summary>
    public readonly float B { get; }

    /// <summary>
    ///     Gets the green component.
    /// </summary>
    public readonly float G { get; }

    /// <summary>
    ///     Gets the red component.
    /// </summary>
    public readonly float R { get; }

    /// <summary>
    ///     Creates a <see cref="FullColor"/> from a tuple representing its components.
    /// </summary>
    /// <param name="tuple">The RGB tuple to use.</param>
    public static implicit operator FullColor((float r, float g, float b) tuple)
    {
        return new FullColor(tuple.r, tuple.g, tuple.b);
    }

    /// <summary>
    ///     Determines whether two FullColor instances are equal.
    /// </summary>
    /// <param name="c1">The first FullColor to compare.</param>
    /// <param name="c2">The second FullColor to compare.</param>
    /// <returns>true if the colors are equal; otherwise, false.</returns>
    public static bool operator ==(FullColor c1, FullColor c2)
    {
        return c1.R == c2.R
            && c1.G == c2.G
            && c1.B == c2.B;
    }

    /// <summary>
    ///     Determines whether two FullColor instances are not equal.
    /// </summary>
    /// <param name="c1">The first FullColor to compare.</param>
    /// <param name="c2">The second FullColor to compare.</param>
    /// <returns>true if the colors are not equal; otherwise, false.</returns>
    public static bool operator !=(FullColor c1, FullColor c2)
    {
        return !(c1 == c2);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is FullColor fullColor && this == fullColor;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B);
    }

    private static void ThrowIfOutOfRange(float value, string name)
    {
        if (value is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                name,
                value,
                "Color RGB values must be between 0 and 1"
            );
        }
    }
}
