namespace Termule.Types;

public sealed class Text : Content
{
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

                Cells[pos.X, pos.Y] = new() { Char = character };
                pos = pos with { X = pos.X + 1 };
            }
        }
    }

    public Color Color
    {
        get;

        set
        {
            field = value;
            ApplyColor();
        }
    }

    public Text() { }

    private void ApplyColor()
    {
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Cells[x, y] = Cells[x, y] with { CharColor = Color };
            }
        }
    }
}