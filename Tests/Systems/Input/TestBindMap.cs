using System.Collections;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Systems.Input;

public class TestBindMap
{
    private class AlternatingBind : FakeBind
    {
        private bool currentValue;

        internal override object GetValue()
        {
            base.GetValue();

            currentValue = !currentValue;
            return currentValue;
        }
    }

    [Fact]
    public void Enumerating_EnumeratesBindsAsKeyValuePairs()
    {
        Bind bindA = new FakeBind();
        Bind bindB = new FakeBind();

        BindMap bindMap = new()
        {
            ["A"] = bindA,
            ["B"] = bindB
        };

        IEnumerator enumerator = bindMap.GetEnumerator();
        using ((IDisposable)enumerator)
        {
            Assert.IsType<IEnumerator<KeyValuePair<string, Bind>>>(enumerator, false);

            enumerator.MoveNext();
            Assert.Equal(new KeyValuePair<string, Bind>("A", bindA), enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(new KeyValuePair<string, Bind>("B", bindB), enumerator.Current);
        }
    }

    [Fact]
    public void PollValues_ShouldUpdateValuesFromBinds()
    {
        BindMap bindMap = new()
        {
            ["Test"] = new AlternatingBind()
        };

        bindMap.PollValues();
        _ = bindMap.TryGetValue("Test", out object value);
        Assert.False((bool)value);

        bindMap.PollValues();
        _ = bindMap.TryGetValue("Test", out value);
        Assert.True((bool)value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndOutputNull_WhenNameDoesNotExist()
    {
        BindMap bindMap = new();

        Assert.False(bindMap.TryGetValue("Test", out object value));
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndOutputValue_WhenNameExists()
    {
        BindMap bindMap = new()
        {
            ["Test"] = new FakeBind()
        };

        Assert.True(bindMap.TryGetValue("Test", out object value));
        Assert.IsType<bool>(value);
    }
}