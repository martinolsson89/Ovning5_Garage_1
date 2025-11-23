using Ovning5_Garage_1_ConsoleApp.Interfaces;

namespace Ovning5_Garage_1_ConsoleApp.UI;

public class ConsoleUI : IUI
{
    public string ReadInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Garage Menu ===");
        Console.WriteLine("1. List parked vehicles");
        Console.WriteLine("2. Park new vehicle");
        Console.WriteLine("3. Remove vehicle");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }

    public void ShowVehiclesSubMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== List parked Vehicles ===");
        Console.WriteLine("1. List all parked vehicles");
        Console.WriteLine("2. List vehicle parked types");
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}
