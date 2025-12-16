namespace Termule.Rendering;

public sealed class Text : Content
{
    public string Value
    {
        get => _value;

        set
        {
            if (value == null)
            {
                Resize(0, 0);
            }

            string[] lines = value.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int width = lines.Max(l => l.Length);
            int height = lines.Length;

            Resize(width, height);
            VectorInt pos = (0, 0);
            foreach (char character in value)
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
                pos.X++;
            }

            _value = value;
        }
    }
    private string _value;

    public Color Color
    {
        get => _color;

        set
        {
            _color = value;
            ApplyColor();
        }
    }
    private Color _color;

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