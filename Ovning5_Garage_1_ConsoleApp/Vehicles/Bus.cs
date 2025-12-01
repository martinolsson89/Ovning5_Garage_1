using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Bus : Vehicle
{
    public int NumberOfSeats { get; }
    public bool IsDoubleDecker { get; } = false;

    public override VehicleType VehicleType => VehicleType.Bus;

    public Bus(string registrationNumber, string color, int wheels, FuelType fueltype, int numberOfSeats, bool isDoubleDecker)
        : base(registrationNumber, color, 4, fueltype)
    {
        if (numberOfSeats <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfSeats), "Number of seats must be greater than zero.");
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

    protected override bool MatchesSpecific(VehicleQueryDto query)
    {
        if (query.NumberOfSeats is not null &&
            NumberOfSeats != query.NumberOfSeats.Value)
            return false;

        if (query.IsDoubleDecker is not null &&
            IsDoubleDecker != query.IsDoubleDecker.Value)
            return false;

        return true;
    }
}
