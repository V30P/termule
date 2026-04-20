using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Systems.Rendering;

public class TestRenderSystem
{
    private class OrderedRenderer(List<OrderedRenderer> renderTracker) : FakeRenderer
    {
        protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
        {
            base.Render(frame, viewOrigin);

            renderTracker.Add(this);
        }
    }

    [Fact]
    public void DefaultLayer_ShouldBeTheFirstLayer()
    {
        Layer layer = new SimpleLayer();
        RenderSystem renderSystem = new() { Layers = [layer, new SimpleLayer()] };
        Assert.Same(layer, renderSystem.DefaultLayer);
    }

    [Fact]
    public void Layers_ShouldDefaultToASimpleLayer()
    {
        RenderSystem renderSystem = new();

        Assert.Single(renderSystem.Layers);
        Assert.IsType<SimpleLayer>(renderSystem.Layers[0]);
    }

    [Fact]
    public void Render_ShouldCallRenderersInCorrectOrder()
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

        renderSystem.Render((0, 0), new FrameBuffer(0, 0));
        Assert.Equal([rendererA, rendererB], renderTracker);
    }

    [Fact]
    public void Render_ShouldPassArgumentsToRenderers()
    {
        FakeRenderer renderer = new();
        RenderSystem renderSystem = new();
        renderSystem.DefaultLayer.Add(renderer);

        FrameBuffer frame = new(0, 0);
        renderer.Render(frame, (10, 5));

        Assert.Same(frame, renderer.CapturedFrame);
        Assert.Equal((10, 5), renderer.CapturedViewOrigin);
    }

    [Fact]
    public void SettingLayers_ToNullOrEmpty_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new RenderSystem { Layers = null }
        );

        Assert.Throws<ArgumentException>(() =>
            new RenderSystem { Layers = [] }
        );
    }
}