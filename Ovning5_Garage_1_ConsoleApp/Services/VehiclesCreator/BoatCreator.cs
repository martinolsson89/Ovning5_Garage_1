
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System.Drawing;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public class BoatCreator : VehicleCreator
{
    public override VehicleType Type => VehicleType.Boat;
    public BoatCreator(Random random) : base(random)
    {
    }


    protected override Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, () => FuelType.None, allowRandomFallbacks);

        return new Boat(
            registrationNumber,
            color,
            0,
            fuelType,
            ResolveEnum(dto.BoatType, GetRandomBoatType, nameof(dto.BoatType), allowRandomFallbacks),
            ResolveNumber(dto.Length, () => Random.Next(5, 40), nameof(dto.Length), allowRandomFallbacks));
    }

    private BoatType GetRandomBoatType()
    {
        BoatType[] boatTypes = { BoatType.Sailboat, BoatType.Motorboat, BoatType.Yacht, BoatType.FishingBoat };
        return boatTypes[Random.Next(boatTypes.Length)];
    }
}
