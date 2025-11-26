using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;

namespace Ovning5_Garage_1_ConsoleApp.UI;

public class Manager
{
    private IUI _ui;
    private IHandler _handler;

    public Manager(IUI ui, IHandler handler)
    {
        _ui = ui;
        _handler = handler;
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            _ui.ShowMenu();
            var choice = _ui.ReadInput("Select menu option: ");

            // Handle user menu selection
            switch (choice)
            {
                case "1":
                    VehiclesSubMenu();
                    break;

                case "2":
                    SeedVehicles();
                    break;

                case "3":
                    AddVehicle();
                    break;

                case "4":
                    RemoveVehicle();
                    break;

                case "5":
                    FindVehicleByRegNr();
                    break;

                case "6":
                    FindVehiclebyProp();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    _ui.ShowMessage("Invalid choice, try again.");
                    break;
            }
        }
    }

    private void FindVehiclebyProp()
    {
        _ui.ShowMessage("\n=== Search Vehicles by Properties ===");
        _ui.ShowMessage("Enter search criteria (leave blank to skip):");

        // Collect search criteria from user
        _ui.ShowAddVehicleSubMenu();
        var vehicleTypeInput = _ui.ReadInput("Enter choice (1–5):");
        var color = _ui.ReadInput("Color: ");
        var wheelsInput = _ui.ReadInput("Number of wheels: ");
        var fuelTypeInput = _ui.ReadInput("Fuel type (1=Gasoline, 2=Diesel, 3=Electric, 4=Hybrid, 5=None): ");

        // Parse user inputs into proper types
        string? vehicleType = GetVehicleType(vehicleTypeInput);
        int? wheels = CheckWeelsInput(wheelsInput);
        FuelType? fuelType = ParseFuelType(fuelTypeInput);

        // Search garage with the provided criteria
        var results = _handler.GetVehicles(vehicleType, color, wheels, fuelType);

        // Display search results
        if (results.Count() == 0)
        {
            _ui.ShowMessage("\nNo vehicles found matching the criteria.");
        }
        else
        {
            _ui.ShowMessage($"\nFound {results.Count()} vehicle(s):");
            foreach (var vehicle in results)
            {
                _ui.ShowMessage(vehicle.ToString());
            }
        }
    }

    private void FindVehicleByRegNr()
    {
        var reg = _ui.ReadInput("\nEnter registration number: ");
        var res = _handler.GetVehicleByRegNr(reg);

        // Check if vehicle was found
        if (res is null)
        {
            _ui.ShowMessage("\nVehicle was not found.");
        }
        else
        {
            _ui.ShowMessage($"\n{res}");
        }
    }

    private void RemoveVehicle()
    {
        try
        {
            var regNr = _ui.ReadInput("\nEnter Reg Number to remove vehicle: ");
            var result = _handler.RemoveVehicle(regNr);

            // Display removal result to user
            if (result)
            {
                _ui.ShowMessage($"\nVehicle with {regNr.ToUpper()} was successfully removed from garage!");
            }
            else
            {
                _ui.ShowMessage($"\nVehicle with {regNr.ToUpper()} was not found in the garage");
            }
        }
        catch (ArgumentNullException)
        {
            _ui.ShowMessage("\nRegistration number cannot be empty.");
        }
        catch (Exception ex)
        {
            _ui.ShowMessage($"\nError removing vehicle: {ex.Message}");
        }
    }

    private void AddVehicle()
    {
        try
        {
            while (true)
            {
                // Collect vehicle information from user
                _ui.ShowAddVehicleSubMenu();
                var typeChoice = _ui.ReadIntInRange("Select menu option: ", 1, 5);
                var color = _ui.ReadUserInput("Enter color: ");
                var wheels = _ui.ReadInt("Enter number of wheels: ");
                var fuelType = AskFuelType();

                // Attempt to park the vehicle
                var (result, parkedVehicle) = _handler.ParkVehicle(typeChoice, color, wheels, fuelType);

                // Handle different parking results
                switch (result)
                {
                    case ParkResult.Success:
                        _ui.ShowMessage(
                            $"\nVehicle parked successfully. Reg: {parkedVehicle!.RegistrationNumber}");
                        break;

                    case ParkResult.AlreadyInGarage:
                        _ui.ShowMessage("\nThat vehicle is already parked in the garage.");
                        break;

                    case ParkResult.GarageIsFull:
                        _ui.ShowMessage("\nGarage is full. Could not park vehicle.");
                        break;
                }
                return;
            }
        }
        catch (ArgumentNullException ex)
        {
            _ui.ShowMessage($"\nError: Required information is missing. {ex.Message}");
        }
        catch (Exception ex)
        {
            _ui.ShowMessage($"\nError adding vehicle: {ex.Message}");
        }
    }

    private void SeedVehicles()
    {
        try
        {
            _ui.ShowMessage("\n=== Seed Vehicles ===");
            _ui.ShowMessage($"Available spots in garage: {_handler.GetAvailableSpots()}");

            int requested = _ui.ReadInt("\nHow many vehicles do you want to seed? ");
            var result = _handler.SeedGarage(requested);

            // Display seeding result based on success and available spots
            if (!result.Success)
            {
                if (result.AvailableSpots <= 0)
                    _ui.ShowMessage("\nGarage is full, could not seed more vehicles.");
                else
                    _ui.ShowMessage("\nNo vehicles where seeded.");
            }
            else
            {
                // Check if only partial seeding was possible
                if (result.SeededCount < result.RequestedCount)
                {
                    _ui.ShowMessage(
                        $"\nSeeded {result.SeededCount} vehicles (you asked for {result.RequestedCount}, but was only {result.AvailableSpots} free spots in garage).");
                }
                else
                {
                    _ui.ShowMessage($"\nSeeded {result.SeededCount} vehicles.");
                }
            }
        }
        catch (Exception ex)
        {
            _ui.ShowMessage($"\nError seeding vehicles: {ex.Message}");
        }

    }

    private void VehiclesSubMenu()
    {
        while (true)
        {
            _ui.ShowVehiclesSubMenu();
            var choice = _ui.ReadInput("Select menu option: ");

            // Handle submenu choices for vehicle listings
            switch (choice)
            {
                case "1":
                    GetVehicles();
                    return;

                case "2":
                    GetVehicleTypeCount();
                    return;

                default:
                    _ui.ShowMessage("Invalid choice, try again.");
                    break;
            }

        }
    }

    private void GetVehicles()
    {
        var vehicles = _handler.GetAllVehicles();

        // Display all vehicles or show empty message
        if (vehicles.Count() == 0)
        {
            _ui.ShowMessage("\nGarage is empty");
        }
        else
        {
            _ui.ShowMessage($"\nVehicles in garage ({vehicles.Count()}):");
            foreach (var v in vehicles)
            {
                _ui.ShowMessage(v.ToString());
            }

        }
    }

    private void GetVehicleTypeCount()
    {
        var counts = _handler.GetVehicleTypeCount();

        // Display vehicle type statistics
        if (counts.Count() == 0)
        {
            _ui.ShowMessage("\nGarage is empty");
        }
        else
        {
            _ui.ShowMessage("\nVehicles types in garage:");
            foreach (var kvp in counts)
            {
                _ui.ShowMessage($"{kvp.Key}: {kvp.Value}");
            }
        }
    }

    // Helper methods

    private FuelType AskFuelType()
    {
        while (true)
        {
            _ui.ShowMessage("\nChoose fuel type:");
            _ui.ShowMessage("1. Gasoline");
            _ui.ShowMessage("2. Diesel");
            _ui.ShowMessage("3. Electric");
            _ui.ShowMessage("4. Hybrid");
            _ui.ShowMessage("5. None");

            var input = _ui.ReadInput("Enter choice (1–5): ");

            var fuel = ParseFuelType(input);

            // Keep asking until valid fuel type is selected
            if (fuel != null) return (FuelType)fuel;
        }
    }

    private FuelType? ParseFuelType(string input)
    {
        // Return null for empty input to support optional search criteria
        if (string.IsNullOrWhiteSpace(input)) return null;

        // Map user input to FuelType enum
        return input switch
        {
            "1" => FuelType.Gasoline,
            "2" => FuelType.Diesel,
            "3" => FuelType.Electric,
            "4" => FuelType.Hybrid,
            "5" => FuelType.None,
            _ => null
        };
    }

    private string? GetVehicleType(string? choice)
    {
        // Convert numeric choice to vehicle type name
        string? vehicleType = choice switch
        {
            "1" => "Car",
            "2" => "Motorcycle",
            "3" => "Bus",
            "4" => "Boat",
            "5" => "Airplane",
            _ => null
        };

        return vehicleType;
    }

    private int? CheckWeelsInput(string wheelsInput)
    {
        // Parse wheels input and return null if empty or invalid
        int? wheels = null;
        if (!string.IsNullOrWhiteSpace(wheelsInput))
        {
            if (int.TryParse(wheelsInput, out int parsedWheels))
            {
                wheels = parsedWheels;
            }
        }

        return wheels;
    }
}
