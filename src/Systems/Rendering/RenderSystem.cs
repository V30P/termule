namespace Termule.Rendering;

public static class RenderSystem
{
    internal static readonly Layer DefaultLayer = new();

    public static OrderedSet<Layer> Layers
    {
        get => [.. _layers.Where(layer => layer != DefaultLayer)];
        set => _layers = [.. value, DefaultLayer];
    }
    private static OrderedSet<Layer> _layers = [DefaultLayer];

    internal static Frame Render(Vector viewOrigin, VectorInt viewSize, Color background = Color.Black)
    {
        Frame frame = new((viewSize.X, viewSize.Y), background);
        foreach (Layer layer in _layers)
        {
            foreach (Renderer renderer in layer)
            {
                renderer.Render(frame, viewOrigin);
            }
        }

        return frame;
    }
}