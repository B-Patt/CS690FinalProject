using PackingListApp.Application;
using PackingListApp.Infrastructure;
using PackingListApp.Domain;

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

    manager = new PackingListManager(repo);
    }

    [Fact]
    public void Test_CreateList()
    {
        var list = manager.CreateList("Trip");
        Assert.NotNull(list);
        Assert.Equal("Trip", list.Name);
    }

    [Fact]
    public void Test_CreateList_Invalid()
    {
        var list = manager.CreateList("");
        Assert.Null(list);
    }

    [Fact]
    public void Test_LoadList()
    {
        manager.CreateList("LoadMe");
        var loaded = manager.LoadList("LoadMe");

        Assert.NotNull(loaded);
        Assert.Equal("LoadMe", loaded.Name);
    }

    [Fact]
    public void Test_DeleteList()
    {
        manager.CreateList("Gone");
        manager.DeleteList("Gone");

        Assert.Null(manager.LoadList("Gone"));
    }

    [Fact]
    public void RenameList_Should_Rename_List_When_NewName_Is_Unique()
    {

        var storage = new TextFileStorage(testDir);
        var repo = new PackingListRepository(storage);
        var manager = new PackingListManager(repo);

        var list = new PackingList("Trip");
        repo.SaveList(list);

        bool result = manager.RenameList("Trip", "Vacation");

        Assert.True(result);
        Assert.True(File.Exists(Path.Combine(testDir, "Vacation.txt")));
        Assert.False(File.Exists(Path.Combine(testDir, "Trip.txt")));
    }

}
