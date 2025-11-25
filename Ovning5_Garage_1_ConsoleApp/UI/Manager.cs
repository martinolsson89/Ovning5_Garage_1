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

                case "0":
                    running = false;
                    break;

                default:
                    _ui.ShowMessage("Invalid choice, try again.");
                    break;
            }
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
