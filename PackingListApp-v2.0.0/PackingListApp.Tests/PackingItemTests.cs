namespace PackingListApp.Tests;

public class PackingItemTests
{
    private PackingItem item;

    public PackingItemTests()
    {
        item = new PackingItem("Pants", 1);
    }

    [Fact]
    public void Test_MarkPacked()
    {
        item.MarkPacked();
        Assert.True(item.IsPacked);
    }

    [Fact]
    public void Test_MarkUnpacked()
    {
        item.MarkPacked();
        item.MarkUnpacked();
        Assert.False(item.IsPacked);
    }

    [Fact]
    public void Test_SetPacked()
    {
        item.SetPacked(true);
        Assert.True(item.IsPacked);

        item.SetPacked(false);
        Assert.False(item.IsPacked);
    }

    [Fact]
    public void Test_SetQuantity()
    {
        item.SetQuantity(5);
        Assert.Equal(5, item.Quantity);
    }

    [Fact]
    public void Test_Constructor_SetsProperties()
    {
        var newItem = new PackingItem("Pants", 3);

        Assert.Equal("Pants", newItem.Name);
        Assert.Equal(3, newItem.Quantity);
        Assert.False(newItem.IsPacked);
    }
}
