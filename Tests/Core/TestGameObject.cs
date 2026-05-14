using Termule.Engine.Core;

namespace Termule.Tests.Core;

public class TestGameObject
{
    public static readonly TheoryData<Type[], int> GetAllData = new()
    {
        { Array.Empty<Type>(), 0 },
        { new[] { typeof(ComponentB) }, 0 },
        { new[] { typeof(ComponentA) }, 1 },
        { new[] { typeof(ComponentA), typeof(ComponentB) }, 1 },
        { new[] { typeof(ComponentA), typeof(ComponentA) }, 2 },
        { new[] { typeof(ComponentA), typeof(ComponentA), typeof(ComponentA) }, 3 }
    };

    private interface IDerivedComponent
    {
    }

    [Fact]
    public void Add_AddsAndActivatesComponent()
    {
        Game game = new();
        GameObject gameObject = [];
        game.World.Add(gameObject);
        FakeComponent component = new();

        gameObject.Add(component);

        Assert.Equal(component, gameObject.Get<FakeComponent>());
        Assert.Equal(1, component.ActivateCount);
    }

    [Fact]
    public void Add_AddsThenActivatesComponentsSimultaneously()
    {
        Game game = new();
        GameObject gameObject = [];
        game.World.Add(gameObject);
        DependentComponent dependentComponent = new();

        gameObject.Add(dependentComponent, new FakeComponent());

        Assert.True(dependentComponent.HasDependency);
    }

    [Fact]
    public void Add_WhenComponentAlreadyInAGameObject_Throws()
    {
        FakeComponent component = new();
        new GameObject().Add(component);
        _ = Assert.Throws<ArgumentException>(() => new GameObject().Add(component));
    }

    [Fact]
    public void Add_WhenSameComponentAlreadyAdded_Throws()
    {
        GameObject gameObject = [];
        FakeComponent component = new();

        gameObject.Add(component);

        _ = Assert.Throws<ArgumentException>(() => gameObject.Add(component));
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
            .GetMethod(nameof(GameObject.Get))
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
    public void GetAll_ReturnsMatchingComponents(Type[] componentTypes, int matchingCount)
    {
        Component[] components = new Component[componentTypes.Length];
        for (int i = 0; i < componentTypes.Length; i++)
        {
            components[i] = (Component) Activator.CreateInstance(componentTypes[i]);
        }

        GameObject gameObject = [.. components];
        Assert.Equal(matchingCount, gameObject.GetAll<ComponentA>().Count());
    }

    [Fact]
    public void Activate_ActivatesComponents()
    {
        Game game = new();
        FakeComponent component = new();
        GameObject gameObject = [component];

        game.World.Add(gameObject);

        Assert.Equal(1, component.ActivateCount);
    }

    [Fact]
    public void Remove_RemovesAndDeactivatesComponent()
    {
        Game game = new();
        GameObject gameObject = [];
        game.World.Add(gameObject);

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

        _ = Assert.Throws<InvalidOperationException>(() => new GameObject().Remove(component));
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
    public void Deactivate_DeactivatesComponents()
    {
        Game game = new();
        FakeComponent component = new();
        GameObject gameObject = [component];
        game.World.Add(gameObject);

        game.World.Remove(gameObject);

        Assert.Equal(1, component.DeactivateCount);
    }

    private sealed class DerivedComponent : FakeComponent, IDerivedComponent
    {
    }

    private sealed class ComponentA : Component
    {
    }

    private sealed class ComponentB : Component
    {
    }

    private sealed class DependentComponent : Component
    {
        public bool HasDependency { get; private set; }

        protected override void Activate()
        {
            HasDependency = GameObject.Get<FakeComponent>() != null;
        }
    }
}
