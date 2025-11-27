
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services;

public class VehicleFactory
{
    private readonly Func<string> _registrationNumberGenerator;
    private Random _random;

    public VehicleFactory(Func<string> registrationNumberGenerator, Random? random = null)
    {
        _registrationNumberGenerator = registrationNumberGenerator ??
            throw new ArgumentNullException(nameof(registrationNumberGenerator));
        _random = random ?? new Random();
    }

    public Vehicle CreateRandomVehicle()
    {
        var vehicleType = GetRandomVehicleType();
        return CreateVehicle(vehicleType);
    }

    public bool TryCreateVehicle(VehicleType vehicleType, VehicleFactoryOptions? options, out Vehicle? vehicle)
    {
        if (!Enum.IsDefined(typeof(VehicleType), vehicleType))
        {
            vehicle = null;
            return false;
        }

        vehicle = CreateVehicleInternal(vehicleType, options ?? new VehicleFactoryOptions());
        return true;
    }

    public Vehicle CreateVehicle(VehicleType vehicleType, VehicleFactoryOptions? options = null)
    {
        if (!TryCreateVehicle(vehicleType, options, out var vehicle) || vehicle is null)
        {
            throw new ArgumentOutOfRangeException(nameof(vehicleType));
        }

        return vehicle;
    }

    private Vehicle CreateVehicleInternal(VehicleType vehicleType, VehicleFactoryOptions options)
    {
        var registrationNumber = _registrationNumberGenerator();
        var color = options.Color ?? GetRandomColor();
        var wheels = options.Wheels ?? GetDefaultWheels(vehicleType);
        var fuelType = options.FuelType ?? GetDefaultFuelType(vehicleType);

        return vehicleType switch
        {
            VehicleType.Car => new Car(registrationNumber, color, wheels, fuelType, _random.Next(2, 5), GetRandomCarType()),
            VehicleType.Motorcycle => new Motorcycle(registrationNumber, color, wheels, fuelType, GetRandomMotorcycleType(), _random.Next(50, 1300)),
            VehicleType.Bus => new Bus(registrationNumber, color, wheels, fuelType, _random.Next(10, 60), _random.Next(0, 2) == 1),
            VehicleType.Boat => new Boat(registrationNumber, color, wheels, fuelType, GetRandomBoatType(), _random.Next(5, 40)),
            VehicleType.Airplane => new Airplane(registrationNumber, color, wheels, fuelType, _random.Next(1, 5), _random.Next(10, 60)),
            _ => throw new ArgumentOutOfRangeException(nameof(vehicleType))
        };
    }

    private VehicleType GetRandomVehicleType()
    {
        var values = Enum.GetValues<VehicleType>();
        return values[_random.Next(values.Length)];
    }

    private int GetDefaultWheels(VehicleType vehicleType) => vehicleType switch
    {
        VehicleType.Car => 4,
        VehicleType.Motorcycle => 2,
        VehicleType.Bus => 4,
        VehicleType.Boat => 0,
        VehicleType.Airplane => _random.Next(2, 9),
        _ => throw new ArgumentOutOfRangeException(nameof(vehicleType))
    };

    private FuelType GetDefaultFuelType(VehicleType vehicleType) => vehicleType switch
    {
        VehicleType.Car => GetRandomFuel(),
        VehicleType.Motorcycle => GetRandomFuel(),
        VehicleType.Bus => FuelType.Diesel,
        VehicleType.Boat => FuelType.None,
        VehicleType.Airplane => FuelType.Gasoline,
        _ => throw new ArgumentOutOfRangeException(nameof(vehicleType))
    };

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

public class VehicleFactoryOptions
{
    public string? Color { get; init; }
    public int? Wheels { get; init; }
    public FuelType? FuelType { get; init; }
}
