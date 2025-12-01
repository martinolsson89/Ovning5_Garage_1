
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public class AirplaneCreator : VehicleCreator
{
    public override VehicleType Type => VehicleType.Airplane;

    public AirplaneCreator(Random random) : base(random)
    {
    }


    protected override Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, () => FuelType.Gasoline, allowRandomFallbacks);

        return new Airplane(
            registrationNumber,
            color,
            6,
            fuelType,
            ResolveNumber(dto.Engines, () => Random.Next(1, 5), nameof(dto.Engines), allowRandomFallbacks),
            ResolveNumber(dto.Wingspan, () => Random.Next(10, 60), nameof(dto.Wingspan), allowRandomFallbacks));
    }
}
