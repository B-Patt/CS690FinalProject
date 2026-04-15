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

    public void RenameList(string oldName, string newName)
    {
        // Renaming List to duplicate list name error
        if (repository.ListAll().Contains(newName))
        throw new InvalidOperationException("A list with that name already exists.");

        repository.RenameList(oldName, newName);

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
        var item = list.FindItem(itemName);
        item.SetPacked(!item.IsPacked);
        repository.SaveList(list);
    }

    public List<string> ListAll()
    {
        return repository.ListAll();
    }
}