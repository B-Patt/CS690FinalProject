namespace PackingListApp.Interfaces;

public interface IPackingListRepository
{
    PackingList LoadList(string name);
    void SaveList(PackingList list);
    void RenameList(string oldName, string newName);
    void DeleteList(string name);
    List<string> ListAll();
    
}