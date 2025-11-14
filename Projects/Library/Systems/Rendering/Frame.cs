namespace Termule.Rendering;

public class Frame
{
    public readonly VectorInt Size;
    public readonly Image Image; // Don't modify this directly

    internal readonly List<Renderer>[,] blame;
    internal readonly Dictionary<Renderer, List<VectorInt>> contributions;

    internal Frame(int sizeX, int sizeY)
    {
        Size = (sizeX, sizeY);
        Image = new Image(sizeX, sizeY);

        blame = new List<Renderer>[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                blame[x, y] = [];
            }
        }

        contributions = [];
    }

    // Always deal with frames through Contribute() unless you know what you are doing
    internal void Contribute(Color color, int x, int y)
    {
        Image.Color[x, y] = color;
    }

    internal void Contribute(Color color, int x, int y, Renderer renderer)
    {
        if (color != Color.Clear)
        {
            Contribute(color, x, y);
            RecordContribution(renderer, x, y);
        }
    }

    internal void Contribute(char character, int x, int y)
    {
        Image.Text[x, y] = character;
    }

    internal void Contribute(char character, int x, int y, Renderer renderer)
    {
        Contribute(character, x, y);
        RecordContribution(renderer, x, y);
    }

    private void RecordContribution(Renderer renderer, int x, int y)
    {
        blame[x, y].Add(renderer);

        if (!contributions.TryGetValue(renderer, out List<VectorInt> contributionPositions))
        {
            contributionPositions = [];
            contributions.Add(renderer, contributionPositions);
        }
        contributionPositions.Add((x, y));
    }

    public Color GetColor(int x, int y)
    {
        return Image.Color[x, y];
    }

    public char GetText(int x, int y)
    {
        return Image.Text[x, y];
    }

    internal bool EqualsAt(Frame f, int x, int y)
    {
        return x < Size.X
            && x < f.Size.X
            && y < Size.Y
            && y < f.Size.Y
            && Image.Color[x, y] == f.GetColor(x, y)
            && Image.Text[x, y] == f.GetText(x, y);
    }
}