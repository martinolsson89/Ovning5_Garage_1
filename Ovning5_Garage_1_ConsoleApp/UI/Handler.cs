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

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _garage.GetVehicleByRegNr(regNr);
    }

    public IEnumerable<Vehicle> GetVehicles(Func<Vehicle, bool> predicate)
    {
         return _garage.GetVehicles(predicate);
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

    public bool SeedGarage()
    {
        var garageCapacity = _garage.Capacity;
        var garageCount = _garage.Count;
        var NrOfSeed = (garageCapacity - garageCount);

        if(NrOfSeed == 0)
        {
            return false;
        }

        var seedList = GenerateVehicles();

        if (NrOfSeed < 10)
        {
            for (int i = 0; i < NrOfSeed; i++)
            {
               (ParkResult parked, Vehicle? parkedVehicle) = _garage.Park(seedList[i]);
                if(parked != ParkResult.Success)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var seed in seedList)
            {
                (ParkResult parked, Vehicle? parkedVehicle) = _garage.Park(seed);
                if(parked != ParkResult.Success)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private List<Vehicle> GenerateVehicles()
    {
        List<Vehicle> seed = new List<Vehicle>();

        var car = new Car(color: "Red", wheels: 4, fueltype: FuelType.Hybrid, numberOfDoors: 4, type: CarType.Suv);
        var car1 = new Car(color: "Blue", wheels: 4, fueltype: FuelType.Electric, numberOfDoors: 2, type: CarType.SportsCar);
        var mc = new Motorcycle(color: "Green", wheels: 2, fueltype: FuelType.Gasoline, type: MotorcycleType.Sport, engineDisplacement: 110);
        var mc1 = new Motorcycle(color: "Black", wheels: 2, fueltype: FuelType.Hybrid, type: MotorcycleType.Cruiser, engineDisplacement: 120);
        var bus = new Bus(color: "Yellow", wheels: 4, fueltype: FuelType.Diesel, numberOfSeats: 20, isDoubleDecker: false);
        var bus1 = new Bus(color: "Red", wheels: 4, fueltype: FuelType.Gasoline, numberOfSeats: 18, isDoubleDecker: false);
        var boat = new Boat(color: "White", wheels: 0, fueltype: FuelType.None, type: BoatType.Sailboat, length: 12);
        var boat1 = new Boat(color: "Orange", wheels: 0, fueltype: FuelType.Gasoline, type: BoatType.Motorboat, length: 8);
        var plane = new Airplane(color: "Black", wheels: 2, fueltype: FuelType.Gasoline, engines: 2, wingspan: 20);
        var plane1 = new Airplane(color: "White", wheels: 4, fueltype: FuelType.Hybrid, engines: 4, wingspan: 32);

        seed.AddRange(car, mc, bus, boat, plane, car1, mc1, bus1, boat1, plane1);

        return seed;
    }


}
