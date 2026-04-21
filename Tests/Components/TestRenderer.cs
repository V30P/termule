using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Components;

public class TestRenderer
{
    private class FakeLayer() : Layer((r1, r2) => r1.ElementId.CompareTo(r2.ElementId))
    {
        public int RegisterCount { get; private set; }
        public int UnregisterCount { get; private set; }

        protected override void OnRendererAdded(Renderer renderer)
        {
            RegisterCount++;
        }

        protected override void OnRendererRemoved(Renderer renderer)
        {
            UnregisterCount++;
        }
    }

    private class FakeRenderer : Renderer
    {
        protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
        {
        }
    }



    [Fact]
    public void Register_WhenNoLayerSet_MovesToDefaultLayer()
    {
        IConfigurableGame game = Game.Create();
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
    public void Register_WhenLayerIsSet_MovesToProvidedLayer()
    {
        IConfigurableGame game = Game.Create();
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
    public void SettingLayer_WhenRegistered_MovesRendererBetweenLayers()
    {
        IConfigurableGame game = Game.Create();
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
    public void SettingLayer_ToNullWhenRegistered_MovesRendererToDefaultLayer()
    {
        IConfigurableGame game = Game.Create();
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
    public void Unregister_RemovesFromCurrentLayer()
    {
        IConfigurableGame game = Game.Create();
        FakeLayer defaultLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();
        game.Root.Add(renderer);
    }
}