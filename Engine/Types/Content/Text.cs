namespace Termule.Engine.Types;

/// <summary>
///     Content implementation that represents text.
/// </summary>
public sealed class Text : IContent
{
    private Cell[][] lines = [];
    private VectorInt size = (0, 0);

    /// <summary>
    ///     Gets or sets the color of this text's glyphs.
    /// </summary>
    public Color Color
    {
        get;

        set
        {
            field = value;
            foreach (Cell[] line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i].GlyphColor = field;
                }
            }
        }
    }

    /// <summary>
    ///     Gets or sets the string of glyphs for this text.
    /// </summary>
    public string Value
    {
        get;

        set
        {
            if (field == value)
            {
                return;
            }

            field = value;
            if (string.IsNullOrEmpty(field))
            {
                lines = [];
                size = (0, 0);

                return;
            }

            string[] stringLines = field.Split('\n');
            size = new(size.X, stringLines.Length);

            lines = new Cell[stringLines.Length][];
            for (int i = 0; i < stringLines.Length; i++)
            {
                string line = stringLines[i];
                lines[i] = [.. line.Select(c => new Cell(default, c, Color))];

                if (line.Length > size.X)
                {
                    size = new(line.Length, size.Y);
                }
            }
        }
    }

    VectorInt IContent.Size => size;

    Cell IContent.this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= size.X)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= size.Y)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            // Returns blank spaces at the end of lines since content is rectangular
            return x >= lines[y].Length ? default : lines[y][x];
        }
    }
}
