using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System;



namespace Ovning5_Garage_1_ConsoleApp.Services;

public class Handler : IHandler
{
    private readonly IGarage<Vehicle> _garage;
    private readonly Random _random = new();
    
    public Handler(int capacity)
    {
        _garage = new Garage<Vehicle>(capacity);
    }

    public int GetAvailableSpots()
    {
        return _garage.Capacity - _garage.Count;
    }

    public IEnumerable<Vehicle> GetAllVehicles() => _garage;
    
    public IEnumerable<Vehicle> GetVehicles(string? vehicleType, string? color, int? wheels, FuelType? fuelType)
    {
        // Build dynamic search predicate based on provided criteria
        var results = _garage.GetVehicles(v =>
        {
            bool matches = true;

            // Check color if provided
            if (!string.IsNullOrWhiteSpace(color))
            {
                matches &= v.Color.Equals(color, StringComparison.OrdinalIgnoreCase);
            }

            // Check wheels if provided
            if (wheels.HasValue)
            {
                matches &= v.Wheels == wheels.Value;
            }

            // Check fuel type if provided
            if (fuelType.HasValue)
            {
                matches &= v.FuelType == fuelType.Value;
            }

            // Check vehicle type if provided
            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                matches &= v.GetType().Name.Equals(vehicleType, StringComparison.OrdinalIgnoreCase);
            }

            return matches;
        });

        return results;

    }

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _garage.GetVehicleByRegNr(regNr);
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _garage.GetVehicleTypeCount();
    }

    public (ParkResult, Vehicle? parkedVehicle) ParkVehicle(int vehicleType, string color, int wheels, FuelType fuelType)
    {
        // Create the appropriate vehicle type based on user choice
        Vehicle? vehicle = CreateVehicleType(vehicleType, color, wheels, fuelType);

        if (vehicle is null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        return _garage.Park(vehicle);
    }

    public bool RemoveVehicle(string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
        {
            throw new ArgumentNullException(nameof(registrationNumber));
        }

        return _garage.Remove(registrationNumber);
    }

    public (bool Success, int SeededCount, int RequestedCount, int AvailableSpots) SeedGarage(int requestedCount)
    {
        var availableSpots = _garage.Capacity - _garage.Count;

        // Check if seeding is possible
        if (requestedCount <= 0 || availableSpots <= 0)
        {
            return (false, 0, requestedCount, availableSpots);
        }

        // Limit the number of vehicles to the available space
        var toCreate = Math.Min(requestedCount, availableSpots);

        var vehiclesToSeed = GenerateVehicles(toCreate);
        var seeded = 0;

        // Attempt to park each generated vehicle
        foreach (var v in vehiclesToSeed)
        {
            (ParkResult parked, Vehicle? pk) = _garage.Park(v);
            if(parked == ParkResult.Success)
            {
                seeded++;
            }
        }

        return (seeded > 0, seeded, requestedCount, availableSpots);
    }

    // Helper methods

    private IEnumerable<Vehicle> GenerateVehicles(int count)
    {
        // Lazy generate vehicles one at a time
        for (int i = 0; i < count; i++)
        {
            yield return GenerateRandomVehicle();
        }
    }
    
    private Vehicle GenerateRandomVehicle()
    {
        // Randomly select vehicle type (0-4)
        var vehicleType = _random.Next(0, 5);

        switch (vehicleType)
        {
            case 0:
                return new Car(
                    registrationNumber: _garage.GenerateRegistrationNumber(),
                    color: GetRandomColor(),
                    wheels: 4,
                    fueltype: GetRandomFuel(),
                    numberOfDoors: _random.Next(2, 5),
                    type: GetRandomCarType());

            case 1:
                return new Motorcycle(
                    registrationNumber: _garage.GenerateRegistrationNumber(),
                    color: GetRandomColor(),
                    wheels: 2,
                    fueltype: GetRandomFuel(),
                    type: GetRandomMotorcycleType(),
                    engineDisplacement: _random.Next(50, 1300));

            case 2:
                return new Bus(
                    registrationNumber: _garage.GenerateRegistrationNumber(),
                    color: GetRandomColor(),
                    wheels: 4,
                    fueltype: FuelType.Diesel,
                    numberOfSeats: _random.Next(10, 60),
                    isDoubleDecker: _random.Next(0, 2) == 1);

            case 3:
                return new Boat(
                    registrationNumber: _garage.GenerateRegistrationNumber(),
                    color: GetRandomColor(),
                    wheels: 0,
                    fueltype: FuelType.None,
                    type: GetRandomBoatType(),
                    length: _random.Next(5, 40));

            case 4:
            default:
                return new Airplane(
                    registrationNumber: _garage.GenerateRegistrationNumber(),
                    color: GetRandomColor(),
                    wheels: _random.Next(2, 9),
                    fueltype: FuelType.Gasoline,
                    engines: _random.Next(1, 5),
                    wingspan: _random.Next(10, 60));
        }
    }
    
    private Vehicle? CreateVehicleType(int typeVehicle, string color, int wheels, FuelType fuelType)
    {
        // Create vehicle based on type selection with user-provided properties
        Vehicle? vehicle = typeVehicle switch
        {
            1 => new Car(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, numberOfDoors: _random.Next(2, 5), GetRandomCarType()),
            2 => new Motorcycle(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, GetRandomMotorcycleType(), _random.Next(50, 1300)),
            3 => new Bus(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, _random.Next(10, 60), _random.Next(0, 2) == 1),
            4 => new Boat(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, GetRandomBoatType(), _random.Next(5, 40)),
            5 => new Airplane(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, _random.Next(1, 5), _random.Next(10, 60)),
            _ => null
        };

        return vehicle;
    }

    private string GetRandomColor()
    {
        string[] colors = { "Red", "Blue", "Green", "Black", "White", "Yellow", "Pink" };
        return colors[_random.Next(colors.Length)];
    }

    private FuelType GetRandomFuel()
    {
        FuelType[] fuels = { FuelType.Gasoline, FuelType.Diesel, FuelType.Hybrid, FuelType.Electric, FuelType.None };
        return fuels[_random.Next(fuels.Length)];
    }
    
    private CarType GetRandomCarType()
    {
        CarType[] carTypes = { CarType.Sedan, CarType.Van, CarType.SportsCar, CarType.Suv };
        return carTypes[_random.Next(carTypes.Length)];
    }
    
    private MotorcycleType GetRandomMotorcycleType()
    {
        MotorcycleType[] motorcycleTypes = { MotorcycleType.Motocross, MotorcycleType.Cruiser, MotorcycleType.Chopper, MotorcycleType.Sport };
        return motorcycleTypes[_random.Next(motorcycleTypes.Length)];
    }
    
    private BoatType GetRandomBoatType()
    {
        BoatType[] boatTypes = { BoatType.Sailboat, BoatType.Motorboat, BoatType.Yacht, BoatType.FishingBoat };
        return boatTypes[_random.Next(boatTypes.Length)];
    }
}
