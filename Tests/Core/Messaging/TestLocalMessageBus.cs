using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;

namespace Termule.Tests.Core.Messaging;

public class TestLocalMessageBus
{
    [Theory]
    [InlineData(Route.Downward, new[] { true, false, false })]
    [InlineData(Route.Local, new[] { false, true, false })]
    [InlineData(Route.Upward, new[] { false, false, true })]
    [InlineData(Route.Downward | Route.Local, new[] { true, true, false })]
    [InlineData(Route.Local | Route.Upward, new[] { false, true, true })]
    [InlineData(Route.Upward | Route.Downward, new[] { true, false, true })]
    [InlineData(Route.Local | Route.Upward | Route.Downward, new[] { true, true, true })]
    public void Broadcast_RoutesCorrectly(Route route, bool[] values)
    {
        Game game = new();
        GameObject child = [];
        GameObject gameObject = [child];
        GameObject parent = [gameObject];

        game.Root.Add(parent);

        FakeListener<bool> downwardListener = new();
        FakeListener<bool> localListener = new();
        FakeListener<bool> upwardListener = new();

        child.Bus.Subscribe(downwardListener);
        gameObject.Bus.Subscribe(localListener);
        parent.Bus.Subscribe(upwardListener);

        gameObject.Bus.Broadcast(true, route);

        Assert.Equal(values[0], downwardListener.ReceivedMessage);
        Assert.Equal(values[1], localListener.ReceivedMessage);
        Assert.Equal(values[2], upwardListener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_WhenNotActivatedAndRouteIsNonlocal_Throws()
    {
        GameObject gameObject = [];

        Assert.Throws<InvalidOperationException>(() => gameObject.Bus.Broadcast(true, Route.Upward)
        );
        Assert.Throws<InvalidOperationException>(() => gameObject.Bus.Broadcast(true, Route.Downward)
        );
    }
}