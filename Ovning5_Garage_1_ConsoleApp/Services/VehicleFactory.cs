
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

    public Vehicle CreateFromDto(VehicleDto dto, bool allowRandomFallbacks = true)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var registrationNumber = _registrationNumberGenerator();
        var vehicleType = dto.VehicleType;
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, vehicleType, allowRandomFallbacks);

        return vehicleType switch
        {
            VehicleType.Car => new Car(
                registrationNumber,
                color,
                4,
                fuelType,
                ResolveNumber(dto.NumberOfDoors, () => _random.Next(2, 5), nameof(dto.NumberOfDoors), allowRandomFallbacks),
                ResolveEnum(dto.CarType, GetRandomCarType, nameof(dto.CarType), allowRandomFallbacks)),

            VehicleType.Motorcycle => new Motorcycle(
                registrationNumber,
                color,
                2,
                fuelType,
                ResolveEnum(dto.MotorcycleType, GetRandomMotorcycleType, nameof(dto.MotorcycleType), allowRandomFallbacks),
                ResolveNumber(dto.EngineDisplacement, () => _random.Next(50, 1300), nameof(dto.EngineDisplacement), allowRandomFallbacks)),

            VehicleType.Bus => new Bus(
                registrationNumber,
                color,
                4,
                fuelType,
                ResolveNumber(dto.NumberOfSeats, () => _random.Next(10, 60), nameof(dto.NumberOfSeats), allowRandomFallbacks),
                ResolveBool(dto.IsDoubleDecker, () => _random.Next(0, 2) == 1, nameof(dto.IsDoubleDecker), allowRandomFallbacks)),

            VehicleType.Boat => new Boat(
                registrationNumber,
                color,
                0,
                fuelType,
                ResolveEnum(dto.BoatType, GetRandomBoatType, nameof(dto.BoatType), allowRandomFallbacks),
                ResolveNumber(dto.Length, () => _random.Next(5, 40), nameof(dto.Length), allowRandomFallbacks)),

            VehicleType.Airplane => new Airplane(
                registrationNumber,
                color,
                6,
                fuelType,
                ResolveNumber(dto.Engines, () => _random.Next(1, 5), nameof(dto.Engines), allowRandomFallbacks),
                ResolveNumber(dto.Wingspan, () => _random.Next(10, 60), nameof(dto.Wingspan), allowRandomFallbacks)),

            _ => throw new ArgumentOutOfRangeException(nameof(dto.VehicleType))
        };
    }

    public Vehicle CreateRandomVehicle()
    {
        var vehicleType = GetRandomVehicleType();
        var dto = new VehicleDto
        {
            VehicleType = vehicleType,
            Color = string.Empty,
            FuelType = FuelType.None
        };

        return CreateFromDto(dto, allowRandomFallbacks: true);
    }

    private string ResolveColor(string? color, bool allowRandomFallbacks)
    {
        if (!string.IsNullOrWhiteSpace(color))
        {
            return color;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentException("Color must be provided", nameof(color));
        }

        return GetRandomColor();
    }

    private int ResolveWheels(int wheels, VehicleType vehicleType, bool allowRandomFallbacks)
    {
        if (wheels > 0)
        {
            return wheels;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentException("Wheels must be provided", nameof(wheels));
        }

        return GetDefaultWheels(vehicleType);
    }

    private FuelType ResolveFuel(FuelType fuelType, VehicleType vehicleType, bool allowRandomFallbacks)
    {
        if (Enum.IsDefined(typeof(FuelType), fuelType) && fuelType != FuelType.None)
        {
            return fuelType;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentException("Fuel type must be provided", nameof(fuelType));
        }

        return GetDefaultFuelType(vehicleType);
    }

    private int ResolveNumber(int? value, Func<int> randomValue, string paramName, bool allowRandomFallbacks)
    {
        if (value.HasValue && value.Value > 0)
        {
            return value.Value;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentNullException(paramName);
        }

        return randomValue();
    }

    private bool ResolveBool(bool? value, Func<bool> randomValue, string paramName, bool allowRandomFallbacks)
    {
        if (value.HasValue)
        {
            return value.Value;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentNullException(paramName);
        }

        return randomValue();
    }

    private T ResolveEnum<T>(T? value, Func<T> randomValue, string paramName, bool allowRandomFallbacks) where T : struct, Enum
    {
        if (value.HasValue)
        {
            return value.Value;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentNullException(paramName);
        }

        return randomValue();
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

