using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Exceptions;
using Termule.Tests.Utilities;

namespace Termule.Tests.Core;

public class TestComponent
{
    private static FakeComponent GetComponentOnGameObject()
    {
        GameObject gameObject = [];
        var component = new FakeComponent();
        gameObject.Add(component);

        return component;
    }

    [Fact]
    internal void Destroy_ShouldRemoveFromGameObject()
    {
        var component = GetComponentOnGameObject();
        component.Destroy();
        Assert.Null(component.GameObject);
    }

    [Fact]
    internal void Destroy_ShouldThrow_WhenGameObjectIsNull()
    {
        FakeComponent component = new();
        Assert.Throws<InvalidOperationException>(component.Destroy);
    }

    [Fact]
    internal void GetRequiredComponent_ShouldReturnExistingComponent()
    {
        var component = GetComponentOnGameObject();
        Assert.Equal(component, component.CallGetRequiredComponent<FakeComponent>());
    }

    [Fact]
    internal void GetRequiredComponent_ShouldThrow_WhenComponentMissing()
    {
        var component = GetComponentOnGameObject();
        Assert.Throws<MissingComponentException<Transform>>(component.CallGetRequiredComponent<Transform>);
    }
}