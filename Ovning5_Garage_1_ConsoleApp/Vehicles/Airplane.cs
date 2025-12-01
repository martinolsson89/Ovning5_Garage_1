using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Airplane : Vehicle
{
    public int Engines { get; }
    public int Wingspan { get; }

    public override VehicleType VehicleType => VehicleType.Airplane;

    public Airplane(string registrationNumber, string color, int wheels, FuelType fueltype, int engines, int wingspan)
        : base(registrationNumber, color, 6, fueltype)
    {
        if (engines <= 0)
            throw new ArgumentOutOfRangeException(nameof(engines), "Number of engines must be greater than zero.");
        if (wingspan <= 0)
            throw new ArgumentOutOfRangeException(nameof(wingspan), "Wingspan must be greater than zero.");
        Engines = engines;
        Wingspan = wingspan;
    }

    public override string ToString()
    {
        return string.Format(
            "Vehicle: {0,-10} Reg: {1,-8} Color: {2,-8} Wheels: {3,-4} FuelType: {4,-10} Engines: {5,-6} Wingspan: {6,-5}",
            nameof(Airplane),
            RegistrationNumber,
            Color,
            Wheels,
            FuelType,
            Engines,
            $"{Wingspan} m"
        );
    }

    protected override bool MatchesSpecific(VehicleQueryDto query)
    {
        if (query.Engines is not null && Engines != query.Engines.Value)
            return false;

        if (query.Wingspan is not null && Wingspan != query.Wingspan.Value)
            return false;

        return true;
    }
}
