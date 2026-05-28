using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestRenderSystem
{
    [Fact]
    public void DefaultLayer_IsTheFirstLayer()
    {
        Layer layer = new SimpleLayer();
        RenderSystem renderSystem = new() { Layers = [layer, new SimpleLayer()] };
        Assert.Same(layer, renderSystem.DefaultLayer);
    }

    [Fact]
    public void Layers_DefaultsToASimpleLayer()
    {
        RenderSystem renderSystem = new();

        _ = Assert.Single(renderSystem.Layers);
        _ = Assert.IsType<SimpleLayer>(renderSystem.Layers[0]);
    }

    [Fact]
    public void Render_CallsRenderersInCorrectOrder()
    {
        List<OrderedRenderer> renderTracker = [];

        OrderedRenderer rendererA = new(renderTracker);
        OrderedRenderer rendererB = new(renderTracker);

        RenderSystem renderSystem = new()
        {
            Layers =
            [
                new SimpleLayer { rendererA },
                new SimpleLayer { rendererB }
            ]
        };

        renderSystem.Render(new FrameBuffer(0, 0), (0, 0));
        Assert.Equal([rendererA, rendererB], renderTracker);
    }

    [Fact]
    public void Render_PassesArgumentsToRenderers()
    {
        FakeRenderer renderer = new();
        RenderSystem renderSystem = new();
        renderSystem.DefaultLayer.Add(renderer);

        FrameBuffer frame = new(0, 0);
        renderer.Render(frame, (10, 5));

        Assert.Same(frame, renderer.CapturedTarget);
        Assert.Equal((10, 5), renderer.CapturedViewOrigin);
    }

    [Fact]
    public void SettingLayers_ToNullOrEmpty_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() =>
            new RenderSystem { Layers = null }
        );

        _ = Assert.Throws<ArgumentException>(() =>
            new RenderSystem { Layers = [] }
        );
    }

    private sealed class OrderedRenderer(List<OrderedRenderer> renderTracker) : FakeRenderer
    {
        protected internal override void Render(IRenderTarget target, Vector viewOrigin)
        {
            base.Render(target, viewOrigin);

            renderTracker.Add(this);
        }
    }
}
