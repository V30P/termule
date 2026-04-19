using Termule.Engine.Systems.Input;
using Termule.Tests.Systems.Input.Fakes;

namespace Termule.Tests.Systems.Input;

public class TestController
{
    [Fact]
    public void Get_ShouldReturnValue_WhenBindExists()
    {
        Keyboard keyboard = new()
        {
            Binds = new BindMap { ["Test"] = new FakeBind() }
        };

        Assert.True(keyboard.Get<bool>("Test"));
    }

    [Fact]
    public void Get_ShouldThrow_WhenIncorrectValueTypeIsGiven()
    {
        Keyboard keyboard = new()
        {
            Binds = new BindMap { ["Test"] = new FakeBind() }
        };

        Assert.Throws<ArgumentException>(() => keyboard.Get<int>("Test"));
    }

    [Fact]
    public void Get_ShouldThrow_WhenKeyNoBindOfName()
    {
        Keyboard keyboard = new();

        Assert.Throws<ArgumentException>(() => keyboard.Get<object>("Test"));
    }

    [Fact]
    public void SettingBinds_ShouldThrow_WhenValueIsNull()
    {
        Keyboard keyboard = new();
        Assert.Throws<ArgumentNullException>(() => keyboard.Binds = null);
    }

    [Fact]
    public void Tick_ShouldCallUpdateOnBindMap()
    {
        Keyboard keyboard = new();
        FakeBind bind = new();
        keyboard.Binds = new BindMap { ["Test"] = bind };

        keyboard.Tick();

        Assert.True(bind.GetValueInvoked);
    }
}