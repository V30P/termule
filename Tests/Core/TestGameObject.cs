using Termule.Engine.Core;

namespace Termule.Tests.Core;

public class TestGameObject
{
    public static readonly IEnumerable<object[]> GetAllData =
    [
        [Array.Empty<Component>(), 0],
        [new Component[] { new ComponentB() }, 0],
        [new Component[] { new ComponentA() }, 1],
        [new Component[] { new ComponentA(), new ComponentB() }, 1],
        [new Component[] { new ComponentA(), new ComponentA() }, 2],
        [new Component[] { new ComponentA(), new ComponentA(), new ComponentA() }, 3]
    ];

    [Fact]
    public void Add_AddsAndRegistersComponent()
    {
        Game game = new();
        GameObject gameObject = [];
        game.Root.Add(gameObject);
        FakeComponent component = new();

        gameObject.Add(component);

        Assert.Equal(component, gameObject.Get<FakeComponent>());
        Assert.Equal(1, component.ActivateCount);
    }

    [Fact]
    public void Add_AddsThenRegistersComponentsSimultaneously()
    {
        Game game = new();
        GameObject gameObject = [];
        game.Root.Add(gameObject);
        DependentComponent dependentComponent = new();

        gameObject.Add(dependentComponent, new FakeComponent());

        Assert.True(dependentComponent.HasDependency);
    }

    [Fact]
    public void Add_WhenComponentAlreadyInAGameObject_Throws()
    {
        FakeComponent component = new();
        new GameObject().Add(component);
        Assert.Throws<ArgumentException>(() => new GameObject().Add(component));
    }

    [Fact]
    public void Add_WhenSameComponentAlreadyAdded_Throws()
    {
        GameObject gameObject = [];
        FakeComponent component = new();

        gameObject.Add(component);

        Assert.Throws<ArgumentException>(() => gameObject.Add(component));
    }

    [Theory]
    [InlineData(typeof(Component))]
    [InlineData(typeof(FakeComponent))]
    [InlineData(typeof(IDerivedComponent))]
    [InlineData(typeof(DerivedComponent))]
    public void Get_ReturnsExistingComponent(Type getType)
    {
        Component component = new DerivedComponent();
        GameObject gameObject = [component];

        Component result = (Component) typeof(GameObject)
            .GetMethod(nameof(GameObject.Get))!
            .MakeGenericMethod(getType)
            .Invoke(gameObject, null);

        Assert.Equal(component, result);
    }

    [Fact]
    public void Get_WhenComponentMissing_ReturnsNull()
    {
        GameObject gameObject = [];
        Assert.Null(gameObject.Get<FakeComponent>());
    }

    [Theory]
    [MemberData(nameof(GetAllData))]
    public void GetAll_ReturnsMatchingComponents(Component[] components, int matchingCount)
    {
        GameObject gameObject = [.. components];
        Assert.Equal(matchingCount, gameObject.GetAll<ComponentA>().Count());
    }

    [Fact]
    public void Register_RegistersComponents()
    {
        Game game = new();
        FakeComponent component = new();
        GameObject gameObject = [component];

        game.Root.Add(gameObject);

        Assert.Equal(1, component.ActivateCount);
    }

    [Fact]
    public void Remove_RemovesAndUnregistersComponent()
    {
        Game game = new();
        GameObject gameObject = [];
        game.Root.Add(gameObject);

        FakeComponent component = new();
        gameObject.Add(component);

        gameObject.Remove(component);

        Assert.Null(gameObject.Get<FakeComponent>());
        Assert.Equal(1, component.DeactivateCount);
    }

    [Fact]
    public void Remove_WhenComponentNotInGameObject_Throws()
    {
        FakeComponent component = new();
        new GameObject().Add(component);

        Assert.Throws<InvalidOperationException>(() => new GameObject().Remove(component));
    }

    [Fact]
    public void Tick_TicksComponents()
    {
        FakeComponent component = new();
        GameObject gameObject = [component];

        gameObject.Tick();

        Assert.Equal(1, component.TickCount);
    }

    [Fact]
    public void Unregister_UnregistersComponents()
    {
        Game game = new();
        FakeComponent component = new();
        GameObject gameObject = [component];
        game.Root.Add(gameObject);

        game.Root.Remove(gameObject);

        Assert.Equal(1, component.DeactivateCount);
    }

    private interface IDerivedComponent
    {
    }

    private class DerivedComponent : FakeComponent, IDerivedComponent
    {
    }

    private class ComponentA : Component
    {
    }

    private class ComponentB : Component
    {
    }

    private class DependentComponent : Component
    {
        public bool HasDependency { get; private set; }

        protected override void Activate()
        {
            HasDependency = GameObject.Get<FakeComponent>() != null;
        }
    }
}