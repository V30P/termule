namespace Termule.Engine.Types;

/// <summary>
///     Content that represents textual content.
/// </summary>
public sealed class Text : Content
{
    /// <summary>
    ///     Gets or sets the string displayed by this Text.
    /// </summary>
    public string Value
    {
        get;

        set
        {
            field = value;
            if (value == null)
            {
                Resize(0, 0);
                return;
            }

            string[] lines = field.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int width = lines.Length > 0 ? lines.Max(l => l.Length) : 0;
            int height = lines.Length;

            Resize(width, height);
            VectorInt pos = (0, 0);
            foreach (char character in field)
            {
                if (character == '\n')
                {
                    pos = (0, pos.Y + 1);
                    continue;
                }

                if (char.IsControl(character))
                {
                    continue;
                }

                Cells[pos.X, pos.Y] = new Cell { Char = character };
                pos = pos with { X = pos.X + 1 };
            }
        }
    }

    /// <summary>
    ///     Gets or sets the color for this text's characters.
    /// </summary>
    public Color Color
    {
        get;

        set
        {
            field = value;
            ApplyColor();
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text" /> class.
    /// </summary>
    public Text()
        : base(0, 0)
    {
    }

    private void ApplyColor()
    {
        for (int x = 0; x < Size.X; x++)
        for (int y = 0; y < Size.Y; y++)
        {
#pragma warning disable SA1101
            // Prefix local calls with this
            Cells[x, y] = Cells[x, y] with { CharColor = Color };
#pragma warning restore SA1101
            // Prefix local calls with this
        }
    }
}