using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Services;
using Ovning5_Garage_1_ConsoleApp.UI;

try
{
    // Initialize the console UI
    IUI ui = new ConsoleUI();

    // Ask user for garage capacity before creating the garage
    int capacity = ui.AskForCapacity();

    // Create handler with the specified capacity for the garage
    IHandler handler = new Handler(capacity);

    // Initialize the manager that coordinates UI and business logic
    Manager manager = new Manager(ui, handler);

    // Start the main application loop
    manager.Run();
}
catch(Exception ex)
{
    Console.WriteLine($"\nA critical error occurred: {ex.Message}");
    Console.WriteLine("The application will now exit.");
}
