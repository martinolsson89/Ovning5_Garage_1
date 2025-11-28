using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public abstract class VehicleCreator
{
    public abstract VehicleType Type { get; }
    protected Random Random { get; }

    protected VehicleCreator(Random random)
    {
        Random = random ?? throw new ArgumentNullException(nameof(random));
    }

    // Validates input and delegates to concrete creation
    public Vehicle Create(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrWhiteSpace(registrationNumber))
        {
            throw new ArgumentNullException(nameof(registrationNumber));
        }

        return CreateVehicle(dto, registrationNumber, allowRandomFallbacks);
    }

    // Implemented by subclasses to build the specific vehicle
    protected abstract Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks);

    // Resolve color or generate a random one when allowed
    protected string ResolveColor(string? color, bool allowRandomFallbacks)
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

    // Resolve positive integer or use provided random generator
    protected int ResolveNumber(int? value, Func<int> randomValue, string paramName, bool allowRandomFallbacks)
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

    // Resolve boolean or use provided random generator
    protected bool ResolveBool(bool? value, Func<bool> randomValue, string paramName, bool allowRandomFallbacks)
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

    // Resolve enum or use provided random generator
    protected T ResolveEnum<T>(T? value, Func<T> randomValue, string paramName, bool allowRandomFallbacks) where T : struct, Enum
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

    // Resolve valid fuel type or default
    protected FuelType ResolveFuel(FuelType fuelType, Func<FuelType> defaultFuel, bool allowRandomFallbacks)
    {
        if (Enum.IsDefined(typeof(FuelType), fuelType) && fuelType != FuelType.None)
        {
            return fuelType;
        }

        if (!allowRandomFallbacks)
        {
            throw new ArgumentException("Fuel type must be provided", nameof(fuelType));
        }

        return defaultFuel();
    }

    // Pick a random color from a small palette
    private string GetRandomColor()
    {
        string[] colors = { "Red", "Blue", "Green", "Black", "White", "Yellow", "Pink" };
        return colors[Random.Next(colors.Length)];
    }
}
