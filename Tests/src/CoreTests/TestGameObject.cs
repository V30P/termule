namespace Termule.Tests.Core;

using Termule.Core;
using Termule.Tests.Utilities;

public class TestGameObject()
{
    [Fact]
    internal void Add_ShouldAddAndRegisterComponent()
    {
        IConfigurableGame game = Game.Create();
        GameObject gameObject = [];
        game.Root.Add(gameObject);
        FakeComponent component = new();

        gameObject.Add(component);

        Assert.Equal(component, gameObject.Get<FakeComponent>());
        Assert.True(component.RegisteredInvoked);
    }

    [Fact]
    internal void Add_ShouldThrow_WhenComponentAlreadyInAGameObject()
    {
        FakeComponent component = new();
        new GameObject().Add(component);

        Assert.Throws<InvalidOperationException>(() => new GameObject().Add(component));
    }

    private class DependentComponent : Component
    {
        internal DependentComponent()
        {
            this.Registered += this.CheckForDependency;
        }

        internal bool HasDependency { get; private set; }

        private void CheckForDependency()
        {
            this.HasDependency = this.GameObject.Get<FakeComponent>() != null;
        }
    }

    [Fact]
    internal void Add_ShouldAddThenRegisterComponentsSimultaneously()
    {
        IConfigurableGame game = Game.Create();
        GameObject gameObject = [];
        game.Root.Add(gameObject);
        DependentComponent dependentComponent = new();

        gameObject.Add(dependentComponent, new FakeComponent());

        Assert.True(dependentComponent.HasDependency);
    }

    [Fact]
    internal void Remove_ShouldRemoveAndUnregisterComponent()
    {
        IConfigurableGame game = Game.Create();
        GameObject gameObject = [];
        game.Root.Add(gameObject);

        FakeComponent component = new();
        gameObject.Add(component);

        gameObject.Remove(component);

        Assert.Null(gameObject.Get<FakeComponent>());
        Assert.True(component.UnregisteredInvoked);
    }

    [Fact]
    internal void Remove_ShouldThrow_WhenComponentNotOnGameObject()
    {
        FakeComponent component = new();
        new GameObject().Add(component);

        Assert.Throws<InvalidOperationException>(() => new GameObject().Remove(component));
    }

    private interface IDerivedComponent
    {
    }

    private class DerivedComponent : FakeComponent, IDerivedComponent
    {
    }

    [Theory]
    [InlineData(typeof(Component))]
    [InlineData(typeof(FakeComponent))]
    [InlineData(typeof(IDerivedComponent))]
    [InlineData(typeof(DerivedComponent))]
    internal void Get_ShouldReturnExistingComponent(Type getType)
    {
        Component component = new DerivedComponent();
        GameObject gameObject = [component];

        Component? result = (Component?)typeof(GameObject)
            .GetMethod(nameof(GameObject.Get))!
            .MakeGenericMethod(getType)
            .Invoke(gameObject, null);

        Assert.Equal(component, result);
    }

    [Fact]
    internal void Get_ShouldReturnNull_WhenComponentMissing()
    {
        GameObject gameObject = [];
        Assert.Null(gameObject.Get<FakeComponent>());
    }

    private class ComponentA : Component
    {
    }

    private class ComponentB : Component
    {
    }

    public class GetAllData : TheoryData<Component[], int>
    {
        public GetAllData()
        {
            this.Add([], 0);
            this.Add([new ComponentB()], 0);
            this.Add([new ComponentA()], 1);
            this.Add([new ComponentA(), new ComponentB()], 1);
            this.Add([new ComponentA(), new ComponentA()], 2);
            this.Add([new ComponentA(), new ComponentA(), new ComponentA()], 3);
        }
    }

    [Theory]
    [ClassData(typeof(GetAllData))]
    internal void GetAll_ShouldReturnMatchingComponents(Component[] components, int matchingCount)
    {
        GameObject gameObject = [.. components];
        Assert.Equal(matchingCount, gameObject.GetAll<ComponentA>().Count());
    }

    [Fact]
    internal void Register_ShouldRegisterComponents()
    {
        IConfigurableGame game = Game.Create();
        FakeComponent component = new();
        GameObject gameObject = [component];

        game.Root.Add(gameObject);

        Assert.True(component.RegisteredInvoked);
    }

    [Fact]
    internal void Tick_ShouldTickComponents()
    {
        FakeComponent component = new();
        GameObject gameObject = [component];

        ((IHostedComponent)gameObject).Tick();

        Assert.Equal(1, component.TickCount);
    }

    [Fact]
    internal void Unregister_ShouldUnregisterComponents()
    {
        IConfigurableGame game = Game.Create();
        FakeComponent component = new();
        GameObject gameObject = [component];
        game.Root.Add(gameObject);

        game.Root.Remove(gameObject);

        Assert.True(component.UnregisteredInvoked);
    }
}