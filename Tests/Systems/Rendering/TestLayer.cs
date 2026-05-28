using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestLayer
{
    [Fact]
    public void AddingAndRemovingRenderers_Dirties()
    {
        Renderer renderer = new FakeRenderer();
        FakeLayer layer = [renderer];

        Assert.True(layer.IsDirty);

        using IEnumerator<Renderer> enumerator = layer.GetEnumerator();
        layer.Remove(renderer);

        Assert.True(layer.IsDirty);
    }

    [Fact]
    public void GetEnumerator_ClearsDirtiness()
    {
        FakeLayer layer = [new FakeRenderer()];

        using IEnumerator<Renderer> enumerator = layer.GetEnumerator();

        Assert.False(layer.IsDirty);
    }

    [Fact]
    public void OnAddedAndOnRemoved_AreInvokedAccordingly()
    {
        Renderer renderer = new FakeRenderer();
        FakeLayer layer = [renderer];
        layer.Remove(renderer);

        Assert.True(layer.OnAddedCalled);
        Assert.True(layer.OnRemovedCalled);
    }

    [Fact]
    public void Renderers_AreSorted()
    {
        PriorityRenderer rendererA = new(2);
        PriorityRenderer rendererB = new(1);
        PriorityRenderer rendererC = new(3);
        PriorityLayer layer = [rendererB, rendererA, rendererC];

        Assert.Equal([rendererB, rendererA, rendererC], layer);
    }

    private sealed class FakeLayer() : Layer((_, _) => 0)
    {
        public bool OnAddedCalled { get; private set; }

        public bool OnRemovedCalled { get; private set; }

        public bool IsDirty => Dirty;

        protected override void OnRendererAdded(Renderer renderer)
        {
            base.OnRendererAdded(renderer);
            OnAddedCalled = true;
        }

        protected override void OnRendererRemoved(Renderer renderer)
        {
            base.OnRendererRemoved(renderer);
            OnRemovedCalled = true;
        }
    }

    private sealed class PriorityLayer()
        : Layer(
            (r1, r2) => ((PriorityRenderer) r1).Priority.CompareTo(((PriorityRenderer) r2).Priority)
        )
    {
    }

    private sealed class PriorityRenderer(int priority) : Renderer
    {
        public int Priority { get; } = priority;

        protected internal override void Render(IRenderTarget target, Vector viewOrigin)
        {
        }
    }
}
