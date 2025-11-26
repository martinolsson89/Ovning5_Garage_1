using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Motorcycle : Vehicle
{
    public MotorcycleType Type { get; }
    public int EngineDisplacement { get; }
    public Motorcycle(string registrationNumber, string color, int wheels, FuelType fueltype, MotorcycleType type, int engineDisplacement)
        : base(registrationNumber, color, wheels, fueltype)
    {
        if (engineDisplacement <= 0)
            throw new ArgumentOutOfRangeException(nameof(engineDisplacement), "Engine displacement must be greater than zero.");
        Type = type;
        EngineDisplacement = engineDisplacement;
    }

    public override string ToString()
    {
        return string.Format(
            "Vehicle: {0,-10} Reg: {1,-8} Color: {2,-8} Wheels: {3,-4} FuelType: {4,-10} Engine: {5,-7} Type: {6,-10}",
            nameof(Motorcycle),
            RegistrationNumber,
            Color,
            Wheels,
            FuelType,
            $"{EngineDisplacement} cc",
            Type
        );
    }
}
