namespace Termule.Rendering;

public class Frame
{
    public readonly VectorInt size;
    public readonly Image image; // Don't modify this directly
    
    internal readonly List<Renderer>[,] blame;
    internal readonly Dictionary<Renderer, List<VectorInt>> contributions;

    internal Frame(int sizeX, int sizeY)
    {
        size = (sizeX, sizeY);
        image = new Image(sizeX, sizeY);

        blame = new List<Renderer>[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++) for (int y = 0; y < sizeY; y++) blame[x, y] = [];
        contributions = [];
    }

    internal void Contribute(Color color, int x, int y) => image.color[x, y] = color;
    internal void Contribute(Color color, int x, int y, Renderer renderer)
    {
        if (color != Color.Clear)
        {
            Contribute(color, x, y);
            RecordContribution(renderer, x, y);
        }
    }

    internal void Contribute(char character, int x, int y) => image.text[x, y] = character;
    internal void Contribute(char character, int x, int y, Renderer renderer)
    {
        Contribute(character, x, y);
        RecordContribution(renderer, x, y);
    }

    void RecordContribution(Renderer renderer, int x, int y)
    {
        blame[x, y].Add(renderer);

        if (!contributions.TryGetValue(renderer, out List<VectorInt> contributionPositions))
        {
            contributionPositions = [];
            contributions.Add(renderer, contributionPositions);
        }
        contributionPositions.Add((x, y));
    }

    public Color GetColor(int x, int y) => image.color[x, y];
    public char GetText(int x, int y) => image.text[x, y];

    internal bool EqualsAt(Frame f, int x, int y) =>
    x < size.x && x < f.size.x && y < size.y && y < f.size.y
    && image.color[x, y] == f.GetColor(x, y) && image.text[x, y] == f.GetText(x, y);
}