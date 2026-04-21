using Termule.Engine.Systems.Input;

namespace Termule.Tests.Systems.Input;

public class TestKeyboard
{
    [Fact]
    public void Get_WhenBindExists_ReturnsValue()
    {
        Keyboard keyboard = new() { Binds = new BindMap { ["Test"] = new FakeBind() } };

        Assert.True(keyboard.Get<bool>("Test"));
    }

    [Fact]
    public void Get_WhenNoBindOfName_Throws()
    {
        Keyboard keyboard = new();

        Assert.Throws<ArgumentException>(() => keyboard.Get<object>("Test"));
    }

    [Fact]
    public void Get_WithIncorrectValueType_Throws()
    {
        Keyboard keyboard = new() { Binds = new BindMap { ["Test"] = new FakeBind() } };

        Assert.Throws<ArgumentException>(() => keyboard.Get<int>("Test"));
    }

    [Fact]
    public void SettingBinds_ToNull_Throws()
    {
        Keyboard keyboard = new();
        Assert.Throws<ArgumentNullException>(() => keyboard.Binds = null);
    }

    [Fact]
    public void Tick_CallsUpdateOnBindMap()
    {
        Keyboard keyboard = new();
        FakeBind bind = new();
        keyboard.Binds = new BindMap { ["Test"] = bind };

        keyboard.Tick();

        Assert.True(bind.GetValueInvoked);
    }
}