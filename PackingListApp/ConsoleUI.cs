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

        string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What would you like to do next?")
            .AddChoices(
                "Load this list now",
                "Return to Main Menu"));

    if (choice == "Load this list now")
    {
        //Back to main menu or load new list
        ShowListContents(newList);
        ManageItemsMenu(newList);
    }
    else
    {
        return;
    }
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
        AnsiConsole.WriteLine($"Loaded list: {list.Name}");
        AnsiConsole.WriteLine("Items:");

        if(list.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items yet.");
            return;
        }

        foreach (PackingItem item in list.Items)
        {
            string packed = item.IsPacked ? "Packed" : "Not Packed";
            AnsiConsole.WriteLine($"- {item.Name} x{item.Quantity} ({packed})");
        }
    }

    private void ManageItemsMenu(PackingList list)
    {
        string choice;

        do
        {
            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Manage Items in {list.Name}")
                    .AddChoices(
                        "Add Item",
                        "Edit Item Quantity",
                        "Remove Item",
                        "Packed Status",
                        "Back",
                        "Save & Exit Program"));

            if (choice == "Add Item")
                AddItemToList(list);

            else if (choice == "Edit Item Quantity")
                EditItemQuantity(list);

            else if (choice == "Remove Item")
                RemoveItemFromList(list);

            else if (choice == "Packed Status")
                TogglePackedStatus(list);

            else if (choice == "Save & Exit Program")
            {
                manager.SaveList(list);
                Environment.Exit(0);
            }
        } while (choice != "Back");

    manager.SaveList(list);
}


private void AddItemToList(PackingList list)
{
    string name = AnsiConsole.Ask<string>("Item name:");
    int qty = AnsiConsole.Ask<int>("Quantity:");

    list.AddItem(new PackingItem(name, qty));
    AnsiConsole.WriteLine("Item added.");

    ShowListContents(list);
}

private void EditItemQuantity(PackingList list)
{
    if (list.Items.Count == 0)
    {
        AnsiConsole.WriteLine("No items to edit.");
        return;
    }

    string itemName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select an item to edit quantity")
            .AddChoices(list.Items.Select(i => i.Name)));

    PackingItem item = list.FindItem(itemName);

    int qty = AnsiConsole.Ask<int>("New quantity:");
    item.SetQuantity(qty);

    AnsiConsole.WriteLine("Quantity updated.");

    ShowListContents(list);

}


private void RemoveItemFromList(PackingList list)
{
    if (list.Items.Count == 0)
    {
        AnsiConsole.WriteLine("No items to remove.");
        return;
    }

    // Build choices + Back option
    var choices = list.Items.Select(i => i.Name).ToList();
    choices.Add("Back");

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select an item to remove")
            .AddChoices(choices));

    if (choice == "Back")
    {
        return;
    }

    bool removed = list.RemoveItem(choice);

    if (removed)
        AnsiConsole.WriteLine("Item removed.");
    else
        AnsiConsole.WriteLine("Item not found.");
    ShowListContents(list);
}

private void TogglePackedStatus(PackingList list)
{
    if (list.Items.Count == 0)
    {
        AnsiConsole.WriteLine("No items to update.");
        return;
    }

    // Build choices + Back option
    var choices = list.Items.Select(i => i.Name).ToList();
    choices.Add("Back");

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select an item to toggle packed status")
            .AddChoices(choices));

    if (choice == "Back")
    {
        return;
    }

    PackingItem item = list.FindItem(choice);

    item.SetPacked(!item.IsPacked);

    AnsiConsole.WriteLine(
        $"Packed status updated: {item.Name} is now {(item.IsPacked ? "Packed" : "Not Packed")}");

    ShowListContents(list);
}

}