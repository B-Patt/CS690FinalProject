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

            return PackingList.FromString(contents);
        }

        public void SaveList(PackingList list)
        {
            storage.WriteFile(list.Name, list.ToString());
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
            return storage.ListFiles();
        }
    }
}
