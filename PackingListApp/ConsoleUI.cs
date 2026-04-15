namespace PackingListApp.UI;

using Spectre.Console;
using PackingListApp.Interfaces;
using PackingListApp.Domain;

public class ConsoleUI
{
    private readonly IPackingListManager manager;

    public ConsoleUI(IPackingListManager manager)
    {
        this.manager = manager;
    }

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
                CreateNewPackingList();

            else if (choice == "Load Existing Packing List")
                LoadExistingPackingList();

            else if (choice == "Manage Saved Packing Lists")
                ShowSavedListsMenu();

        } while (choice != "Exit");
    }

    private void CreateNewPackingList()
    {
        string name = AnsiConsole.Ask<string>("Enter new list name:");

        PackingList newList = manager.CreateList(name);

        if (newList == null)
        {
            AnsiConsole.WriteLine("Invalid list name.");
            return;
        }

        AnsiConsole.WriteLine("List created.");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do next?")
                .AddChoices("Load this list now", "Return to Main Menu"));

        if (choice == "Load this list now")
        {
            ShowListContents(newList);
            ManageItemsMenu(newList);
        }
    }

    private void LoadExistingPackingList()
    {
        var files = manager.ListAll();

        if (files.Count == 0)
        {
            AnsiConsole.WriteLine("No saved lists found.");
            return;
        }

        var choices = new List<string>(files);
        choices.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a list to load")
                .AddChoices(choices));

        if (choice == "Back")
            return;

        var list = manager.LoadList(choice);

        if (list == null)
        {
            AnsiConsole.WriteLine("Failed to load list.");
            return;
        }

        ShowListContents(list);
        ManageItemsMenu(list);
    }

    private void ShowListContents(PackingList list)
    {
        AnsiConsole.WriteLine($"Loaded list: {list.Name}");
        AnsiConsole.WriteLine("Items:");

        if (list.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items yet.");
            return;
        }

        foreach (var item in list.Items)
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

            list = manager.LoadList(list.Name);

        } while (choice != "Back");

        manager.SaveList(list);
    }

    private void AddItemToList(PackingList list)
    {
        string name = AnsiConsole.Ask<string>("Item name:");
        int qty = AnsiConsole.Ask<int>("Quantity:");

        manager.AddItem(list.Name, name, qty);

        var updated = manager.LoadList(list.Name);

        AnsiConsole.WriteLine("Item added.");
        ShowListContents(updated);
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

        int qty = AnsiConsole.Ask<int>("New quantity:");

        manager.UpdateQuantity(list.Name, itemName, qty);

        var updated = manager.LoadList(list.Name);

        AnsiConsole.WriteLine("Quantity updated.");
        ShowListContents(updated);
    }

    private void RemoveItemFromList(PackingList list)
    {
        if (list.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items to remove.");
            return;
        }

        var choices = list.Items.Select(i => i.Name).ToList();
        choices.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an item to remove")
                .AddChoices(choices));

        if (choice == "Back")
            return;

        manager.RemoveItem(list.Name, choice);

        var updated = manager.LoadList(list.Name);

        AnsiConsole.WriteLine("Item removed.");
        ShowListContents(updated);
    }

    private void TogglePackedStatus(PackingList list)
    {
        if (list.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items to update.");
            return;
        }

        var choices = list.Items.Select(i => i.Name).ToList();
        choices.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an item to toggle packed status")
                .AddChoices(choices));

        if (choice == "Back")
            return;

        manager.TogglePacked(list.Name, choice);

        var updated = manager.LoadList(list.Name);

        AnsiConsole.WriteLine("Packed status updated.");
        ShowListContents(updated);
    }

    private void ShowSavedListsMenu()
    {
        var lists = manager.ListAll();

        if (lists.Count == 0)
        {
            AnsiConsole.WriteLine("No saved lists found.");
            return;
        }

        var choices = lists.ToList();
        choices.Add("Back");
        choices.Add("Save & Exit Program");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select saved lists:")
                .AddChoices(choices));

        if (choice == "Back")
            return;

        if (choice == "Save & Exit Program")
            Environment.Exit(0);

        ShowListActionsMenu(choice);
    }

    private void ShowListActionsMenu(string listName)
    {
        string choice;

        do
        {
            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Manage \"{listName}\"")
                    .AddChoices(
                        "Load (View) List",
                        "Rename List",
                        "Delete List",
                        "Back",
                        "Save & Exit Program"));

            if (choice == "Load (View) List")
            {
                var list = manager.LoadList(listName);

                if (list == null)
                {
                    AnsiConsole.WriteLine("List could not be found.");
                    return;
                }

                ShowListContents(list);
                ManageItemsMenu(list);
            }
            else if (choice == "Rename List")
            {
                listName = RenameListFlow(listName);
            }
            else if (choice == "Delete List")
            {
                DeleteListFlow(listName);
                return;
            }
            else if (choice == "Save & Exit Program")
            {
                Environment.Exit(0);
            }

        } while (choice != "Back");
    }

    private string RenameListFlow(string oldName)
    {
        string newName = AnsiConsole.Ask<string>("Enter new list name:");

        manager.RenameList(oldName, newName);

        AnsiConsole.WriteLine("List renamed.");

        return newName;
    }

    private void DeleteListFlow(string listName)
    {
        string confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Delete \"{listName}\"?")
                .AddChoices("Yes", "No"));

        if (confirm == "Yes")
            manager.DeleteList(listName);

        AnsiConsole.WriteLine("List deleted.");
    }
}
