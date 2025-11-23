
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public class Car : Vehicle
{
    public int NumberOfDoors { get; }
    public CarType Type { get; }

    public Car(string color, int wheels, FuelType fueltype, int numberOfDoors, CarType type)
        : base(color, wheels, fueltype)
    {
        NumberOfDoors = numberOfDoors;
        Type = type;
    }

    public override string ToString()
    {
        return $"Vehical: {nameof(Car)}, Reg: {RegistrationNumber}, Color: {Color}, Wheels: {Wheels}, FuelType: {FuelType}, Nr of doors: {NumberOfDoors}, Type: {Type}";
    }
}
