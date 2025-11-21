
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Airplane : Vehicle
{
    public int Engines { get; }
    public int Wingspan { get; }

    public Airplane(string color, int wheels, FuelType fueltype, int engines, int wingspan)
        : base(color, wheels, fueltype)
    {
        Engines = engines;
        Wingspan = wingspan;
    }
}
