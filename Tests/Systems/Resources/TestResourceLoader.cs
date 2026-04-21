using System.IO.Abstractions.TestingHelpers;
using Termule.Engine.Exceptions;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Systems.Resources;

public class TestResourceLoader
{
    private class FakeResource(string text) : IResource
    {
        public string Text { get; set; } = text;
        public static string FileExtension => ".fake";
    }

    [Fact]
    public void Load_CachesValuesAndPullFromCache()
    {
        MockFileSystem fileSystem = new(new Dictionary<string, MockFileData>
        {
            { "/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Test"))) }
        });
        ResourceLoader resourceLoader = new(fileSystem, "/");

        _ = resourceLoader.Load<FakeResource>("test");
        fileSystem.RemoveFile("/test.fake");
        FakeResource loaded = resourceLoader.Load<FakeResource>("test");

        Assert.Equivalent(new FakeResource("Test"), loaded);
    }

    [Fact]
    public void Load_RespectsResourceDir()
    {
        MockFileSystem fileSystem = new(new Dictionary<string, MockFileData>
        {
            { "/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Wrong"))) },
            { "/dir/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Correct"))) }
        });
        ResourceLoader resourceLoader = new(fileSystem, "/dir", false);

        FakeResource loaded = resourceLoader.Load<FakeResource>("test");

        Assert.Equivalent(new FakeResource("Correct"), loaded);
    }

    [Fact]
    public void Load_WithFullPath_ReturnsResource()
    {
        MockFileSystem fileSystem = new(new Dictionary<string, MockFileData>
        {
            { "/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Test"))) }
        });
        ResourceLoader resourceLoader = new(fileSystem, "/");

        FakeResource loaded = resourceLoader.Load<FakeResource>("test.fake");

        Assert.Equivalent(new FakeResource("Test"), loaded);
    }

    [Fact]
    public void Load_WithMultilevelPath_ReturnsResource()
    {
        MockFileSystem fileSystem = new(new Dictionary<string, MockFileData>
        {
            { "/dir1/dir2/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Test"))) }
        });
        ResourceLoader resourceLoader = new(fileSystem, "/");

        FakeResource loaded = resourceLoader.Load<FakeResource>("dir1/dir2/test");

        Assert.Equivalent(new FakeResource("Test"), loaded);
    }

    [Fact]
    public void Load_GivenPathWithoutExtension_ReturnsResource()
    {
        MockFileSystem fileSystem = new(new Dictionary<string, MockFileData>
        {
            { "/test.fake", new MockFileData(Serializer.Serialize(new FakeResource("Test"))) }
        });
        ResourceLoader resourceLoader = new(fileSystem, "/");

        FakeResource loaded = resourceLoader.Load<FakeResource>("test");

        Assert.Equivalent(new FakeResource("Test"), loaded);
    }

    [Fact]
    public void Load_WhenPathDoesNotExist_Throws()
    {
        ResourceLoader resourceLoader = new(new MockFileSystem(), "/");

        Assert.Throws<ResourceLoadException>(() => resourceLoader.Load<FakeResource>("test"));
    }
}