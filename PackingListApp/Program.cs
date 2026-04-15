using PackingListApp.UI;
using PackingListApp.Infrastructure;
using PackingListApp.Application;

namespace PackingListApp;

class Program
{
    static void Main()
    {
        var storage = new TextFileStorage("PackingLists");
        var repository = new PackingListRepository(storage);
        var manager = new PackingListManager(repository);

        var ui = new ConsoleUI(manager);
        ui.Run();
    }
}