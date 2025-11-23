using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.UI;

IUI ui = new ConsoleUI();
int capacity = AskForCapacity(ui);

var handler = new Handler(capacity);
var manager = new Manager(ui, handler);

manager.Run();

static int AskForCapacity(IUI ui)
{
    while (true)
    {
        var input = ui.ReadInput("Enter garage capacity (number of parking slots): ");

        if (int.TryParse(input, out int capacity) && capacity > 0)
        {
            return capacity;
        }

        ui.ShowMessage("Invalid capacity, please enter a positive integer.");
    }
}
