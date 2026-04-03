namespace PackingListApp;

public class PackingListManager
{
    public PackingList CreateNewList(string name)
    {
        if (!Validator.IsValidListName(name))
            return null;

        return new PackingList(name);
    }
}

