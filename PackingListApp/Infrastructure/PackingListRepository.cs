using PackingListApp.Domain;
using PackingListApp.Interfaces;

namespace PackingListApp.Infrastructure
{
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
                return null;

            var lines = contents
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var list = new PackingList(name);

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length != 3)
                    continue;

                string itemName = parts[0];
                int quantity = int.Parse(parts[1]);
                bool isPacked = bool.Parse(parts[2]);

                list.AddItem(itemName, quantity);

                if (isPacked)
                    list.TogglePacked(itemName);
            }

            return list;
        }

        public void SaveList(PackingList list)
        {
            var lines = list.Items
                .Select(i => $"{i.Name}|{i.Quantity}|{i.IsPacked}")
                .ToList();

            string contents = string.Join(Environment.NewLine, lines);

            storage.WriteFile(list.Name, contents);
        }

        public bool RenameList(string oldName, string newName)
        {
            return storage.RenameFile(oldName, newName);
        }

        public void DeleteList(string name)
        {
            storage.DeleteFile(name);
        }

        public List<string> ListAll()
        {
            return storage.ListFiles()
                .Select(f => f) // already stripped by TextFileStorage
                .ToList();
        }
    }
}