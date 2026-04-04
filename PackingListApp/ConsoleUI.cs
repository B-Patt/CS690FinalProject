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
                .Title("Packing List")
                .AddChoices(
                    "Create New Packing List",
                    "Load Existing Packing List",
                    "Manage Saved Packing Lists",
                    "Exit"));
            
            if(choice == "Create New Packing List")
            {
                CreateNewPackingList();
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


}

