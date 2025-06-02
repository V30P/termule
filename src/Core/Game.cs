namespace Termule;

using System.Diagnostics;
using Termule.Internals;
using Termule.Rendering;

public static class Game
{
    public static readonly GameObject root = EngineObject.Spawn<GameObject>("Root");

    public static float deltaTime { get; private set; }

    static Game()
    {
        Thread gameThread = new Thread(GameLoop)
        {
            Name = "Game"
        };
        gameThread.Start();
    }

    static void GameLoop()
    {
        Console.Clear();
        Stopwatch frameTimer = new Stopwatch();

        while (true)
        {
            root.Update();
            RenderSystem.Render(RenderSystem.GetFrame());

            deltaTime = frameTimer.ElapsedMilliseconds / 1000f;
            frameTimer.Restart();
        }
    }
}