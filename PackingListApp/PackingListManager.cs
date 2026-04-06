namespace PackingListApp;

public class PackingListManager
{
    private PackingListRepository repository;

    public PackingListManager()
    {
        TextFileStorage storage = new TextFileStorage("PackingLists");

        repository = new PackingListRepository(storage);
    }


    public PackingList CreateNewList(string name)
    {
        if (!Validator.IsValidListName(name))
        {
            return null;
        }

        //Check Duplicates
        List<string> existing = repository.ListAll();
        for (int i = 0; i < existing.Count; i++)
        {
            if (existing[i].ToLower() == name.ToLower())
            {
                return null;
            }
        }

        PackingList list = new PackingList(name);

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

    public void DeleteList(string name)
    {
        repository.DeleteList(name);
    }

    public List<string> ListAllLists()
    {
        return repository.ListAll();
    }

}

