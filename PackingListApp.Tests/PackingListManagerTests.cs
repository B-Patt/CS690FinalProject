using PackingListApp.Application;
using PackingListApp.Infrastructure;
using PackingListApp.Interfaces;
using PackingListApp.Domain;

namespace PackingListApp.Tests;

public class PackingListManagerTests
{
    private PackingListManager manager;
    private IPackingListRepository repo;

    public PackingListManagerTests()
    {
    repo = new PackingListRepository(new TextFileStorage("TestData"));
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
}
