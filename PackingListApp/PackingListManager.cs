namespace PackingListApp.Application;

using PackingListApp.Interfaces;
using PackingListApp.Domain;

public class PackingListManager : IPackingListManager

{
    private readonly TextFileStorage storage;
    private readonly PackingListRepository repo;

    public PackingListManager()
    {
        string folder = Path.Combine(AppContext.BaseDirectory, "PackingLists");
        storage = new TextFileStorage(folder);
        repo = new PackingListRepository(storage);
    }

    public PackingList CreateNewList(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var list = new PackingList(name);
        SaveList(list);
        return list;
    }

    public PackingList LoadList(string name)
    {
        return repo.LoadList(name);
    }

    public void SaveList(PackingList list)
    {
        repo.SaveList(list);
    }

    public void RenameList(string oldName, string newName)
    {
        repo.RenameList(oldName, newName);
    }

    public void DeleteList(string name)
    {
        repo.DeleteList(name);
    }

    public List<string> ListAllLists()
    {
        return repo.ListAll();
    }
}
