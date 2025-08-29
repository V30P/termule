namespace Termule.Rendering;

public abstract class Renderer : Component
{
    public Renderer[] overlappers
    {
        get
        {
            List<Renderer> overlappers = [];
            if (RenderSystem.frame.contributions.TryGetValue(this, out List<(int x, int y)> contributions))
            {
                foreach ((int x, int y) contributionPosition in contributions)
                {
                    overlappers
                    .AddRange(RenderSystem.frame.blame[contributionPosition.x, contributionPosition.y]
                    .Where(renderer => renderer != this));
                }
            }

            return [.. overlappers];
        }
    }

    public Renderer()
    {
        Rooted += () => RenderSystem.renderers.Add(this); ;
        Destroyed += () => RenderSystem.renderers.Remove(this);
    }

    internal abstract void Render(Frame frame, Vector viewOrigin, Vector viewSize);
}