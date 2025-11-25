using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

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
        //_handler.SeedGarage();

        bool running = true;
        while (running)
        {
            _ui.ShowMenu();
            var choice = _ui.ReadInput("Select menu option: ");

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

        var color = _ui.ReadInput("Color: ");
        var wheelsInput = _ui.ReadInput("Number of wheels: ");
        var fuelTypeInput = _ui.ReadInput("Fuel type (1=Gasoline, 2=Diesel, 3=Electric, 4=Hybrid, 5=None): ");
        var vehicleType = _ui.ReadInput("Vehicle type (Car/Motorcycle/Bus/Boat/Airplane): ");

        // Parse input
        int? wheels = string.IsNullOrWhiteSpace(wheelsInput) ? null : int.Parse(wheelsInput);
        FuelType? fuelType = ParseFuelType(fuelTypeInput);

        // Build search predicate
        var results = _handler.GetVehicles(v =>
        {
            bool matches = true;

            if (!string.IsNullOrWhiteSpace(color))
            {
                matches &= v.Color.Equals(color, StringComparison.OrdinalIgnoreCase);
            }

            if (wheels.HasValue)
            {
                matches &= v.Wheels == wheels.Value;
            }

            if (fuelType.HasValue)
            {
                matches &= v.FuelType == fuelType.Value;
            }

            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                matches &= v.GetType().Name.Equals(vehicleType, StringComparison.OrdinalIgnoreCase);
            }

            return matches;
        });

        //var vehicleList = results.ToList();

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

    private FuelType? ParseFuelType(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

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

    private void FindVehicleByRegNr()
    {
        var reg = _ui.ReadInput("Enter registration number: ");
        var res = _handler.GetVehicleByRegNr(reg);

        if (res is null)
        {
            _ui.ShowMessage("Vehicle was not found.");
        }
        else
        {
            _ui.ShowMessage($"{res}");
        }
    }

    private void RemoveVehicle()
    {
        var regNr = _ui.ReadInput("\nEnter Reg Number to remove vehicle: ");
        var result = _handler.RemoveVehicle(regNr);

        if (result)
        {
            _ui.ShowMessage($"Vehicle with {regNr} was successfully removed from garage!");
        }
        else
        {
            _ui.ShowMessage($"Vehicle with {regNr} was not found in the garage");
        }
    }

    private void AddVehicle()
    {
        while (true)
        {
            _ui.ShowAddVehicleSubMenu();
            var typeChoice = _ui.ReadInput("Select menu option: ");
            var color = _ui.ReadInput("Enter color: ");
            var wheels = _ui.ReadInt("Enter number of wheels: ");
            var fuelType = AskFuelType();

            Vehicle? vehicle = typeChoice switch
            {
                "1" => new Car(color, wheels, fuelType, 4, CarType.Suv),
                "2" => new Motorcycle(color, wheels, fuelType, MotorcycleType.Chopper, 150),
                "3" => new Bus(color, wheels, fuelType, 22, false),
                "4" => new Boat(color, wheels, fuelType, BoatType.FishingBoat, 14),
                "5" => new Airplane(color, wheels, fuelType, 2, 16),
                _ => null
            };

            if(vehicle is null)
            {
                _ui.ShowMessage("Invalid vehicle type, cancelling.");
                return;
            }

           var (result, parkedVehicle) = _handler.ParkVehicle(vehicle);

            switch (result)
            {
                case ParkResult.Success:
                    _ui.ShowMessage(
                        $"Vehicle parked successfully. Reg: {parkedVehicle!.RegistrationNumber}");
                    break;

                case ParkResult.AlreadyInGarage:
                    _ui.ShowMessage("That vehicle is already parked in the garage.");
                    break;

                case ParkResult.GarageIsFull:
                    _ui.ShowMessage("Garage is full. Could not park vehicle.");
                    break;
            }
            return;
        }
    }

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

            var fuel = input switch
            {
                "1" => FuelType.Gasoline,
                "2" => FuelType.Diesel,
                "3" => FuelType.Electric,
                "4" => FuelType.Hybrid,
                "5" => FuelType.None,
                _ => default
            };

            if (fuel != default) return fuel;
        }
    }

    private void SeedVehicles()
    {
        var IsSeedSuccessful = _handler.SeedGarage();
        if (!IsSeedSuccessful)
        {
            _ui.ShowMessage("\nSeed was not successful, garage maybe full");
        }
        else
        {
            _ui.ShowMessage("\nSeed was successfull, garage is now populated with vehicles");
        }
    }

    private void VehiclesSubMenu()
    {
        while (true)
        {
            _ui.ShowVehiclesSubMenu();
            var choice = _ui.ReadInput("Select menu option: ");

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

        if(vehicles.Count() == 0)
        {
            _ui.ShowMessage("\nGarage is empty");
        }
        else
        {
            _ui.ShowMessage("\nVehicles in garage:");
            foreach (var v in vehicles)
            {
                _ui.ShowMessage(v.ToString());
            }
        }
    }

    private void GetVehicleTypeCount()
    {
        var counts = _handler.GetVehicleTypeCount();

        if(counts.Count() == 0)
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
}
