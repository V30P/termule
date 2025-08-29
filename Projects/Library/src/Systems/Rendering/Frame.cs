namespace Termule.Rendering;

public class Frame
{

    public readonly int sizeX, sizeY;

    internal readonly Image image;
    internal readonly char[,] text;

    internal readonly List<Renderer>[,] blame;
    internal readonly Dictionary<Renderer, List<(int x, int y)>> contributions;

    internal Frame(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        image = new Image(sizeX, sizeY);
        text = new char[sizeX, sizeY];

        blame = new List<Renderer>[sizeX, sizeY];
        contributions = [];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                text[x, y] = ' ';
                blame[x, y] = []; 
            }    
        }    
    }

    internal void Contribute(Color color, int x, int y, Renderer renderer)
    {
        image[x, y] = color;
        RecordContribution(renderer, x, y);
    }

    internal void Contribute(char character, int x, int y, Renderer renderer)
    {
        text[x, y] = character;
        RecordContribution(renderer, x, y);
    }

    void RecordContribution(Renderer renderer, int x, int y)
    {
        blame[x, y].Add(renderer);

        if (!contributions.TryGetValue(renderer, out List<(int x, int y)> contributionPositions))
        {
            contributionPositions = [];
            contributions.Add(renderer, contributionPositions);
        }
        contributionPositions.Add((x, y));
    }
}