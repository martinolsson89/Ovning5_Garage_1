
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public class MotorcycleCreator : VehicleCreator
{
    public override VehicleType Type => VehicleType.Motorcycle;
    public MotorcycleCreator(Random random) : base(random)
    {
    }


    protected override Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, GetDefaultFuelType, allowRandomFallbacks);

        return new Motorcycle(
            registrationNumber,
            color,
            2,
            fuelType,
            ResolveEnum(dto.MotorcycleType, GetRandomMotorcycleType, nameof(dto.MotorcycleType), allowRandomFallbacks),
            ResolveNumber(dto.EngineDisplacement, () => Random.Next(50, 1300), nameof(dto.EngineDisplacement), allowRandomFallbacks));
    }

    private FuelType GetDefaultFuelType() => GetRandomFuel();

    private MotorcycleType GetRandomMotorcycleType()
    {
        MotorcycleType[] motorcycleTypes = { MotorcycleType.Motocross, MotorcycleType.Cruiser, MotorcycleType.Chopper, MotorcycleType.Sport };
        return motorcycleTypes[Random.Next(motorcycleTypes.Length)];
    }

    private FuelType GetRandomFuel()
    {
        FuelType[] fuels = { FuelType.Gasoline, FuelType.Diesel, FuelType.Hybrid, FuelType.Electric, FuelType.None };
        return fuels[Random.Next(fuels.Length)];
    }
}
