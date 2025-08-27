namespace Termule.Rendering;

internal class Frame : Image
{
    internal readonly Vector upperLeftBound;

    internal Frame() : base(RenderSystem.sizeX, RenderSystem.sizeY)
    {
        upperLeftBound = new Vector(-RenderSystem.sizeX, RenderSystem.sizeY) / 2;
    }

    public override string ToString()
    {
        string renderedFrame = "\u001b[?25l\u001b[0;0H"; // Hide the cursor, go to (0, 0)
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                renderedFrame += $"\u001b[{(int) this[x, y]}m "; // Switch to the correct background color and print a space
            }

            renderedFrame += "\u001b[E"; // Go to the start of the next line
        }
        renderedFrame += "\u001b[?25h"; //Unhide the cursor
        
        return renderedFrame;
    }
}