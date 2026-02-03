namespace Termule.Types;

using System.Text.Json.Serialization;

public readonly record struct Color
{
    public FullColor? Full { get; private init; }
    public BasicColor Basic { get; private init; }

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

    public static implicit operator Color((int r, int g, int b) t)
    {
        return new Color(t.r, t.g, t.b);
    }

    public static implicit operator Color(BasicColor b)
    {
        return new Color(b);
    }
}

public readonly record struct FullColor
{
    public readonly int R;
    public readonly int G;
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

public enum BasicColor
{
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
}
