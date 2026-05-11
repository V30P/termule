using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class FakeRenderer : Renderer
{
    public FrameBuffer CapturedFrame { get; private set; }
    public Vector? CapturedViewOrigin { get; private set; }

    protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
    {
        CapturedFrame = frame;
        CapturedViewOrigin = viewOrigin;
    }
}