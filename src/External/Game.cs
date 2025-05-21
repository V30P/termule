namespace Termule;

using Termule.Internals;

public static class Game
{
    const int targetFPS = 60;

    static internal GameObject root = EngineObject.Spawn<GameObject>("Root");
    static readonly Thread gameThread = new Thread(GameLoop);

    static Game()
    {
        gameThread.Start();
    }

    static void GameLoop()
    {
        while (true)
        {
            root.Update();

            Thread.Sleep((int) (1000 * (1f / targetFPS)));
        }
    }
}