using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Common;

internal class FakeRenderer : Renderer
{
    public IRenderTarget CapturedTarget { get; private set; }

    public Vector? CapturedViewOrigin { get; private set; }

    protected internal override void Render(IRenderTarget target, Vector viewOrigin)
    {
        CapturedTarget = target;
        CapturedViewOrigin = viewOrigin;
    }
}
