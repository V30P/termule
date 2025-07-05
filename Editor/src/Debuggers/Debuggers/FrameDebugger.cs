using System.Diagnostics;

namespace Termule.Editor;

internal class FrameDebugger : Debugger, IDisposable
{
    internal override DebuggerInfo info => new DebuggerInfo
    {
        name = "frame",
    };

    float period;

    Timer debugTimer;
    FrameMeasurer frameMeasurer;

    int framesSinceLastDebug;
    int maxFrameTimeMili;

    static int measurersCreated; //Used to make sure two measurers don't get the same name

    internal override void Debug()
    {
        period = float.Parse(args[0]);
        int periodMili = (int) (period * 1000);

        debugTimer = new Timer(DebugFrames, null, periodMili, periodMili);
        frameMeasurer = Component.Spawn<FrameMeasurer>(game, $"FrameMeasurer{++measurersCreated}");
        frameMeasurer.debugger = this;
    }

    void DebugFrames(object _)
    {
        writer.Write
        (
            "-----------------------\n" +
            $"Ran {framesSinceLastDebug} frames in {period} s\n" +
            $"FPS: {(int)(framesSinceLastDebug / period)}\n" +
            $"AVG: {(int)(period / framesSinceLastDebug * 1000)} ms Max: {maxFrameTimeMili} ms\n" +
            "-----------------------\n"
        );

        framesSinceLastDebug = maxFrameTimeMili = 0;
    }

    internal override void Stop()
    {
        frameMeasurer.Destroy();
        Dispose();
    }

    public void Dispose()
    {
        debugTimer.Dispose();
    }

    class FrameMeasurer : Behavior
    {
        internal FrameDebugger debugger;
        readonly Stopwatch stopwatch;

        public FrameMeasurer()
        {
            Updated += MeasureFrame;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        private void MeasureFrame()
        {
            debugger.framesSinceLastDebug++;
            if (stopwatch.Elapsed.Milliseconds > debugger.maxFrameTimeMili)
            {
                debugger.maxFrameTimeMili = (int) stopwatch.ElapsedMilliseconds;
            }

            stopwatch.Restart();
        }
    }
}