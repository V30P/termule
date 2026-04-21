namespace Termule.Engine.Types;

/// <summary>
///     Content that represents textual content.
/// </summary>
public sealed class Text : IContent
{
    private Cell[][] lines = [];
    private VectorInt size = (0, 0);

    /// <summary>
    ///     Gets or sets the color of this text's characters.
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
                    line[i].CharColor = field;
                }
            }
        }
    }

    /// <summary>
    ///     Gets or sets the string of characters for this text.
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
            size = size with { Y = stringLines.Length };

            lines = new Cell[stringLines.Length][];
            for (int i = 0; i < stringLines.Length; i++)
            {
                string line = stringLines[i];
                lines[i] = line.Select(c => new Cell(default, c, Color)).ToArray();

                if (line.Length > size.X)
                {
                    size = size with { X = line.Length };
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
                throw new IndexOutOfRangeException("X position falls outside of content.");
            }

            if (y < 0 || y >= size.Y)
            {
                throw new IndexOutOfRangeException("Y position falls outside of content.");
            }

            // Returns blank spaces at the end of lines since content is rectangular
            if (x >= lines[y].Length)
            {
                return default;
            }

            return lines[y][x];
        }
    }
}