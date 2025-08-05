using System.Diagnostics;

namespace Termule.Editor;

internal class FrameDebugger : Debugger, IDisposable
{
    internal override DebuggerInfo info => new DebuggerInfo
    {
        name = "frame",
    };

    float period;

    FrameMeasurer frameMeasurer;
    Timer debugTimer;

    int framesSinceLastDebug;
    float maxFrameTime;

    internal override void Start()
    {
        period = float.Parse(args[0]);
        int periodMili = (int) (period * 1000);

        frameMeasurer = game.Get<FrameMeasurer>();
        if (frameMeasurer == null)
        {
            frameMeasurer = new FrameMeasurer();
            game.Add(frameMeasurer);
        }
        frameMeasurer.FrameMeasured += RecordFrame;

        debugTimer = new Timer(DebugFrames, null, periodMili, periodMili);
    }

    void RecordFrame(float deltaTime)
    {
        framesSinceLastDebug++;
        if (deltaTime > maxFrameTime)
        {
            maxFrameTime = deltaTime;
        }
    }

    void DebugFrames(object _)
    {
        output.Write
        (
            "-----------------------\n" +
            $"Ran {framesSinceLastDebug} frames in {period} s\n" +
            $"FPS: {(int) (framesSinceLastDebug / period)}\n" +
            $"AVG: {(int) (period / framesSinceLastDebug * 1000)} ms Max: {(int) (maxFrameTime * 1000)} ms\n" +
            "-----------------------\n"
        );

        maxFrameTime = framesSinceLastDebug = 0;
    }

    internal override void Stop() => Dispose();
    public void Dispose()
    {
        frameMeasurer.Destroy();
        debugTimer.Dispose();
    }

    class FrameMeasurer : Component
    {
        internal event Action<float> FrameMeasured;

        internal FrameMeasurer()
        {
            Ticked += () => FrameMeasured?.Invoke(game.deltaTime);
        }
    }
}