namespace PackingListApp.Domain;

public class PackingItem
{
    public string Name { get; }
    public int Quantity { get; private set; }
    public bool IsPacked { get; private set; }

    public PackingItem(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
        IsPacked = false;
    }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }

    public void SetPacked(bool value)
    {
        IsPacked = value;
    }

    public void MarkUnpacked()
    {
        IsPacked = false;
    }
    
}

