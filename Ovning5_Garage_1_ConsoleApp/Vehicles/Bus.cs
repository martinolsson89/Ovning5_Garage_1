
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Bus : Vehicle
{
    public int NumberOfSeats { get; }
    public bool IsDoubleDecker { get; } = false;
    public Bus(string color, int wheels, FuelType fueltype, int numberOfSeats, bool isDoubleDecker)
        : base(color, wheels, fueltype)
    {
        NumberOfSeats = numberOfSeats;
        IsDoubleDecker = isDoubleDecker;
    }
}
