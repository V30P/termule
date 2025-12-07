namespace Termule.Rendering;

internal sealed class Frame
{
    internal Image Image => new(_image);
    internal VectorInt Size => _image.Size;
    private readonly Image _image;

    internal readonly List<Renderer>[,] Blame;
    internal readonly Dictionary<Renderer, List<VectorInt>> Contributions;

    internal Frame(VectorInt size, Color? background = null)
    {
        _image = new(size.X, size.Y);
        Blame = new List<Renderer>[size.X, size.Y];
        Contributions = [];

        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                _image.Color[x, y] = background;
                Blame[x, y] = [];
            }
        }
    }

    public void Contribute(VectorInt pos, Renderer renderer, Color? color)
    {
        if (color != null)
        {
            // Cover up existing characters
            _image.Text[pos.X, pos.Y] = null;
            _image.TextColor[pos.X, pos.Y] = null;

            _image.Color[pos.X, pos.Y] = color;
            Credit(renderer, pos);
        }
    }

    public void Contribute(VectorInt pos, Renderer renderer, char? character, Color? characterColor = null)
    {
        if (character != null)
        {
            _image.Text[pos.X, pos.Y] = character;
            _image.TextColor[pos.X, pos.Y] = characterColor;
            Credit(renderer, pos);
        }
    }

    // Enforces the proper order for contributing both color and character
    public void Contribute(VectorInt pos, Renderer renderer, Color? color, char? character, Color? characterColor = null)
    {
        Contribute(pos, renderer, color);
        Contribute(pos, renderer, character, characterColor);
    }

    private void Credit(Renderer renderer, VectorInt pos)
    {
        Blame[pos.X, pos.Y].Add(renderer);

        if (!Contributions.TryGetValue(renderer, out List<VectorInt> contributionPositions))
        {
            contributionPositions = [];
            Contributions.Add(renderer, contributionPositions);
        }
        contributionPositions.Add((pos.X, pos.Y));
    }

    internal bool EqualsAt(Frame f, VectorInt pos)
    {
        return pos.X < Size.X && pos.X < f.Size.X
            && pos.Y < Size.Y && pos.Y < Size.Y
            && _image.Color[pos.X, pos.Y] == f._image.Color[pos.X, pos.Y]
            && _image.Text[pos.X, pos.Y] == f._image.Text[pos.X, pos.Y]
            && _image.TextColor[pos.X, pos.Y] == f._image.TextColor[pos.X, pos.Y];
    }
}