namespace PackingListApp.Tests;

public class PackingListManagerTests
{
    private readonly string testDir = "TestLists";
    private PackingListManager manager;

    public PackingListManagerTests()
    {
        if (Directory.Exists(testDir))
            Directory.Delete(testDir, true);

        // Override internal storage via reflection
        var storage = new TextFileStorage(testDir);
        var repo = new PackingListRepository(storage);

        manager = new PackingListManager();

        typeof(PackingListManager)
            .GetField("repository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
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

