using PackingListApp.Interfaces;
using PackingListApp.Domain;

namespace PackingListApp.Application;

public class PackingListManager : IPackingListManager
{
    private readonly IPackingListRepository repository;

    public PackingListManager(IPackingListRepository repository)
    {
        this.repository = repository;
    }

    public PackingList CreateList(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var list = new PackingList(name);
        repository.SaveList(list);
        return list;
    }

    public PackingList LoadList(string name)
    {
        return repository.LoadList(name);
    }

    public void SaveList(PackingList list)
    {
        repository.SaveList(list);
    }

    public bool RenameList(string oldName, string newName)
    {
        var list = repository.LoadList(oldName);
        if (list == null)
            return false;

        var allLists = repository.ListAll();
        if (allLists.Any(n => n.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            return false;

        list.Rename(newName);
        repository.RenameList(oldName, newName);
        repository.SaveList(list);

        return true;
    }

    public void DeleteList(string name)
    {
        repository.DeleteList(name);
    }
    public void AddItem(string listName, string itemName, int quantity)
    {
        var list = repository.LoadList(listName);
        list.AddItem(new PackingItem(itemName, quantity));
        repository.SaveList(list);
    }

    public void UpdateQuantity(string listName, string itemName, int quantity)
    {
        var list = repository.LoadList(listName);
        var item = list.FindItem(itemName);
        item.SetQuantity(quantity);
        repository.SaveList(list);
    }

    public void RemoveItem(string listName, string itemName)
    {
        var list = repository.LoadList(listName);
        list.RemoveItem(itemName);
        repository.SaveList(list);
    }

    public void TogglePacked(string listName, string itemName)
    {
        var list = repository.LoadList(listName);
        list.TogglePacked(itemName);
        repository.SaveList(list);
    }

    public List<string> ListAll()
    {
        return repository.ListAll();
    }

    public void ClearPackedStatus(string listName)
    {
        var list = LoadList(listName);
        list.ClearPackedStatus();
        repository.SaveList(list);
    }

    public void ResetQuantities(string listName)
    {
        var list = LoadList(listName);
        list.ResetQuantitiesToDefault();
        SaveList(list);
    }

    public void ResetAll(string listName)
    {
        var list = LoadList(listName);
        list.ResetAll();
        SaveList(list);
    }

    public IEnumerable<PackingItem> SortByQuantity(string listName, bool descending = true)
    {
    var list = repository.LoadList(listName);
    var sorted = list.GetItemsSortedByQuantity(descending);
    list.ApplySort(sorted);
    repository.SaveList(list);
    return sorted;
    }

    public IEnumerable<PackingItem> SortByPackedStatus(string listName, bool packedFirst = false)
    {
    var list = repository.LoadList(listName);
    var sorted = list.GetItemsSortedByPackedStatus(packedFirst);
    list.ApplySort(sorted);
    repository.SaveList(list);
    return sorted;
    }

    public IEnumerable<PackingItem> SortAlphabetically(string listName)
    {
        var list = repository.LoadList(listName);
        var sorted = list.GetItemsSortedAlphabetically();
        list.ApplySort(sorted);
        repository.SaveList(list);
        return sorted;
    }
}