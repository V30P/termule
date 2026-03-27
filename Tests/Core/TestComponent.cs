using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Exceptions;
using Termule.Tests.Core.Fakes;

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
    public void Destroy_ShouldRemoveFromGameObject()
    {
        var component = GetComponentOnGameObject();
        component.Destroy();
        Assert.Null(component.GameObject);
    }

    [Fact]
    public void GetRequiredComponent_ShouldReturnExistingComponent()
    {
        var component = GetComponentOnGameObject();
        Assert.Equal(component, component.CallGetRequiredComponent<FakeComponent>());
    }

    [Fact]
    public void GetRequiredComponent_ShouldThrow_WhenComponentMissing()
    {
        var component = GetComponentOnGameObject();
        Assert.Throws<MissingComponentException<Transform>>(component.CallGetRequiredComponent<Transform>);
    }

    [Fact]
    public void SetGameObject_ShouldRemoveFromOldGameObjectThenAddToNewGameObject()
    {
        var component = new FakeComponent();
        GameObject oldGameObject = [component];
        GameObject newGameObject = [];

        component.GameObject = newGameObject;

        Assert.Null(oldGameObject.Get<FakeComponent>());
        Assert.Equal(newGameObject.Get<FakeComponent>(), component);
    }
}