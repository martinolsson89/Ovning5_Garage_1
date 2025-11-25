using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

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
        Console.WriteLine("2. Seed vehicles");
        Console.WriteLine("3. Park new vehicle");
        Console.WriteLine("4. Remove vehicle");
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

    public void ShowAddVehicleSubMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Add vehicle ===");
        Console.WriteLine("Choose vehicle type:");
        Console.WriteLine("1. Car");
        Console.WriteLine("2. Motorcykle");
        Console.WriteLine("3. Bus");
        Console.WriteLine("4. Boat");
        Console.WriteLine("5. Airplane");
        Console.Write("Choose an option: ");
    }

    public int ReadInt(string prompt)
    {
        while (true)
        {
            var s = ReadInput(prompt);
            if (int.TryParse(s, out var value))
            if(value >= 0)
                {
                    return value;
                }

            Console.WriteLine("Write a positive integer");
        }
    }
}
