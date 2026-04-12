namespace PackingListApp.Tests;

public class PackingListTests
{
    private PackingList list;

    public PackingListTests()
    {
        list = new PackingList("Test Trip");
    }

    [Fact]
    public void Test_AddItem()
    {
        var item = new PackingItem("Pants", 2);
        list.AddItem(item);

        Assert.Single(list.Items);
        Assert.Equal("Pants", list.Items[0].Name);
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
