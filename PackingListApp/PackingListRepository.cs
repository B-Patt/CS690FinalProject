namespace PackingListApp.Infrastructure;

using PackingListApp.Interfaces;
using PackingListApp.Domain;

public class PackingListRepository : IPackingListRepository
{
    private readonly IStorage storage;

    public PackingListRepository(IStorage storage)
    {
        this.storage = storage;
    }

    public PackingList LoadList(string name) 
    {
        string contents = storage.ReadFile(name);

        if (string.IsNullOrWhiteSpace(contents))
        {
            return null;
        }

        return PackingList.FromString(contents);
    }

    public void SaveList(PackingList list)
    {
        string contents = list.ToString();
        storage.WriteFile(list.Name, contents);
    }

    public void RenameList(string oldName, string newName)
    {
        storage.RenameFile(oldName, newName);
    }

    public void DeleteList(string name) 
    {
        storage.DeleteFile(fileName);
    }

    public List<string> ListAll()
    {
        return Storage.ListFiles();
    }

}
