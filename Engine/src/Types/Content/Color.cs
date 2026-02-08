namespace Termule.Types;

using System.Text.Json.Serialization;

/// <summary>
/// A Color that is able to be rendered by Terminals.
/// </summary>
public readonly record struct Color
{
    /// <summary>
    /// Gets the full RGB color if one was set.
    /// </summary>
    public FullColor? Full { get; private init; }

    /// <summary>
    /// Gets the basic preset color if one was set.
    /// </summary>
    public BasicColor Basic { get; private init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> class.
    /// </summary>
    public Color()
        : this(BasicColor.Default)
    {
    }

    [JsonConstructor]
#pragma warning disable IDE0051
    private Color(FullColor? full, BasicColor basic)
#pragma warning restore IDE0051
    {
        this.Full = full;
        this.Basic = basic;
    }

    private Color(int r, int g, int b)
    {
        this.Full = new(r, g, b);
    }

    private Color(BasicColor baseColor)
    {
        this.Basic = baseColor;
    }

    /// <summary>
    /// Creates a color from an RGB tuple.
    /// </summary>
    /// <param name="t">The RGB values to use.</param>
    public static implicit operator Color((int r, int g, int b) t)
    {
        return new Color(t.r, t.g, t.b);
    }

    /// <summary>
    /// Creates a Color from a BasicColor.
    /// </summary>
    /// <param name="b">The BasicColor to use.</param>
    public static implicit operator Color(BasicColor b)
    {
        return new Color(b);
    }
}

/// <summary>
/// A full RGB color.
/// </summary>
public readonly record struct FullColor
{
    /// <summary>
    /// The red component.
    /// </summary>
    public readonly int R;

    /// <summary>
    /// The green component.
    /// </summary>
    public readonly int G;

    /// <summary>
    /// The blue component.
    /// </summary>
    public readonly int B;

    [JsonConstructor]
    internal FullColor(int r, int g, int b)
    {
        ThrowIfOutOfRange(r, nameof(r));
        ThrowIfOutOfRange(g, nameof(g));
        ThrowIfOutOfRange(b, nameof(b));

        this.R = r;
        this.G = g;
        this.B = b;
    }

    private static void ThrowIfOutOfRange(int value, string name)
    {
        if (value is < 0 or > 255)
        {
            throw new ArgumentOutOfRangeException(name, value, "Color RGB values must be between 0 and 255");
        }
    }
}

/// <summary>
/// The 16 basic color values supported on even older terminals.
/// </summary>
public enum BasicColor
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Default,
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    BrightBlack,
    BrightRed,
    BrightGreen,
    BrightYellow,
    BrightBlue,
    BrightMagenta,
    BrightCyan,
    BrightWhite,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
