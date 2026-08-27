using System.Text;
using Termule.Engine.Exceptions;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Systems.Resources;

public class TestResourceLoader
{
    [Fact]
    public void Load_LoadsResource()
    {
        FakeResourceProvider provider = new()
        {
            Resources = new()
            {
                { "TEST", Serializer.Serialize(new FakeResource("Wrong")) },
                { "test", Serializer.Serialize(new FakeResource("Correct")) },
                { "test1", Serializer.Serialize(new FakeResource("Wrong")) }
            }
        };
        ResourceLoader resourceLoader = new(provider);

        FakeResource loaded = resourceLoader.Load<FakeResource>("test");

        Assert.Equivalent(new FakeResource("Correct"), loaded);
    }

    [Fact]
    public void Load_CachesAndPullsFromCache()
    {
        FakeResourceProvider provider = new()
        {
            Resources = new() { { "test", Serializer.Serialize(new FakeResource("Test")) } }
        };
        ResourceLoader resourceLoader = new(provider);

        _ = resourceLoader.Load<FakeResource>("test");
        _ = provider.Resources.Remove("test");
        FakeResource loaded = resourceLoader.Load<FakeResource>("test");

        Assert.Equivalent(new FakeResource("Test"), loaded);
    }

    [Fact]
    public void Load_WhenResourceDoesNotExist_Throws()
    {
        ResourceLoader resourceLoader = new(new FakeResourceProvider());

        _ = Assert.Throws<ResourceLoadException>(() => resourceLoader.Load<FakeResource>("test"));
    }

    private sealed class FakeResource(string text) : IResource
    {
        public string Text { get; set; } = text;
    }

    private sealed class FakeResourceProvider() : IResourceProvider
    {
        public Dictionary<string, string> Resources { get; init; } = [];

        public bool ResourceExists(string name)
        {
            return Resources.ContainsKey(name);
        }

        public Stream GetResourceStream(string name)
        {
            byte[] resourceBytes = Encoding.UTF8.GetBytes(Resources[name]);
            return new MemoryStream(resourceBytes);
        }
    }
}
