using PackingListApp.Domain;

namespace PackingListApp.Interfaces;

public interface IPackingListManager
{
   PackingList CreateList(string name);
   PackingList LoadList(string name);
   void SaveList(PackingList list);
   bool RenameList(string oldName, string newName);
   void DeleteList(string name);
   List<string> ListAll();
   void AddItem(string listName, string itemName, int quantity);
   void UpdateQuantity(string listName, string itemName, int quantity);
   void RemoveItem(string listName, string itemName);
   void TogglePacked(string listName, string itemName);
   void ClearPackedStatus(string listName);
   void ResetQuantities(string listName);
   void ResetAll(string listName);
}