using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Boat : Vehicle
{
    public BoatType Type { get; }
    public int Lenght { get; }

    public Boat(string registrationNumber, string color, int wheels, FuelType fueltype, BoatType type, int length)
        : base(registrationNumber, color, wheels, fueltype)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");
        Type = type;
        Lenght = length;
    }
    public override string ToString()
    {
        return string.Format(
            "Vehicle: {0,-10} Reg: {1,-8} Color: {2,-8} Wheels: {3,-4} FuelType: {4,-10} Length: {5,-7} Type: {6,-10}",
            nameof(Boat),
            RegistrationNumber,
            Color,
            Wheels,
            FuelType,
            $"{ Lenght} m",
            Type
        );
    }
}
