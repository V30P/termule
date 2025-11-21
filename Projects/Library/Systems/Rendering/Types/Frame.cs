namespace Termule.Rendering;

public class Frame : Image
{
    internal readonly List<Renderer>[,] Blame;
    internal readonly Dictionary<Renderer, List<VectorInt>> Contributions;

    internal Frame(VectorInt size, Color background = Color.Black) : base(size.X, size.Y)
    {
        Blame = new List<Renderer>[size.X, size.Y];
        for (int x = 0; x < size.X; x++)
        {
            for (int y = 0; y < size.Y; y++)
            {
                Colors[x, y] = background;
                Blame[x, y] = [];
            }
        }

        Contributions = [];
    }

    // Always render to frames with Contribute() unless you know what you are doing
    public void Contribute(VectorInt pos, Renderer renderer, Color? color)
    {
        if (color != null)
        {
            Colors[pos.X, pos.Y] = color;
            Text[pos.X, pos.Y] = ' '; // Cover up existing colors
            Credit(renderer, pos);
        }
    }

    public void Contribute(VectorInt pos, Renderer renderer, char? character)
    {
        if (character != null)
        {
            Text[pos.X, pos.Y] = character;
            Credit(renderer, pos);
        }
    }

    // Enforces the proper order for contributing both color and character at one time
    public void Contribute(VectorInt pos, Renderer renderer, Color? color, char? character)
    {
        Contribute(pos, renderer, color);
        Contribute(pos, renderer, character);
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
        return pos.X < Size.X
            && pos.X < f.Size.X
            && pos.Y < Size.Y
            && pos.Y < f.Size.Y
            && Colors[pos.X, pos.Y] == f.Colors[pos.X, pos.Y]
            && Text[pos.X, pos.Y] == f.Text[pos.X, pos.Y];
    }
}