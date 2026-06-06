using Termule.Engine.Components;

namespace Termule.Tests.Components;

public class TestCircleRenderer
{
    [Fact]
    public void SettingRadius_ToNegative_Throws()
    {
        CircleRenderer renderer = new();

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => renderer.Radius = -1);
    }
}
