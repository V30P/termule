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
    public void Destroy_RemovesFromGameObject()
    {
        FakeComponent component = GetComponentOnGameObject();
        component.Destroy();
        Assert.Null(component.GameObject);
    }

    [Fact]
    public void GetRequiredComponent_ReturnsExistingComponent()
    {
        FakeComponent component = GetComponentOnGameObject();
        Assert.Equal(component, component.CallGetRequiredComponent<FakeComponent>());
    }

    [Fact]
    public void GetRequiredComponent_WhenComponentMissing_Throws()
    {
        FakeComponent component = GetComponentOnGameObject();
        Assert.Throws<MissingComponentException<Transform>>(component.CallGetRequiredComponent<Transform>);
    }

    [Fact]
    public void SettingGameObject_RemovesFromOldGameObjectAndAddsToNewGameObject()
    {
        FakeComponent component = new();
        GameObject oldGameObject = [component];
        GameObject newGameObject = [];

        component.GameObject = newGameObject;

        Assert.Null(oldGameObject.Get<FakeComponent>());
        Assert.Equal(newGameObject.Get<FakeComponent>(), component);
    }
}