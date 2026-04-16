namespace PackingListApp.Domain
{
    public class PackingList
    {
        private List<PackingItem> items;

        public string Name { get; private set; }
        public IReadOnlyList<PackingItem> Items => items;

        public PackingList(string name)
        {
            Name = name;
            items = new List<PackingItem>();
        }

        public void AddItem(PackingItem item)
        {
            items.Add(item);
        }

        public bool RemoveItem(string itemName)
        {
            var item = FindItem(itemName);
            if (item == null)
                return false;

            items.Remove(item);
            return true;
        }

        public PackingItem FindItem(string itemName)
        {
            return items.FirstOrDefault(i => i.Name == itemName);
        }

        public void Rename(string newName)
        {
            Name = newName;
        }


        public override string ToString()
        {
            List<string> lines = new();
            lines.Add(Name);

            foreach (var item in Items)
                lines.Add($"{item.Name},{item.Quantity},{item.IsPacked}");

            return string.Join(Environment.NewLine, lines);
        }

        public static PackingList FromString(string contents)
        {
            var lines = contents.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var list = new PackingList(lines[0].Trim());

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                var item = new PackingItem(parts[0], int.Parse(parts[1]));
                item.SetPacked(bool.Parse(parts[2]));
                list.AddItem(item);
            }

            return list;
        }

    }
}
