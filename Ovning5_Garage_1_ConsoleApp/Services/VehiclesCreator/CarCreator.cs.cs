

using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;

public class CarCreator : VehicleCreator
{
    public override VehicleType Type => VehicleType.Car;
    public CarCreator(Random random) : base(random)
    {
    }


    protected override Vehicle CreateVehicle(VehicleDto dto, string registrationNumber, bool allowRandomFallbacks)
    {
        var color = ResolveColor(dto.Color, allowRandomFallbacks);
        var fuelType = ResolveFuel(dto.FuelType, GetDefaultFuelType, allowRandomFallbacks);

        return new Car(
            registrationNumber,
            color,
            4,
            fuelType,
            ResolveNumber(dto.NumberOfDoors, () => Random.Next(2, 5), nameof(dto.NumberOfDoors), allowRandomFallbacks),
            ResolveEnum(dto.CarType, GetRandomCarType, nameof(dto.CarType), allowRandomFallbacks));
    }

    private FuelType GetDefaultFuelType() => GetRandomFuel();

    private CarType GetRandomCarType()
    {
        CarType[] carTypes = { CarType.Sedan, CarType.Van, CarType.SportsCar, CarType.Suv };
        return carTypes[Random.Next(carTypes.Length)];
    }

    private FuelType GetRandomFuel()
    {
        FuelType[] fuels = { FuelType.Gasoline, FuelType.Diesel, FuelType.Hybrid, FuelType.Electric, FuelType.None };
        return fuels[Random.Next(fuels.Length)];
    }
}
