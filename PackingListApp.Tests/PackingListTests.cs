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
    public void Rename_UpdatesListName()
    {
        var list = new PackingList("Trip");
        list.Rename("Vacation");
        Assert.Equal("Vacation", list.Name);
    }

    [Fact]
    public void ClearPackedStatus_Should_Unpack_All_Items()
    {
        var list = new PackingList("Trip");
        var a = new PackingItem("Shirt", 1);
        var b = new PackingItem("Socks", 1);

        a.MarkPacked();
        b.MarkPacked();

        list.AddItem(a);
        list.AddItem(b);

        list.ClearPackedStatus();

        Assert.All(list.Items, item => Assert.False(item.IsPacked));
    }

    [Fact]
    public void ResetQuantities_Should_Set_All_Quantities_To_Default()
    {
        var list = new PackingList("Trip");
        list.AddItem(new PackingItem("Shirt", 3));
        list.AddItem(new PackingItem("Socks", 5));

        list.ResetQuantitiesToDefault();

        Assert.All(list.Items, item => Assert.Equal(1, item.Quantity));
    }

    [Fact]
    public void ResetAll_Should_Unpack_All_Items_And_Reset_Quantities()
    {
        var list = new PackingList("Trip");
        list.AddItem(new PackingItem("Shirt", 3));
        list.AddItem(new PackingItem("Socks", 5));

        list.Items[0].MarkPacked();
        list.Items[1].MarkPacked();

        list.ResetAll();

        Assert.All(list.Items, item =>
        {
            Assert.False(item.IsPacked);
            Assert.Equal(1, item.Quantity);
        });
    }
}
