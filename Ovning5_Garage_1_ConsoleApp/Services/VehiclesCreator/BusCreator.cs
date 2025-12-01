
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public class BusCreator : VehicleCreator
{
    public override VehicleType Type => VehicleType.Bus;
    public BusCreator(Random random) : base(random)
    {
    }


    protected override Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, () => FuelType.Diesel, allowRandomFallbacks);

        return new Bus(
            registrationNumber,
            color,
            4,
            fuelType,
            ResolveNumber(dto.NumberOfSeats, () => Random.Next(10, 60), nameof(dto.NumberOfSeats), allowRandomFallbacks),
            ResolveBool(dto.IsDoubleDecker, () => Random.Next(0, 2) == 1, nameof(dto.IsDoubleDecker), allowRandomFallbacks));
    }
}
