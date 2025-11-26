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

    public string ReadUserInput(string prompt)
    {
        while (true)
        {
            var res = ReadInput(prompt);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            // Keep asking until valid input is provided
            Console.WriteLine("Input can't be empty");
        }
    }

    public void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Garage Menu ===");
        Console.WriteLine("1. List parked vehicles");
        Console.WriteLine("2. Seed vehicles");
        Console.WriteLine("3. Park new vehicle");
        Console.WriteLine("4. Remove vehicle");
        Console.WriteLine("5. Find vehicle by registration number");
        Console.WriteLine("6. Find vehicles by properties");
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
        Console.WriteLine("=== Vehicles ===");
        Console.WriteLine("Choose vehicle type:");
        Console.WriteLine("1. Car");
        Console.WriteLine("2. Motorcykle");
        Console.WriteLine("3. Bus");
        Console.WriteLine("4. Boat");
        Console.WriteLine("5. Airplane");
    }

    public int ReadInt(string prompt)
    {
        while (true)
        {
            var s = ReadInput(prompt);
            if (int.TryParse(s, out var value))
                if (value >= 0)
                {
                    return value;
                }

            // Keep asking until valid positive integer is provided
            Console.WriteLine("Write a positive integer");
        }
    }

    public int ReadIntInRange(string prompt, int min, int max)
    {
        while (true)
        {
            var s = ReadInput(prompt);
            if (int.TryParse(s, out var value))
            {
                // Validate that value is within the specified range
                if (value >= min && value <= max)
                {
                    return value;
                }
            }

            Console.WriteLine($"Please enter a number between {min} and {max}");
        }
    }

    public int AskForCapacity()
    {
        while (true)
        {
            var input = ReadInput("Enter garage capacity (number of parking slots): ");

            // Validate capacity is a positive integer
            if (int.TryParse(input, out int capacity) && capacity > 0)
            {
                return capacity;
            }

            ShowMessage("Invalid capacity, please enter a positive integer.");
        }
    }

    public void ShowFuelTypeMenu()
    {
        Console.WriteLine("\nChoose fuel type:");
        Console.WriteLine("1. Gasoline");
        Console.WriteLine("2. Diesel");
        Console.WriteLine("3. Electric");
        Console.WriteLine("4. Hybrid");
        Console.WriteLine("5. None");
    }
}
