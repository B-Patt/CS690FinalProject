using PackingListApp.Domain;

namespace PackingListApp.Tests;

public class PackingListTests
{
    private PackingList list;

    public PackingListTests()
    {
        list = new PackingList("Test Trip");
    }

    [Fact]
    public void AddItem_AddsItemToList()
    {
    var list = new PackingList("Trip");
    var item = new PackingItem("Pants", 2);

    list.AddItem(item);

    Assert.Contains(item, list.Items);
}

    [Fact]
    public void Test_RemoveItem_Found()
    {
        var item = new PackingItem("Pants", 1);
        list.AddItem(item);

        bool removed = list.RemoveItem("Pants");

        Assert.True(removed);
        Assert.Empty(list.Items);
    }

    [Fact]
    public void Test_RemoveItem_NotFound()
    {
        bool removed = list.RemoveItem("Missing");
        Assert.False(removed);
    }

    [Fact]
    public void RemoveItem_ReturnsFalse_WhenItemNotFound()
    {
        var list = new PackingList("Trip");

        var result = list.RemoveItem("Nonexistent");

        Assert.False(result);
    }

    [Fact]
    public void Test_FindItem()
    {
        var item = new PackingItem("Shoes", 1);
        list.AddItem(item);

        var found = list.FindItem("Shoes");

        Assert.NotNull(found);
        Assert.Equal("Shoes", found.Name);
    }

    [Fact]
    public void Test_FindItem_NotFound()
    {
        var found = list.FindItem("Nope");
        Assert.Null(found);
    }

    [Fact]
    public void Test_Rename()
    {
        list.Rename("Vacation");
        Assert.Equal("Vacation", list.Name);
    }
}
