namespace PackingListApp.Application;

using PackingListApp.Interfaces;
using PackingListApp.Domain;

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
        repository.RenameList(oldName, newName);
    }

    public void DeleteList(string name)
    {
        repository.DeleteList(name);
    }

    public List<string> ListAll()
    {
        return repository.ListAll();
    }
}