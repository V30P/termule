using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Exceptions;

namespace Termule.Tests.Core;

public class TestComponent
{
    private static FakeComponent GetComponentOnGameObject()
    {
        GameObject gameObject = [];
        FakeComponent component = new();
        gameObject.Add(component);

        return component;
    }

    [Fact]
    public void Destroy_ShouldRemoveFromGameObject()
    {
        FakeComponent component = GetComponentOnGameObject();
        component.Destroy();
        Assert.Null(component.GameObject);
    }

    [Fact]
    public void GetRequiredComponent_ShouldReturnExistingComponent()
    {
        FakeComponent component = GetComponentOnGameObject();
        Assert.Equal(component, component.CallGetRequiredComponent<FakeComponent>());
    }

    [Fact]
    public void GetRequiredComponent_ShouldThrow_WhenComponentMissing()
    {
        FakeComponent component = GetComponentOnGameObject();
        Assert.Throws<MissingComponentException<Transform>>(component.CallGetRequiredComponent<Transform>);
    }

    [Fact]
    public void SetGameObject_ShouldRemoveFromOldGameObjectThenAddToNewGameObject()
    {
        FakeComponent component = new();
        GameObject oldGameObject = [component];
        GameObject newGameObject = [];

        component.GameObject = newGameObject;

        Assert.Null(oldGameObject.Get<FakeComponent>());
        Assert.Equal(newGameObject.Get<FakeComponent>(), component);
    }
}