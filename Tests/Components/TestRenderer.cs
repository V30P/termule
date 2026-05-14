using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Components;

public class TestRenderer
{
    [Fact]
    public void Activate_WhenLayerIsSet_MovesToProvidedLayer()
    {
        Game game = new();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new() { Layer = customLayer };

        game.World.Add(renderer);

        Assert.Same(customLayer, renderer.Layer);
        Assert.Equal(1, customLayer.ActivateCount);
        Assert.Equal(0, customLayer.DeactivateCount);
        Assert.Equal(0, defaultLayer.ActivateCount);
    }

    [Fact]
    public void Activate_WhenNoLayerSet_MovesToDefaultLayer()
    {
        Game game = new();
        FakeLayer layer = new();
        RenderSystem renderSystem = new() { Layers = [layer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();

        game.World.Add(renderer);

        Assert.Same(renderSystem.DefaultLayer, renderer.Layer);
        Assert.Equal(1, layer.ActivateCount);
        Assert.Equal(0, layer.DeactivateCount);
    }

    [Fact]
    public void SettingLayer_ToNullWhenActivated_MovesRendererToDefaultLayer()
    {
        Game game = new();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new() { Layer = customLayer };
        game.World.Add(renderer);

        renderer.Layer = null;

        Assert.Same(defaultLayer, renderer.Layer);
        Assert.Equal(1, customLayer.DeactivateCount);
        Assert.Equal(1, defaultLayer.ActivateCount);
    }

    [Fact]
    public void SettingLayer_WhenActivated_MovesRendererBetweenLayers()
    {
        Game game = new();
        FakeLayer defaultLayer = new();
        FakeLayer customLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer, customLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();
        game.World.Add(renderer);

        Assert.Same(defaultLayer, renderer.Layer);
        Assert.Equal(1, defaultLayer.ActivateCount);

        renderer.Layer = customLayer;

        Assert.Same(customLayer, renderer.Layer);
        Assert.Equal(1, defaultLayer.DeactivateCount);
        Assert.Equal(1, customLayer.ActivateCount);
        Assert.Equal(0, customLayer.DeactivateCount);
    }

    [Fact]
    public void Deactivate_RemovesFromCurrentLayer()
    {
        Game game = new();
        FakeLayer defaultLayer = new();
        RenderSystem renderSystem = new() { Layers = [defaultLayer] };
        game.Systems.Install(renderSystem);

        FakeRenderer renderer = new();
        game.World.Add(renderer);
    }

    private sealed class FakeRenderer : Renderer
    {
        protected internal override void Render(FrameBuffer frame, Vector viewOrigin)
        {
        }
    }

    private sealed class FakeLayer() : Layer((r1, r2) => r1.ElementId.CompareTo(r2.ElementId))
    {
        public int ActivateCount { get; private set; }

        public int DeactivateCount { get; private set; }

        protected override void OnRendererAdded(Renderer renderer)
        {
            ActivateCount++;
        }

        protected override void OnRendererRemoved(Renderer renderer)
        {
            DeactivateCount++;
        }
    }
}
