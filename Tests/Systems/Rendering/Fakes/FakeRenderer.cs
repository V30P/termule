using Termule.Engine.Components;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Systems.RenderSystem.Fakes;

public class FakeRenderer : Renderer
{
    public FrameBuffer? CapturedFrame { get; private set; }
    public Vector? CapturedViewOrigin { get; private set; }

    protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
    {
        CapturedFrame = frame;
        CapturedViewOrigin = viewOrigin;
    }
}