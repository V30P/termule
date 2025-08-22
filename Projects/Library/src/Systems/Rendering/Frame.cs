namespace Termule.Rendering;

internal class Frame : Image
{
    internal readonly Vector upperLeftBound;

    internal Frame(RenderSystem renderSystem) : base(renderSystem.sizeX, renderSystem.sizeY)
    {
        upperLeftBound = new Vector(-renderSystem.sizeX, renderSystem.sizeY) / 2;
    }

    public override string ToString()
    {
        string renderedFrame = "\u001b[0;0H"; // Go to (0, 0)
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                renderedFrame += $"\u001b[{(int) this[x, y]}m "; // Display a ' ' with provided background color
            }

            renderedFrame += "\u001b[E"; // Go to the start of the next line
        }

        return renderedFrame;
    }
}