namespace PackingListApp.Domain
{
    public class PackingList
    {
        public string Name { get; private set; }
        public List<PackingItem> Items { get; } = new();

        public PackingList(string name)
        {
            Name = name;
        }

        public void Rename(string newName)
        {
            Name = newName;
        }

        public void AddItem(PackingItem item)
        {
            if (Items.Any(i => i.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Duplicate item name.");

            Items.Add(item);
        }

        public bool RemoveItem(string itemName)
        {
            var item = Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null)
                return false;

            Items.Remove(item);
            return true;
        }

        public PackingItem FindItem(string itemName)
        {
            return Items.FirstOrDefault(i => i.Name == itemName);
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

        public void ClearPackedStatus()
        {
            foreach (var item in Items)
                item.MarkUnpacked();
        }

        public void ResetQuantitiesToDefault(int defaultQty = 1)
        {
            foreach (var item in Items)
                item.ResetQuantity(defaultQty);
        }

        public void ResetAll()
        {
            foreach (var item in Items)
            {
                item.MarkUnpacked();
                item.ResetQuantity();
            }
        }
    }
}
