using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Components;

public class TestRenderer
{
    private class FakeLayer() : Layer((r1, r2) => r1.ElementId.CompareTo(r2.ElementId))
    {
        public int RegisterCount { get; private set; }
        public int UnregisterCount { get; private set; }

        protected override void OnRendererRegistered(Renderer renderer)
        {
            RegisterCount++;
        }

        protected override void OnRendererUnregistered(Renderer renderer)
        {
            UnregisterCount++;
        }
    }

    private class FakeRenderer : Renderer
    {
        public int RenderCount { get; private set; }

        protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
        {
            RenderCount++;
        }
    }

    [Fact]
    public void LayerSetter_ShouldMoveRendererBetweenLayers_WhenRegistered()
    {
        var game = Game.Create();
        var defaultLayer = new FakeLayer();
        var customLayer = new FakeLayer();
        var renderSystem = new RenderSystem { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        var renderer = new FakeRenderer();
        game.Root.Add(renderer);

        Assert.Same(defaultLayer, renderer.Layer);
        Assert.Equal(1, defaultLayer.RegisterCount);

        renderer.Layer = customLayer;

        Assert.Same(customLayer, renderer.Layer);
        Assert.Equal(1, defaultLayer.UnregisterCount);
        Assert.Equal(1, customLayer.RegisterCount);
        Assert.Equal(0, customLayer.UnregisterCount);
    }

    [Fact]
    public void LayerSetter_ShouldMoveRendererToDefaultLayer_WhenSetToNullWhileRegistered()
    {
        var game = Game.Create();
        var defaultLayer = new FakeLayer();
        var customLayer = new FakeLayer();
        var renderSystem = new RenderSystem { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        var renderer = new FakeRenderer { Layer = customLayer };
        game.Root.Add(renderer);

        renderer.Layer = null;

        Assert.Same(defaultLayer, renderer.Layer);
        Assert.Equal(1, customLayer.UnregisterCount);
        Assert.Equal(1, defaultLayer.RegisterCount);
    }

    [Fact]
    public void Register_ShouldUseConfiguredLayer_WhenLayerIsSetBeforeRegistration()
    {
        var game = Game.Create();
        var defaultLayer = new FakeLayer();
        var customLayer = new FakeLayer();
        var renderSystem = new RenderSystem { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        var renderer = new FakeRenderer { Layer = customLayer };

        game.Root.Add(renderer);

        Assert.Same(customLayer, renderer.Layer);
        Assert.Equal(1, customLayer.RegisterCount);
        Assert.Equal(0, customLayer.UnregisterCount);
        Assert.Equal(0, defaultLayer.RegisterCount);
    }

    [Fact]
    public void Register_ShouldUseDefaultLayer_WhenNoLayerSet()
    {
        var game = Game.Create();
        var layer = new FakeLayer();
        var renderSystem = new RenderSystem { Layers = [layer] };
        game.Systems.Install(renderSystem);

        var renderer = new FakeRenderer();

        game.Root.Add(renderer);

        Assert.Same(renderSystem.DefaultLayer, renderer.Layer);
        Assert.Equal(1, layer.RegisterCount);
        Assert.Equal(0, layer.UnregisterCount);
    }

    [Fact]
    public void Unregister_ShouldUnregisterFromCurrentLayer()
    {
        var game = Game.Create();
        var defaultLayer = new FakeLayer();
        var renderSystem = new RenderSystem { Layers = [defaultLayer] };
        game.Systems.Install(renderSystem);

        var renderer = new FakeRenderer();
        game.Root.Add(renderer);
    }
}