using System.Diagnostics;

namespace Termule.Editor;

internal class FrameDebugger : Debugger, IDisposable
{
    internal override DebuggerInfo info => new DebuggerInfo
    {
        name = "frame",
    };

    float period;

    Stopwatch frameStopWatch;
    Timer debugTimer;

    int framesSinceLastDebug;
    int maxFrameTimeMili;

    internal override void Start()
    {
        period = float.Parse(args[0]);
        int periodMili = (int) (period * 1000);

        frameStopWatch = new Stopwatch();
        game.root.Updated += MeasureFrame;

        debugTimer = new Timer(DebugFrames, null, periodMili, periodMili);
    }

    void MeasureFrame()
    {
        framesSinceLastDebug++;
        if (frameStopWatch.ElapsedMilliseconds > maxFrameTimeMili)
        {
            maxFrameTimeMili = (int) frameStopWatch.ElapsedMilliseconds;
        }

        frameStopWatch.Restart();
    }

    void DebugFrames(object _)
    {
        output.Write
        (
            "-----------------------\n" +
            $"Ran {framesSinceLastDebug} frames in {period} s\n" +
            $"FPS: {(int) (framesSinceLastDebug / period)}\n" +
            $"AVG: {(int) (period / framesSinceLastDebug * 1000)} ms Max: {maxFrameTimeMili} ms\n" +
            "-----------------------\n"
        );

        framesSinceLastDebug = maxFrameTimeMili = 0;
    }

    internal override void Stop() => Dispose();
    public void Dispose() => debugTimer.Dispose();
}