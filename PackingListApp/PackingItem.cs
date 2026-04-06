namespace PackingListApp
{
    public class PackingItem
    {
        private string name;
        private int quantity;
        private bool isPacked;

        public string Name
        {
            get { return name; }
        }

        public int Quantity
        {
            get { return quantity; }
        }

        public bool IsPacked
        {
            get { return isPacked; }
        }

        public PackingItem(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
            this.isPacked = false;
        }

        public void MarkPacked()
        {
            isPacked = true;
        }

        public void MarkUnpacked()
        {
            isPacked = false;
        }

        public void SetPacked(bool value)
        {
            isPacked = value;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }
    }
}