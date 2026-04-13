namespace PackingListApp.Tests;

public class PackingListManagerTests
{
    private readonly string testDir;
    private PackingListManager manager;

    public PackingListManagerTests()
    {
        // Temporary Directory for testing
        testDir = Path.Combine(Path.GetTempPath(), "PackingListAppTests_Manager");

    if (Directory.Exists(testDir))
        Directory.Delete(testDir, true);

    Directory.CreateDirectory(testDir);

    var storage = new TextFileStorage(testDir);
    var repo = new PackingListRepository(storage);

    manager = new PackingListManager();

    typeof(PackingListManager)
        .GetField("repo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .SetValue(manager, repo);
    }

    [Fact]
    public void Test_CreateNewList()
    {
        var list = manager.CreateNewList("Trip");
        Assert.NotNull(list);
        Assert.Equal("Trip", list.Name);
    }

    [Fact]
    public void Test_CreateNewList_Invalid()
    {
        var list = manager.CreateNewList("");
        Assert.Null(list);
    }

    [Fact]
    public void Test_LoadList()
    {
        manager.CreateNewList("LoadMe");
        var loaded = manager.LoadList("LoadMe");

        Assert.NotNull(loaded);
        Assert.Equal("LoadMe", loaded.Name);
    }

    [Fact]
    public void Test_DeleteList()
    {
        manager.CreateNewList("Gone");
        manager.DeleteList("Gone");

        Assert.Null(manager.LoadList("Gone"));
    }
}
