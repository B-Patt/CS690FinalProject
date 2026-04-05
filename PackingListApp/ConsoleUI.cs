namespace PackingListApp;

using Spectre.Console;

public class ConsoleUI
{
    private PackingListManager manager = new PackingListManager();
    public void Show()
    {
        string choice;

        do
        {
           choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Packing List Main Menu")
                .AddChoices(
                    "Create New Packing List",
                    "Load Existing Packing List",
                    "Manage Saved Packing Lists",
                    "Exit"));
            
            if (choice == "Create New Packing List")
            {
                CreateNewPackingList();
            }
            else if (choice == "Load Existing Packing List")
            {
                LoadExistingPackingList();
            }
            

        } while (choice != "Exit");
    }

    private void CreateNewPackingList()
    {
        string name = AnsiConsole.Ask<string>("Enter new list name: ");

        PackingList newList = manager.CreateNewList(name);

        if (newList == null)
        {
            AnsiConsole.WriteLine("Invalid list name");
            return;
        }
        AnsiConsole.WriteLine("List Created.");
    }

    private void LoadExistingPackingList()
    {
        string name = AnsiConsole.Ask<string>("Enter the name of the list to load:");

        PackingList list = manager.LoadList(name);

        if (list == null)
            {
            AnsiConsole.WriteLine("List not found.");
            return;
            }

        ShowListContents(list);

       // Enter List menu
        ManageItemsMenu(list);
    }
    
    private void ShowListContents(PackingList list)
    {
        AnsiConsole.WriteLine("Loaded list: {list.Name}");
        AnsiConsole.WriteLine("Items:");

        if(list.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items yet.");
            return;
        }

        foreach (PackingItem item in list.Items)
        {
            string packed = item.IsPacked ? "Packed" : "Not Packed";
            AnsiConsole.WriteLine("- {item.Name} x{item.quantity} ({packed})")
        }
    }

    private void ManageItemsMenu(PackingList list)
{
    string choice;

    do
    {
        choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Manage Items in {list.Name}")
                .AddChoices(
                    "Add Item",
                    "Edit Item Quantity",
                    "Remove Item",
                    "Packed Status",
                    "Back"));

        if (choice == "Add Item")
            AddItemToList(list);

        else if (choice == "Edit Item Quantity")
            EditItemQuantity(list);

        else if (choice == "Remove Item")
            RemoveItemFromList(list);

        else if (choice == "Packed Status")
            TogglePackedStatus(list);

    } while (choice != "Back");

    manager.SaveList(list);


}

}

