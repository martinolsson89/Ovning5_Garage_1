using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using System.Xml;

namespace Ovning5_Garage_1_ConsoleApp.UI;

public class Manager
{
    private readonly IUI _ui;
    private readonly IHandler _handler;

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
                    FindVehicleByProp();
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
        if (!vehicles.Any())
        {
            _ui.ShowMessage("\nGarage is empty");
            return;
        }

        _ui.ShowMessage($"\nVehicles in garage ({vehicles.Count()}):");
        foreach (var v in vehicles)
        {
            _ui.ShowMessage(v.ToString());
        }
    }

    private void GetVehicleTypeCount()
    {
        var counts = _handler.GetVehicleTypeCount();

        // Display vehicle type statistics
        if (!counts.Any())
        {
            _ui.ShowMessage("\nGarage is empty");
        }

        _ui.ShowMessage("\nVehicles types in garage:");
        foreach (var kvp in counts)
        {
            _ui.ShowMessage($"{kvp.Key}: {kvp.Value}");
        }
    }

    private void AddVehicle()
    {
        try
        {
            // Collect vehicle information from user
            _ui.ShowAddVehicleSubMenu();
            var dto = new VehicleDto();

            dto.VehicleType = (VehicleType)_ui.ReadIntInRange("Select menu option: ", 1, 5);
            dto.Color = _ui.ReadUserInput("Enter color: ");
            dto.Wheels = _ui.ReadInt("Enter number of wheels: ");
            dto.FuelType = AskFuelType();
            CollectTypeDetails(dto);

            // Attempt to park the vehicle
            var (result, parkedVehicle) = _handler.ParkVehicle(dto);

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
        }
        catch (ArgumentNullException ex)
        {
            _ui.ShowMessage($"\nError: Required information is missing. {ex.Message}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            var message = ex.ParamName switch
            {
                "wheels" => "\nInvalid input: Number of wheels cannot be negative.",
                "numberOfDoors" => "\nInvalid input: Number of doors must be greater than zero.",
                "numberOfSeats" => "\nInvalid input: Number of seats must be greater than zero.",
                "engineDisplacement" => "\nInvalid input: Engine displacement must be greater than zero.",
                "length" => "\nInvalid input: Length must be greater than zero.",
                "engines" => "\nInvalid input: Number of engines must be greater than zero.",
                "wingspan" => "\nInvalid input: Wingspan must be greater than zero.",
                _ => $"\nInvalid input: {ex.Message}"
            };

            _ui.ShowMessage(message);
        }
        catch (Exception ex)
        {
            _ui.ShowMessage($"\nError adding vehicle: {ex.Message}");
        }
    }

    private void RemoveVehicle()
    {
        try
        {
            var regNr = _ui.ReadUserInput("\nEnter Reg Number to remove vehicle: ");
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
        catch (Exception ex)
        {
            _ui.ShowMessage($"\nError removing vehicle: {ex.Message}");
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

    private void FindVehicleByProp()
    {
        _ui.ShowMessage("\n=== Search Vehicles by Properties ===");
        _ui.ShowMessage("Enter search criteria (leave blank to skip):");

        // Collect search criteria from user
        _ui.ShowAddVehicleSubMenu();
        var vehicleTypeInput = _ui.ReadInput("Enter choice (1–5):");
        var color = _ui.ReadInput("Color: ");
        var wheelsInput = _ui.ReadInput("Number of wheels: ");
        _ui.ShowFuelTypeMenu();
        var fuelTypeInput = _ui.ReadInput("Enter choice (1-5):");

        // Parse user inputs into proper types
        VehicleType? vehicleType = GetVehicleType(vehicleTypeInput);
        int? wheels = ParseWheelsInput(wheelsInput);
        FuelType? fuelType = ParseFuelType(fuelTypeInput);

        var query = new VehicleQueryDto
        {
            VehicleType = vehicleType,
            Color = string.IsNullOrWhiteSpace(color) ? null : color,
            Wheels = wheels,
            FuelType = fuelType
        };
        // Optional type-specific filters if a type was chosen
        if (vehicleType is not null)
        {
            CollectTypeQueryFilters(query, vehicleType);
        }

        // Search garage with the provided criteria
        //var results = _handler.GetVehicles(vehicleType, color, wheels, fuelType);
        var results = _handler.GetVehicles(query);

        // Display search results
        if (!results.Any())
        {
            _ui.ShowMessage("\nNo vehicles found matching the criteria.");
            return;
        }

        _ui.ShowMessage($"\nFound {results.Count()} vehicle(s):");
        foreach (var vehicle in results)
        {
            _ui.ShowMessage(vehicle.ToString());
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
                    _ui.ShowMessage("\nNo vehicles were seeded.");
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

    // Collect optional sub-class filters (blank to skip each)
    private void CollectTypeQueryFilters(VehicleQueryDto query, VehicleType? vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleType.Car:
                var doorsInput = _ui.ReadInput("Number of doors (blank = any): ");
                query.NumberOfDoors = ParseNullableInt(doorsInput);
                var carType = AskOptionalCarType();
                query.CarType = carType;
                break;

            case VehicleType.Motorcycle:
                var displacementInput = _ui.ReadInput("Engine displacement in cc (blank = any): ");
                query.EngineDisplacement = ParseNullableInt(displacementInput);
                var mcType = AskOptionalMotorcycleType();
                query.MotorcycleType = mcType;
                break;

            case VehicleType.Bus:
                var seatsInput = _ui.ReadInput("Number of seats (blank = any): ");
                query.NumberOfSeats = ParseNullableInt(seatsInput);
                var ddInput = _ui.ReadInput("Is double-decker? (y/n, blank = any): ");
                query.IsDoubleDecker = ParseNullableBool(ddInput);
                break;

            case VehicleType.Boat:
                var lengthInput = _ui.ReadInput("Length in meters (blank = any): ");
                query.Length = ParseNullableInt(lengthInput);
                var boatType = AskOptionalBoatType();
                query.BoatType = boatType;
                break;

            case VehicleType.Airplane:
                var enginesInput = _ui.ReadInput("Number of engines (blank = any): ");
                query.Engines = ParseNullableInt(enginesInput);
                var wingspanInput = _ui.ReadInput("Wingspan in meters (blank = any): ");
                query.Wingspan = ParseNullableInt(wingspanInput);
                break;
        }
    }


    // Helper methods
    private void CollectTypeDetails(VehicleDto dto)
    {
        switch (dto.VehicleType)
        {
            case VehicleType.Car: // Car
                var numberOfDoors = _ui.ReadInt("Enter number of doors: ");
                var carType = AskCarType();
                dto.NumberOfDoors = numberOfDoors;
                dto.CarType = carType;
                break;

            case VehicleType.Motorcycle: // Motorcycle
                var engineDisplacement = _ui.ReadInt("Enter engine displacement (cc): ");
                var motorcycleType = AskMotorcycleType();
                dto.EngineDisplacement = engineDisplacement;
                dto.MotorcycleType = motorcycleType;
                break;

            case VehicleType.Bus: // Bus
                var numberOfSeats = _ui.ReadInt("Enter number of seats: ");
                var isDoubleDecker = _ui.ReadYesNo("Is double-decker? (y/n): ");
                dto.NumberOfSeats = numberOfSeats;
                dto.IsDoubleDecker = isDoubleDecker;
                break;

            case VehicleType.Boat: // Boat
                var length = _ui.ReadInt("Enter length (m): ");
                var boatType = AskBoatType();
                dto.Length = length;
                dto.BoatType = boatType;
                break;

            case VehicleType.Airplane: // Airplane
                var engines = _ui.ReadInt("Enter number of engines: ");
                var wingspan = _ui.ReadInt("Enter wingspan (m): ");
                dto.Engines = engines;
                dto.Wingspan = wingspan;
                break;
        }
    }

    private FuelType AskFuelType()
    {
        while (true)
        {
            _ui.ShowFuelTypeMenu();

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

    private VehicleType? GetVehicleType(string? choice)
    {
        // Convert numeric choice to vehicle type name
        VehicleType? vehicleType = choice switch
        {
            "1" => VehicleType.Car,
            "2" => VehicleType.Motorcycle,
            "3" => VehicleType.Bus,
            "4" => VehicleType.Boat,
            "5" => VehicleType.Airplane,
            _ => null
        };

        return vehicleType;
    }

    private int? ParseWheelsInput(string wheelsInput)
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

    private CarType AskCarType()
    {
        _ui.ShowMessage("\nSelect Car Type:");
        _ui.ShowMessage("1. Sedan");
        _ui.ShowMessage("2. SUV");
        _ui.ShowMessage("3. Van");
        _ui.ShowMessage("4. Sports car");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4): ");
            var type = input switch
            {
                "1" => CarType.Sedan,
                "2" => CarType.Suv,
                "3" => CarType.Van,
                "4" => CarType.SportsCar,
                _ => (CarType?)null
            };
            if (type != null) return (CarType)type;
        }
    }

    private MotorcycleType AskMotorcycleType()
    {
        _ui.ShowMessage("\nSelect Motorcycle Type:");
        _ui.ShowMessage("1. Cruiser");
        _ui.ShowMessage("2. Sport");
        _ui.ShowMessage("3. Motocross");
        _ui.ShowMessage("4. Chopper");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4): ");
            var type = input switch
            {
                "1" => MotorcycleType.Cruiser,
                "2" => MotorcycleType.Sport,
                "3" => MotorcycleType.Motocross,
                "4" => MotorcycleType.Chopper,
                _ => (MotorcycleType?)null
            };
            if (type != null) return (MotorcycleType)type;
        }
    }

    private BoatType AskBoatType()
    {
        _ui.ShowMessage("\nSelect Boat Type:");
        _ui.ShowMessage("1. Sailboat");
        _ui.ShowMessage("2. Motorboat");
        _ui.ShowMessage("3. Yacht");
        _ui.ShowMessage("4. Fishing boat");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4): ");
            var type = input switch
            {
                "1" => BoatType.Sailboat,
                "2" => BoatType.Motorboat,
                "3" => BoatType.Yacht,
                "4" => BoatType.FishingBoat,
                _ => (BoatType?)null
            };
            if (type != null) return (BoatType)type;
        }
    }

    // Query

    private static int? ParseNullableInt(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        return int.TryParse(input, out var value) ? value : null;
    }

    private static bool? ParseNullableBool(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        var s = input.Trim().ToLowerInvariant();
        return s switch
        {
            "y" or "yes" => true,
            "n" or "no" => false,
            _ => null
        };
    }

    private CarType? AskOptionalCarType()
    {
        _ui.ShowMessage("\nSelect Car Type (blank = any):");
        _ui.ShowMessage("1. Sedan");
        _ui.ShowMessage("2. SUV");
        _ui.ShowMessage("3. Van");
        _ui.ShowMessage("4. Sports car");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4) or blank: ");
            if (string.IsNullOrWhiteSpace(input)) return null;
            var type = input switch
            {
                "1" => CarType.Sedan,
                "2" => CarType.Suv,
                "3" => CarType.Van,
                "4" => CarType.SportsCar,
                _ => (CarType?)null
            };
            if (type != null) return (CarType)type;
        }
    }

    private MotorcycleType? AskOptionalMotorcycleType()
    {
        _ui.ShowMessage("\nSelect Motorcycle Type (blank = any):");
        _ui.ShowMessage("1. Cruiser");
        _ui.ShowMessage("2. Sport");
        _ui.ShowMessage("3. Motocross");
        _ui.ShowMessage("4. Chopper");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4) or blank: ");
            if (string.IsNullOrWhiteSpace(input)) return null;
            var type = input switch
            {
                "1" => MotorcycleType.Cruiser,
                "2" => MotorcycleType.Sport,
                "3" => MotorcycleType.Motocross,
                "4" => MotorcycleType.Chopper,
                _ => (MotorcycleType?)null
            };
            if (type != null) return (MotorcycleType)type;
        }
    }

    private BoatType? AskOptionalBoatType()
    {
        _ui.ShowMessage("\nSelect Boat Type (blank = any):");
        _ui.ShowMessage("1. Sailboat");
        _ui.ShowMessage("2. Motorboat");
        _ui.ShowMessage("3. Yacht");
        _ui.ShowMessage("4. Fishing boat");
        while (true)
        {
            var input = _ui.ReadInput("Enter choice (1–4) or blank: ");
            if (string.IsNullOrWhiteSpace(input)) return null;
            var type = input switch
            {
                "1" => BoatType.Sailboat,
                "2" => BoatType.Motorboat,
                "3" => BoatType.Yacht,
                "4" => BoatType.FishingBoat,
                _ => (BoatType?)null
            };
            if (type != null) return (BoatType)type;
        }
    }
}
