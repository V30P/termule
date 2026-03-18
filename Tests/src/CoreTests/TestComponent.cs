namespace Termule.Tests.Core;

using Termule.Components;
using Termule.Core;
using Termule.Tests.Utilities;

public class TestComponent
{
    [Fact]
    internal void Destroy_ShouldRemoveFromGameObject()
    {
        GetGameObjectWithComponent(out GameObject _, out FakeComponent component);
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
        GetGameObjectWithComponent(out GameObject _, out FakeComponent component);
        Assert.Equal(component, component.CallGetRequiredComponent<FakeComponent>());
    }

    [Fact]
    internal void GetRequiredComponent_ShouldThrow_WhenComponentMissing()
    {
        GetGameObjectWithComponent(out GameObject gameObject, out FakeComponent component);
        Assert.Throws<MissingComponentException<Transform>>(() => component.CallGetRequiredComponent<Transform>());
    }

    private static void GetGameObjectWithComponent(out GameObject gameObject, out FakeComponent component)
    {
        gameObject = [];
        component = new();
        gameObject.Add(component);
    }
}