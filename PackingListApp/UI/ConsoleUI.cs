using Spectre.Console;
using PackingListApp.Interfaces;
using PackingListApp.Domain;

namespace PackingListApp.UI;

public class ConsoleUI
{
    private readonly IPackingListManager manager;

    public ConsoleUI(IPackingListManager manager)
    {
        this.manager = manager;
    }

    public void Run()
    {
         ShowSplashScreen();

        while (true)
        {
            ShowWelcomePanel();
            var choice = ShowMainMenu();

            switch (choice)
            {
                case "Create New List":
                    CreateNewPackingList();
                    break;

                case "Load Existing List":
                    LoadExistingPackingList();
                    break;

                case "Manage Saved List Files":
                    ShowSavedListsMenu();
                    break;

                case "Save & Exit Program":
                    return;
            }
        }
    }

    private void Space()
    {
        AnsiConsole.WriteLine();
    }

    private void ShowSplashScreen()
    {
        Console.Clear();

        AnsiConsole.Write(
        new FigletText("THE PACKING LIST APP")
            .Centered()
            .Color(Color.Aqua));     

        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    public string ShowMainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(
                    "Create New List",
                    "Load Existing List",
                    "Manage Saved List Files",
                    "Save & Exit Program"
                ));
    }

    private void ShowWelcomePanel()
    {
        Console.Clear();
        var content = new Markup("MAIN MENU").Centered();
        var panel = new Panel(content)
        {
            Border = BoxBorder.Heavy,
        };
        panel.Header = new PanelHeader("[aqua]WELCOME[/]").Centered();
        panel.Width = 35;
        panel.BorderColor(Color.Aqua);
        var paddedPanel = new Padder(panel, new Padding(0, 0, 0, 2));
        AnsiConsole.Write(paddedPanel);
    }

    private void CreateNewPackingList()
    {
        string name = AnsiConsole.Ask<string>("Enter new list name:");

        PackingList newList = manager.CreateList(name);

        if (newList == null)
        {
            AnsiConsole.MarkupLine("[red]Error: List already exists or name is invalid.[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to go back...[/]");
            Console.ReadKey(true); 
            return;
        }

        AnsiConsole.MarkupLine("List created.");
        Space();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do next?")
                .AddChoices("Load this list now", "Return to Main Menu"));

        if (choice == "Load this list now")
        {
            ShowListSplashScreen(newList);
            ManageItemsMenu(newList);
        }
    }

    private void ShowListSplashScreen(PackingList list)
    {
        Console.Clear();
        var content = new Markup($"[white]{list.Name} List[/]").Centered();
        var panel = new Panel(content)
        {
            Border = BoxBorder.Heavy,
        };
        panel.Header = new PanelHeader("[aqua]ITEM MENU[/]").Centered();
        panel.Width = 35;
        panel.BorderColor(Color.Aqua);
        var paddedPanel = new Padder(panel, new Padding(0, 0, 0, 1));
        AnsiConsole.Write(paddedPanel);

        ShowListContents(list);
    }

    private void LoadExistingPackingList()
    {
        var files = manager.ListAll();

        if (files.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No saved lists found.[/]");
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
            AnsiConsole.MarkupLine("[red]Failed to load list.[/]");
            return;
        }

        ShowListSplashScreen(list);
        ManageItemsMenu(list);
    }

    private void ShowListContents(PackingList list)
    {
        AnsiConsole.MarkupLine("[underline]Items:[/]");
        Space();

        if (list.Items.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey](No items yet)[/]");
            Space();
            return;
        }

        foreach (var item in list.Items)
        {
            string status = item.IsPacked ? "[green]Packed[/]" : "[red]Not Packed[/]";
            AnsiConsole.MarkupLine($"- {item.Name} [grey]x{item.Quantity}[/] ({status})");
        }

        Space();
        Space();
    }

    private void ManageItemsMenu(PackingList list)
    {
        string choice;

        do
        {
            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(
                        "Add Item",
                        "Edit Item Quantity",
                        "Remove Item",
                        "Packed Status",
                        "Sort Items",
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

            else if (choice == "Sort Items")
                ShowSortingMenu(list.Name);

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

        ShowListSplashScreen(updated);
        AnsiConsole.MarkupLine("[green]Item added.[/]");
        Space(); 
    }

    private void EditItemQuantity(PackingList list)
    {
        if (list.Items.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No items to edit.[/]");
            Space();
            return;
        }

        string itemName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cornflowerblue]Select an item to edit quantity[/]")
                .AddChoices(list.Items.Select(i => i.Name)));

        int qty = AnsiConsole.Ask<int>("New quantity:");

        manager.UpdateQuantity(list.Name, itemName, qty);
        var updated = manager.LoadList(list.Name);

        ShowListSplashScreen(updated);
        AnsiConsole.MarkupLine("[green]Quantity updated.[/]");
        Space();
    }

    private void RemoveItemFromList(PackingList list)
    {
        if (list.Items.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No items to remove.[/]");
            return;
        }

        var choices = list.Items.Select(i => i.Name).ToList();
        choices.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cornflowerblue]Select an item to remove [/]")
                .AddChoices(choices));

        if (choice == "Back")
        {
            ShowListSplashScreen(list);    
            return;
        }

        manager.RemoveItem(list.Name, choice);
        var updated = manager.LoadList(list.Name);

        ShowListSplashScreen(updated);
        AnsiConsole.MarkupLine("[fuchsia]Item removed.[/]");
        Space();
    }

    private void TogglePackedStatus(PackingList list)
    {
        if (list.Items.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No items to update.[/]");
            return;
        }

        var choices = list.Items.Select(i => i.Name).ToList();
        choices.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cornflowerblue]Select an item to toggle packed status[/]")
                .AddChoices(choices));

        if (choice == "Back")
        {
            ShowListSplashScreen(list);
            return;
        }

        manager.TogglePacked(list.Name, choice);
        var updated = manager.LoadList(list.Name);

        ShowListSplashScreen(updated);
        AnsiConsole.MarkupLine("[green]Packed status updated.[/]");
        Space();
    }

    private void ShowSavedListsMenu()
    {
        var lists = manager.ListAll();

        if (lists.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No saved lists found.[/]");
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

    private void ManagementSplashScreen(string listName)
    {
        Console.Clear();
        var content = new Markup($"[white]{listName} List[/]").Centered();
        var panel = new Panel(content)
        {
            Border = BoxBorder.Heavy,
        };
        panel.Header = new PanelHeader("[aqua]FILE MANAGEMENT MENU[/]").Centered();
        panel.Width = 35;
        panel.BorderColor(Color.Aqua);
        var paddedPanel = new Padder(panel, new Padding(0, 0, 0, 2));
        AnsiConsole.Write(paddedPanel);
    }

    private void ShowListActionsMenu(string listName)
    {
        string choice;

        do
        {
            ManagementSplashScreen(listName);

            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(
                        "View List",
                        "Rename List",
                        "Delete List",
                        "Reset Items to Default (Unpacked)",
                        "Reset Quantity Default (1)",
                        "Reset All (Unpacked + Quantity Default)",
                        "Back",
                        "Save & Exit Program"));

            if (choice == "View List")
            {
                var list = manager.LoadList(listName); 
                if (list == null)
                {
                    AnsiConsole.MarkupLine("[red]List could not be found.[/]");
                    return;
                }
                ShowListSplashScreen(list);
                ManageItemsMenu(list);
            }
            else if (choice == "Rename List")
            {
                var newName = AnsiConsole.Ask<string>("Enter new name:");
                bool success = manager.RenameList(listName, newName);

                if (!success)
                {
                    AnsiConsole.MarkupLine("[red]Rename failed: name already exists or list not found.[/]");
                    return;
                }

                AnsiConsole.MarkupLine("[green]List renamed successfully![/]");
                Space();
                listName = newName;
            }
            else if (choice == "Delete List")
            {
                DeleteListFlow(listName);
                return;
            }
            else if (choice == "Reset Items to Default (Unpacked)")
            {
                manager.ClearPackedStatus(listName);
                AnsiConsole.MarkupLine("All items marked as unpacked.");
                Space();
                AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
                Console.ReadKey(true);
            }
            else if (choice == "Reset Quantity Default (1)")
            {
                manager.ResetQuantities(listName);
                AnsiConsole.MarkupLine("All item quantities reset to 1.");
                Space();
                AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
                Console.ReadKey(true);    
            }
            else if (choice == "Reset All (Unpacked + Quantity Default)")
            {
                manager.ResetAll(listName);
                AnsiConsole.MarkupLine("All items reset.");
                Space();
                AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
                Console.ReadKey(true);
            }
            else if (choice == "Save & Exit Program")
            {
                Environment.Exit(0);
            }

        } while (choice != "Back");
    }

    private void DeleteListFlow(string listName)
    {
        string confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Delete \"{listName}\"?")
                .AddChoices("Yes", "No"));

        if (confirm == "Yes")
        {
            manager.DeleteList(listName);
            AnsiConsole.MarkupLine("[red]List deleted.[/]");
            Space();
        }
    }

    private void ShowSortingMenu(string listName)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[cornflowerblue]Sorting Options for {listName}[/]")
                .AddChoices(
                    "Quantity (High to Low)",
                    "Quantity (Low to High)",
                    "Packed Status (Not Packed First)",
                    "Packed Status (Packed First)",
                    "Alphabetical (A to Z)",
                    "Back"));

        switch (choice)
        {
            case "Quantity (High to Low)":
                manager.SortByQuantity(listName, true);
                break;
            case "Quantity (Low to High)":
                manager.SortByQuantity(listName, false);
                break;
            case "Packed Status (Not Packed First)":
                manager.SortByPackedStatus(listName, true);
                break;
            case "Packed Status (Packed First)":
                manager.SortByPackedStatus(listName, false);
                break;
            case "Alphabetical (A to Z)":
                manager.SortAlphabetically(listName);
                break;
            case "Back":
                return;
        }

        var updated = manager.LoadList(listName);
        ShowListSplashScreen(updated);
    }
}
