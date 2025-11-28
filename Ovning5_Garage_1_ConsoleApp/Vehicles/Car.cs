using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Car : Vehicle
{
    public int NumberOfDoors { get; }
    public CarType Type { get; }

    public Car(string registrationNumber, string color, int wheels, FuelType fueltype, int numberOfDoors, CarType type)
        : base(registrationNumber, color, 4, fueltype)
    {
        if (numberOfDoors <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfDoors), "Number of doors must be greater than zero.");
        NumberOfDoors = numberOfDoors;
        Type = type;
    }

    public override string ToString()
    {
        return string.Format(
            "Vehicle: {0,-10} Reg: {1,-8} Color: {2,-8} Wheels: {3,-4} FuelType: {4,-10} Doors: {5,-8} Type: {6,-10}",
            nameof(Car),
            RegistrationNumber,
            Color,
            Wheels,
            FuelType,
            NumberOfDoors,
            Type
        );
    }
}
