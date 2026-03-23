using Termule.Engine.Core;
using Termule.Tests.Utilities;

namespace Termule.Tests.Core;

public class TestGameObjectd
{
    private interface IDerivedComponent;

    private class DependentComponent : Component
    {
        internal bool HasDependency { get; private set; }

        internal DependentComponent()
        {
            Registered += CheckForDependency;
        }

        private void CheckForDependency()
        {
            HasDependency = GameObject.Get<FakeComponent>() != null;
        }
    }

    private class DerivedComponent : FakeComponent, IDerivedComponent;

    private class ComponentA : Component;

    private class ComponentB : Component;

    public class TestAdd
    {
        [Fact]
        internal void Add_ShouldAddAndRegisterComponent()
        {
            var game = Game.Create();
            GameObject gameObject = [];
            game.Root.Add(gameObject);
            FakeComponent component = new();

            gameObject.Add(component);

            Assert.Equal(component, gameObject.Get<FakeComponent>());
            Assert.True(component.RegisteredInvoked);
        }

        [Fact]
        internal void Add_ShouldAddThenRegisterComponentsSimultaneously()
        {
            var game = Game.Create();
            GameObject gameObject = [];
            game.Root.Add(gameObject);
            DependentComponent dependentComponent = new();

            gameObject.Add(dependentComponent, new FakeComponent());

            Assert.True(dependentComponent.HasDependency);
        }

        [Fact]
        internal void Add_ShouldThrow_WhenComponentAlreadyInAGameObject()
        {
            FakeComponent component = new();
            new GameObject().Add(component);
            Assert.Throws<ArgumentException>(() => new GameObject().Add(component));
        }

        [Fact]
        internal void Add_ShouldThrow_WhenSameComponentIAddedTwice()
        {
            GameObject gameObject = [];
            FakeComponent component = new();

            gameObject.Add(component);

            Assert.Throws<ArgumentException>(() => gameObject.Add(component));
        }
    }
    
    public class TestRemove
    {
        [Fact]
        internal void Remove_ShouldRemoveAndUnregisterComponent()
        {
            var game = Game.Create();
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
    }

    public class TestGet
    {
        [Fact]
        internal void Get_ShouldReturnQueuedAdditions()
        {
            GameObject gameObject = [];
            FakeComponent component = new();

            gameObject.Add(component);

            Assert.Same(component, gameObject.Get<FakeComponent>());
        }

        [Fact]
        internal void Get_ShouldReturnFullyAddedComponents()
        {
            var game = Game.Create();
            var component = new FakeComponent();
            GameObject gameObject = [component];
            game.Root.Add(gameObject);
        
            game.Prepare();
            game.RunForFrames(1);
        
            Assert.Same(component, gameObject.Get<FakeComponent>());
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

            var result = (Component?)typeof(GameObject)
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
    }

    public class TestGetALl
    {
        private class GetAllData : TheoryData<Component[], int>
        {
            public GetAllData()
            {
                Add([], 0);
                Add([new ComponentB()], 0);
                Add([new ComponentA()], 1);
                Add([new ComponentA(), new ComponentB()], 1);
                Add([new ComponentA(), new ComponentA()], 2);
                Add([new ComponentA(), new ComponentA(), new ComponentA()], 3);
            }
        }

        [Fact]
        internal void GetAll_ShouldReturnComponentsRegardlessOfAdditionState()
        {
            GameObject gameObject = [new FakeComponent()];
            gameObject.Tick();
            
            gameObject.Add(new FakeComponent());
            
            Assert.Equal(2, gameObject.GetAll<FakeComponent>().Count());
        }
        
        [Fact]
        internal void GetAll_ShouldIncludeQueuedComponents()
        {
            GameObject gameObject = [];
            FakeComponent queuedComponent = new();

            gameObject.Add(queuedComponent);

            Assert.Contains(queuedComponent, gameObject.GetAll<FakeComponent>());
        }

        [Theory]
        [ClassData(typeof(GetAllData))]
        internal void GetAll_ShouldReturnMatchingComponents(Component[] components, int matchingCount)
        {
            GameObject gameObject = [.. components];
            Assert.Equal(matchingCount, gameObject.GetAll<ComponentA>().Count());
        }
    }

    [Fact]
    internal void Register_ShouldRegisterComponents()
    {
        var game = Game.Create();
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

        gameObject.Tick();

        Assert.Equal(1, component.TickCount);
    }

    [Fact]
    internal void Unregister_ShouldUnregisterComponents()
    {
        var game = Game.Create();
        FakeComponent component = new();
        GameObject gameObject = [component];
        game.Root.Add(gameObject);

        game.Root.Remove(gameObject);
        
        Assert.True(component.UnregisteredInvoked);
    }
}