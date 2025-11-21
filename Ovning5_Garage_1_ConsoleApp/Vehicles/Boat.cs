using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Boat : Vehicle
{
    public BoatType Type { get; }
    public int Lenght { get; }

    public Boat(string color, int wheels, FuelType fueltype, BoatType type, int length)
        : base(color, wheels, fueltype)
    {
        Type = type;
        Lenght = length;
    }
    public override string ToString()
    {
        return $"Vehical: {nameof(Boat)} Reg: {RegistrationNumber}, Color: {Color}, Wheels: {Wheels}, FuelType: {FuelType}, Length: {Lenght} m, Type: {Type}";
    }
}
