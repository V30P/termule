using System.Text.Json.Serialization;

namespace Termule.Engine.Types;

/// <summary>
///     Full RGB color.
/// </summary>
public readonly record struct FullColor
{
    /// <summary>
    ///     Blue component.
    /// </summary>
    public readonly int B;

    /// <summary>
    ///     Green component.
    /// </summary>
    public readonly int G;

    /// <summary>
    ///     Red component.
    /// </summary>
    public readonly int R;

    [JsonConstructor]
    internal FullColor(int r, int g, int b)
    {
        ThrowIfOutOfRange(r, nameof(r));
        ThrowIfOutOfRange(g, nameof(g));
        ThrowIfOutOfRange(b, nameof(b));

        R = r;
        G = g;
        B = b;
    }

    private static void ThrowIfOutOfRange(int value, string name)
    {
        if (value is < 0 or > 255)
        {
            throw new ArgumentOutOfRangeException(name, value, "Color RGB values must be between 0 and 255");
        }
    }
}