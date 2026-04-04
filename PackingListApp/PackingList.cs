namespace PackingListApp
{
    public class PackingList
    {
        private string name;
        private List<PackingItem> items;

        public string Name
        {
            get { return name; }
        }

        public List<PackingItem> Items
        {
            get { return items; }
        }

        public PackingList(string name)
        {
            this.name = name;
            this.items = new List<PackingItem>();
        }

        public void AddItem(PackingItem item)
        {
            items.Add(item);
        }

        public void RemoveItem(string itemName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == itemName)
                {
                    items.RemoveAt(i);
                    break;
                }
            }
        }

        public PackingItem FindItem(string itemName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == itemName)
                {
                    return items[i];
                }
            }

            return null;
        }
    }
}
