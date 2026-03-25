using Termule.Engine.Components;
using Termule.Engine.Core;

namespace Termule.Tests.Components;

public class TestTransform
{
    [Fact]
    public void CachedLocalPos_ShouldBeApplied_WhenRegistered()
    {
        var game = Game.Create();
        var transform = new Transform { LocalPos = (1, 1) };
        GameObject gameObject = [transform];
        GameObject parentGameObject = [new Transform { Pos = (1, 1) }, gameObject];

        game.Root.Add(parentGameObject);

        Assert.Equal((2, 2), transform.Pos);
    }

    [Fact]
    public void CachedPos_ShouldBeApplied_WhenRegistered()
    {
        var game = Game.Create();
        var transform = new Transform { Pos = (1, 1) };

        game.Root.Add(transform);

        Assert.Equal((1, 1), transform.Pos);
    }

    [Fact]
    public void MovingParent_ShouldMoveChild()
    {
        var game = Game.Create();
        var child = new Transform { LocalPos = (1, 1) };
        GameObject gameObject = [child];

        var parent = new Transform();
        GameObject parentGameObject = [parent, gameObject];
        game.Root.Add(parentGameObject);

        parent.Pos = (-1, -1);

        Assert.Equal(child.Pos, (0, 0));
        Assert.Equal(child.LocalPos, (1, 1));
    }

    [Fact]
    public void NestedTransforms_ShouldApplyRecursively()
    {
        var game = Game.Create();
        var transform = new Transform();
        GameObject gameObject = [transform];

        for (var i = 0; i < 10; i++)
        {
            gameObject = [new Transform { LocalPos = (1, 1) }, gameObject];
        }

        game.Root.Add(gameObject);

        Assert.Equal((10, 10), transform.Pos);
    }

    [Fact]
    public void Pos_ShouldBeConstant_WhenParentChanges()
    {
        var transform = new Transform { Pos = (1, 1) };
        GameObject _ = [transform];
        var newGameObject = new GameObject();

        transform.GameObject = newGameObject;

        Assert.Equal((1, 1), transform.Pos);
    }

    [Fact]
    public void SettingLocalPos_ShouldUpdatePos()
    {
        var game = Game.Create();
        var transform = new Transform { Pos = (1, 1) };
        GameObject gameObject = [transform];
        GameObject parentGameObject = [new Transform { Pos = (1, 1) }, gameObject];
        game.Root.Add(parentGameObject);

        transform.LocalPos = (-1, -1);

        Assert.Equal((0, 0), transform.Pos);
    }

    [Fact]
    public void SettingPos_ShouldUpdateLocalPos()
    {
        var game = Game.Create();
        var transform = new Transform { Pos = (1, 1) };
        GameObject gameObject = [transform];
        GameObject parentGameObject = [new Transform { Pos = (1, 1) }, gameObject];
        game.Root.Add(parentGameObject);

        transform.Pos = (0, 0);

        Assert.Equal((-1, -1), transform.LocalPos);
    }
}