using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System;



namespace Ovning5_Garage_1_ConsoleApp.Services;

public class Handler : IHandler
{
    private readonly IGarage<Vehicle> _garage;
    private readonly Random _random = new();
    private VehicleFactory _vehicleFactory;

    public Handler(int capacity)
    {
        _garage = new Garage<Vehicle>(capacity);
        _vehicleFactory = new VehicleFactory(_garage.GenerateRegistrationNumber, _random);
    }

    public int GetAvailableSpots()
    {
        return _garage.Capacity - _garage.Count;
    }

    public IEnumerable<Vehicle> GetAllVehicles() => _garage;

    public IEnumerable<Vehicle> GetVehicles(VehicleType? vehicleType, string? color, int? wheels, FuelType? fuelType)
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
            if (vehicleType is not null)
            {
                matches &= v.GetType().Name.Equals(vehicleType.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return matches;
        });

        return results;

    }

    public IEnumerable<Vehicle> GetVehicles(VehicleQueryDto query)
    {
        if(query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var result = GetVehicles(query.VehicleType, query.Color, query.Wheels, query.FuelType);

        // Apply type-specific filters only when provided
        if (query.CarType is not null || query.NumberOfDoors is not null)
        {
            result = result.Where(v =>
                v is Car car &&
                (query.CarType is null || car.Type == query.CarType) &&
                (query.NumberOfDoors is null || car.NumberOfDoors == query.NumberOfDoors));
        }

        if (query.MotorcycleType is not null || query.EngineDisplacement is not null)
        {
            result = result.Where(v =>
                v is Motorcycle mc &&
                (query.MotorcycleType is null || mc.Type == query.MotorcycleType) &&
                (query.EngineDisplacement is null || mc.EngineDisplacement == query.EngineDisplacement));
        }

        if (query.NumberOfSeats is not null || query.IsDoubleDecker is not null)
        {
            result = result.Where(v =>
                v is Bus bus &&
                (query.NumberOfSeats is null || bus.NumberOfSeats == query.NumberOfSeats) &&
                (query.IsDoubleDecker is null || bus.IsDoubleDecker == query.IsDoubleDecker));
        }

        if (query.BoatType is not null || query.Length is not null)
        {
            result = result.Where(v =>
                v is Boat boat &&
                (query.BoatType is null || boat.Type == query.BoatType) &&
                (query.Length is null || boat.Length == query.Length));
        }

        if (query.Engines is not null || query.Wingspan is not null)
        {
            result = result.Where(v =>
                v is Airplane ap &&
                (query.Engines is null || ap.Engines == query.Engines) &&
                (query.Wingspan is null || ap.Wingspan == query.Wingspan));
        }

        return result;
    }

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _garage.GetVehicleByRegNr(regNr);
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _garage.GetVehicleTypeCount();
    }

    public (ParkResult, Vehicle? parkedVehicle) ParkVehicle(VehicleDto dto)
    {
        if(dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        // Create the appropriate vehicle type based on user choice
        Vehicle? vehicle = CreateVehicleType(dto);

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
            if (parked == ParkResult.Success)
            {
                seeded++;
            }
        }

        return (seeded > 0, seeded, requestedCount, availableSpots);
    }

    #region Helper methods

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

    private Vehicle? CreateVehicleType(VehicleDto dto)
    {
        // Create vehicle based on type selection with user-provided dto
        Vehicle? vehicle = dto.VehicleType switch
        {
            VehicleType.Car => new Car(_garage.GenerateRegistrationNumber(), dto.Color, dto.Wheels, dto.FuelType, dto?.NumberOfDoors ?? _random.Next(2, 5), dto?.CarType ?? GetRandomCarType()),
            VehicleType.Motorcycle => new Motorcycle(_garage.GenerateRegistrationNumber(), dto.Color, dto.Wheels, dto.FuelType, dto?.MotorcycleType ?? GetRandomMotorcycleType(), dto?.EngineDisplacement ?? _random.Next(50, 1300)),
            VehicleType.Bus => new Bus(_garage.GenerateRegistrationNumber(), dto.Color, dto.Wheels, dto.FuelType, dto?.NumberOfSeats ?? _random.Next(10, 60), dto?.IsDoubleDecker ?? _random.Next(0, 2) == 1),
            VehicleType.Boat => new Boat(_garage.GenerateRegistrationNumber(), dto.Color, dto.Wheels, dto.FuelType, dto?.BoatType ?? GetRandomBoatType(), dto?.Length ?? _random.Next(5, 40)),
            VehicleType.Airplane => new Airplane(_garage.GenerateRegistrationNumber(), dto.Color, dto.Wheels, dto.FuelType, dto?.Engines ?? _random.Next(1, 5), dto?.Wingspan ?? _random.Next(10, 60)),
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

    #endregion
}
