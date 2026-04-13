namespace PackingListApp;

public class PackingListRepository
{
    private TextFileStorage storage;

    public PackingListRepository(TextFileStorage storage)
    {
        this.storage = storage;
    }
    public PackingList LoadList(string name) 
    {
        string fileName = name + ".txt";

        if (!storage.FileExists(fileName))
        {
            return null;
        }

        string contents = storage.ReadFile(fileName);

        if (string.IsNullOrWhiteSpace(contents))
        {
            return null;
        }

        string[] lines = contents.Split('\n');

        string listName = lines[0];
        PackingList list = new PackingList(listName);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(',');

            string itemName = parts[0];
            int quantity = int.Parse(parts[1]);
            bool isPacked = bool.Parse(parts[2]);

            PackingItem item = new PackingItem(itemName, quantity);
            item.SetPacked(isPacked);

            list.AddItem(item);
        }

        return list;
    }


    public void SaveList(PackingList list) 
    {
        List<string> lines = new List<string>();

        lines.Add(list.Name);

        foreach (PackingItem item in list.Items)
        {
            string line = item.Name + "," + item.Quantity + "," + item.IsPacked;
            lines.Add(line);
        }

        string contents = string.Join(Environment.NewLine, lines);

        string fileName = list.Name + ".txt";

        storage.WriteFile(fileName, contents);
    }

    public void RenameList(string oldName, string newName)
    {
        storage.RenameFile(oldName, newName);

        var list = LoadList(newName);
        if(list != null)
        {
            list.Rename(newName);
            SaveList(list);
        }
    }

    public void DeleteList(string name) 
    {
        string fileName = name + ".txt";
        storage.DeleteFile(fileName);
    }


    public List<string> ListAll()
     {
        List<string> files = storage.ListFiles();
        List<string> names = new List<string>();

        foreach (string file in files)
        {
            if (file.EndsWith(".txt"))
            {
                string listName = file.Replace(".txt", "");
                names.Add(listName);
            }
        }

        return names;
    }




}
