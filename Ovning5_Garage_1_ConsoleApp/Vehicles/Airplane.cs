using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Airplane : Vehicle
{
    public int Engines { get; }
    public int Wingspan { get; }

    public Airplane(string registrationNumber, string color, int wheels, FuelType fueltype, int engines, int wingspan)
        : base(registrationNumber, color, wheels, fueltype)
    {
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
}
