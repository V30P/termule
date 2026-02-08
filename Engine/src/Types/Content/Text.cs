namespace Termule.Types;

/// <summary>
/// .
/// </summary>
public sealed class Text : Content
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Text"/> class.
    /// </summary>
    public Text()
    : base(0, 0)
    {
    }

    /// <summary>
    /// Gets or sets the string that this Content should show.
    /// </summary>
    public string Value
    {
        get;

        set
        {
            field = value;
            if (value == null)
            {
                this.Resize(0, 0);
                return;
            }

            string[] lines = field.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int width = lines.Length > 0 ? lines.Max(l => l.Length) : 0;
            int height = lines.Length;

            this.Resize(width, height);
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

                this.Cells[pos.X, pos.Y] = new() { Char = character };
#pragma warning disable SA1101 // Prefix local calls with this
                pos = pos with { X = pos.X + 1 };
#pragma warning restore SA1101 // Prefix local calls with this
            }
        }
    }

    /// <summary>
    /// Gets or sets the color that this Text's characters should be.
    /// </summary>
    public Color Color
    {
        get;

        set
        {
            field = value;
            this.ApplyColor();
        }
    }

    private void ApplyColor()
    {
        for (int x = 0; x < this.Size.X; x++)
        {
            for (int y = 0; y < this.Size.Y; y++)
            {
#pragma warning disable SA1101 // Prefix local calls with this
                this.Cells[x, y] = this.Cells[x, y] with { CharColor = this.Color };
#pragma warning restore SA1101 // Prefix local calls with this
            }
        }
    }
}