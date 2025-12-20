using Termule;
using Termule.Rendering;
using Termule.Resources;

public static class Program
{
    [Init]
    public static void Init()
    {
        Content logo = Resources.Load<Content>("Logo");

        Game.Add
        (
            new GameObject()
            {
                new Transform(),
                new ContentRenderer<Content>()
                {
                    Content = logo,
                    Centered = true
                }
            },
            new GameObject()
            {
                new Transform(),
                new Camera() { MatchDisplaySize = true }
            }
        );
    }
}