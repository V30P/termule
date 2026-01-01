using Termule;
using Termule.Rendering;
using Termule.Resources;

public static class Program
{
    [Init]
    public static void Init()
    {
        Game.Add
        (
            new GameObject()
            {
                new Transform(),
                new Camera() { MatchDisplaySize = true }
            },
            new GameObject()
            {
                new Transform(),
                new ContentRenderer<Content>()
                {
                    Content = Resources.Load<Content>("Logo"),
                    Centered = true
                }
            }
        );
    }
}