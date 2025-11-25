using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public abstract class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; protected set; }
    public int Wheels { get; protected set; }
    public FuelType FuelType { get; protected set; }

    protected Vehicle(string registrationNumber, string color, int wheels, FuelType fuelType)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new ArgumentException("Registration number cannot be empty.", nameof(registrationNumber));

        RegistrationNumber = registrationNumber;
        Color = color;
        Wheels = wheels;
        FuelType = fuelType;
    }

    public override string ToString()
    {
        return string.Format(
            "Reg: {0,-12}  Color: {1,-8}  Wheels: {2,-6}  FuelType: {3,-10}",
            RegistrationNumber,
            Color,
            Wheels,
            FuelType
        );
    }

}
