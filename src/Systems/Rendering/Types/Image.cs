namespace Termule.Rendering;

public class Image
{
    public readonly VectorInt Size;

    public Content<Color?> Colors { get => _colors; set => _colors.Set(value); }
    private readonly Content<Color?> _colors;
    public Content<char?> Text { get => _text; set => _text.Set(value); }
    private readonly Content<char?> _text;

    public Image(int x, int y)
    {
        Size = (x, y);
        _colors = new(Size.X, Size.Y);
        _text = new(Size.X, Size.Y);

        _colors.Changed += OnColorChanged;
        _text.Changed += OnTextChanged;
    }

    public Image(Image image) : this(image.Size.X, image.Size.Y)
    {
        Colors = image.Colors;
        Text = image.Text;
    }

    // Make sure colors always have a character
    private void OnColorChanged(VectorInt pos, Color? color)
    {
        if (color != null && Text[pos.X, pos.Y] == null)
        {
            Text[pos.X, pos.Y] = ' ';
        }
    }

    private void OnTextChanged(VectorInt pos, char? character)
    {
        if (character == null && Colors[pos.X, pos.Y] != null)
        {
            Text[pos.X, pos.Y] = ' ';
        }
    }

    public sealed class Content<T>(int x, int y)
    {
        private readonly T[,] _values = new T[x, y];
        public event Action<VectorInt, T> Changed;

        public T this[int x, int y]
        {
            get => _values[x, y];

            set
            {
                _values[x, y] = value;
                Changed?.Invoke((x, y), value);
            }
        }

        // These two methods are needed so content can be set with an array
        // This intentionally reads "sideways" so hardcoded content values have proper orientation
        public static implicit operator Content<T>(T[,] values)
        {
            Content<T> content = new(values.GetLength(1), values.GetLength(0));
            for (int x = 0; x < values.GetLength(0); x++)
            {
                for (int y = 0; y < values.GetLength(1); y++)
                {
                    content[y, x] = values[x, y];
                }
            }

            return content;
        }

        internal void Set(Content<T> content)
        {
            for (int x = 0; x < _values.GetLength(0); x++)
            {
                for (int y = 0; y < _values.GetLength(1); y++)
                {
                    _values[x, y] = content._values[x, y];
                }
            }
        }
    }
}