using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.UI;

public class Handler : IHandler
{
    private readonly Garage<Vehicle> _garage;
    public Handler(int capacity)
    {
        _garage = new Garage<Vehicle>(capacity);
    }

    public IEnumerable<Vehicle> GetAllVehicles()
    {
        return _garage;
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _garage.GetVehicleTypeCount();
    }

    public (ParkResult, Vehicle? parkedVehicle) ParkVehicle(Vehicle vehicle)
    {
        return _garage.Park(vehicle);
    }

    public bool RemoveVehicle(string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
        {
            throw new ArgumentNullException(nameof(registrationNumber));
        }

        return _garage.Remove(registrationNumber);
    }

    public void SeedGarage()
    {
        var car = new Car(color: "Red", wheels: 4, fueltype: FuelType.Hybrid, numberOfDoors: 4, type: CarType.Suv);
        var mc = new Motorcycle(color: "Green", wheels: 2, fueltype: FuelType.Gasoline, type: MotorcycleType.Sport, engineDisplacement: 110);
        var bus = new Bus(color: "Yellow", wheels: 4, fueltype: FuelType.Diesel, numberOfSeats: 20, isDoubleDecker: false);

        _garage.Park(car);
        _garage.Park(mc);
        _garage.Park(bus);
    }


}
