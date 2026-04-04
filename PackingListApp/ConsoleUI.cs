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
            AnsiConsole.MarkupLine("List not found.");
            return;
            }

        AnsiConsole.MarkupLine($"Loaded list: {list.Name}");
        AnsiConsole.MarkupLine("Items:");

        foreach (PackingItem item in list.Items)
        {
            string packed = item.IsPacked ? "Packed" : "Not Packed";
            AnsiConsole.MarkupLine($"- {item.Name} x{item.Quantity} ({packed})");
        }
    }

}

