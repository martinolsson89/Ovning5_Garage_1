using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Motorcycle : Vehicle
{
    public MotorcycleType Type { get; }
    public int EngineDisplacement { get; }
    public Motorcycle(string color, int wheels, FuelType fueltype, MotorcycleType type, int engineDiscplacement)
        : base(color, wheels, fueltype)
    {
        Type = type;
        EngineDisplacement = engineDiscplacement;
    }
}
