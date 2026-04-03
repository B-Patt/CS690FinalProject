namespace PackingListApp;

using Spectre.Console;

public class ConsoleUI
{
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

        } while (choice != "Exit");
    }
}

