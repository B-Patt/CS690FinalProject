namespace PackingListApp.Tests;

public class PackingListRepositoryTests
{
    private readonly string testDir;
    private TextFileStorage storage;
    private PackingListRepository repo;

    public PackingListRepositoryTests()
    {
        // Temporary Directory for testing
        testDir = Path.Combine(Path.GetTempPath(), "PackingListAppTests_Repository");

        if (Directory.Exists(testDir))
            Directory.Delete(testDir, true);

        Directory.CreateDirectory(testDir);

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
        Assert.Equal("Test Trip", loaded.Name.Trim()); 
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
        repo.SaveList(new PackingList("List1"));
        repo.SaveList(new PackingList("List2"));

        var names = repo.ListAll().Select(n => n.Trim()).ToList(); 

        Assert.Contains("List1", names);
        Assert.Contains("List2", names);
    }
}