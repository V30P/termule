using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types;

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
        IConfigurableGame? game = Game.Create();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();
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
        IConfigurableGame? game = Game.Create();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new() { Layer = customLayer };
        game.Root.Add(renderer);

        renderer.Layer = null;

        Assert.Same(defaultLayer, renderer.Layer);
        Assert.Equal(1, customLayer.UnregisterCount);
        Assert.Equal(1, defaultLayer.RegisterCount);
    }

    [Fact]
    public void Register_ShouldUseConfiguredLayer_WhenLayerIsSetBeforeRegistration()
    {
        IConfigurableGame? game = Game.Create();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new() { Layer = customLayer };

        game.Root.Add(renderer);

        Assert.Same(customLayer, renderer.Layer);
        Assert.Equal(1, customLayer.RegisterCount);
        Assert.Equal(0, customLayer.UnregisterCount);
        Assert.Equal(0, defaultLayer.RegisterCount);
    }

    [Fact]
    public void Register_ShouldUseDefaultLayer_WhenNoLayerSet()
    {
        IConfigurableGame? game = Game.Create();
        FakeLayer layer = new();
        RenderSystem renderSystem = new() { Layers = [layer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();

        game.Root.Add(renderer);

        Assert.Same(renderSystem.DefaultLayer, renderer.Layer);
        Assert.Equal(1, layer.RegisterCount);
        Assert.Equal(0, layer.UnregisterCount);
    }

    [Fact]
    public void Unregister_ShouldUnregisterFromCurrentLayer()
    {
        IConfigurableGame? game = Game.Create();
        FakeLayer defaultLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();
        game.Root.Add(renderer);
    }
}