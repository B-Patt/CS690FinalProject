namespace PackingListApp.Domain;

public class PackingItem
{
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public bool IsPacked { get; private set; }

    public PackingItem(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
        IsPacked = false;
    }

    public void SetPacked(bool value)
    {
        IsPacked = value;
    }

    public void MarkUnpacked()
    {
        IsPacked = false;
    }

    public void MarkPacked()
    {
        IsPacked = true;
    }

    public void ResetQuantity(int defaultQty = 1)
    {
        Quantity = defaultQty;
    }

    public void SetQuantity(int qty)
    {
        Quantity = qty;
    }

    public void TogglePacked()
    {
        IsPacked = !IsPacked;
    }

}

