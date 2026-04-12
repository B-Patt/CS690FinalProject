namespace PackingListApp.Tests;

public class PackingListRepositoryTests
{
    private readonly string testDir = "TestLists";
    private TextFileStorage storage;
    private PackingListRepository repo;

    public PackingListRepositoryTests()
    {
        if (Directory.Exists(testDir))
            Directory.Delete(testDir, true);

        storage = new TextFileStorage(testDir);
        repo = new PackingListRepository(storage);
    }

    [Fact]
    public void Test_SaveAndLoadList()
    {
        var list = new PackingList("Test Trip");
        list.AddItem(new PackingItem("Socks", 2));

        repo.SaveList(list);

        var loaded = repo.LoadList("Test Trip");

        Assert.NotNull(loaded);
        Assert.Equal("Test Trip", loaded.Name);
        Assert.Single(loaded.Items);
        Assert.Equal("Socks", loaded.Items[0].Name);
    }

    [Fact]
    public void Test_DeleteList()
    {
        var list = new PackingList("DeleteMe");
        repo.SaveList(list);

        repo.DeleteList("DeleteMe");

        Assert.Null(repo.LoadList("DeleteMe"));
    }

    [Fact]
    public void Test_ListAll()
    {
        repo.SaveList(new PackingList("A"));
        repo.SaveList(new PackingList("B"));

        var names = repo.ListAll();

        Assert.Contains("A", names);
        Assert.Contains("B", names);
    }
}
