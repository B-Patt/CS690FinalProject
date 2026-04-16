using PackingListApp.Interfaces;
using PackingListApp.Domain;

namespace PackingListApp.Infrastructure;

public class PackingListRepository : IPackingListRepository
{
    private readonly IStorage storage;

    public PackingListRepository(IStorage storage)
    {
        this.storage = storage;
    }

    public PackingList LoadList(string name)
    {
        string fileName = name + ".txt";
        string contents = storage.ReadFile(fileName);

        if (string.IsNullOrWhiteSpace(contents))
            return null;

        return PackingList.FromString(contents);
    }

    public void SaveList(PackingList list)
    {
        string fileName = list.Name + ".txt";
        string contents = list.ToString();
        storage.WriteFile(fileName, contents);
    }

public bool RenameList(string oldName, string newName)
    {
        string oldFile = oldName + ".txt";
        string newFile = newName + ".txt";

        
        if (storage.FileExists(newFile))
            return false;

    
        storage.RenameFile(oldFile, newFile);

        
        var list = LoadList(newName);
        if (list == null)
            return false;

        list.Rename(newName);
        SaveList(list);

        return true;
    }

    public void DeleteList(string name)
    {
        string fileName = name + ".txt";
        storage.DeleteFile(fileName);
    }

    public List<string> ListAll()
    {
        return storage.ListFiles()
                      .Where(f => f.EndsWith(".txt"))
                      .Select(f => f.Replace(".txt", ""))
                      .ToList();
    }

    
}

