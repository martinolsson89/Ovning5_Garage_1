using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Bus : Vehicle
{
    public int NumberOfSeats { get; }
    public bool IsDoubleDecker { get; } = false;
    public Bus(string registrationNumber, string color, int wheels, FuelType fueltype, int numberOfSeats, bool isDoubleDecker)
        : base(registrationNumber, color, wheels, fueltype)
    {
        NumberOfSeats = numberOfSeats;
        IsDoubleDecker = isDoubleDecker;
    }

    public override string ToString()
    {
        return string.Format(
            "Vehicle: {0,-10} Reg: {1,-8} Color: {2,-8} Wheels: {3,-4} FuelType: {4,-10} Seats: {5,-8} DoubleDecker: {6,-5}",
            nameof(Bus),
            RegistrationNumber,
            Color,
            Wheels,
            FuelType,
            NumberOfSeats,
            IsDoubleDecker ? "Yes" : "No"
        );
    }
}
