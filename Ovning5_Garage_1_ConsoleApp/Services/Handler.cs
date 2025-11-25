using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;



namespace Ovning5_Garage_1_ConsoleApp.Services;

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
    public IEnumerable<Vehicle> GetVehicles(string? vehicleTypeChoice, string? color, int? wheels, FuelType? fuelType)
    {
        string? vehicleType = GetVehicleType(vehicleTypeChoice);


        // Build search predicate
        var results = _garage.GetVehicles(v =>
        {
            bool matches = true;

            if (!string.IsNullOrWhiteSpace(color))
            {
                matches &= v.Color.Equals(color, StringComparison.OrdinalIgnoreCase);
            }

            if (wheels.HasValue)
            {
                matches &= v.Wheels == wheels.Value;
            }

            if (fuelType.HasValue)
            {
                matches &= v.FuelType == fuelType.Value;
            }

            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                matches &= v.GetType().Name.Equals(vehicleType, StringComparison.OrdinalIgnoreCase);
            }

            return matches;
        });

        return results;

    }

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _garage.GetVehicleByRegNr(regNr);
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _garage.GetVehicleTypeCount();
    }

    public (ParkResult, Vehicle? parkedVehicle) ParkVehicle(int vehicleType, string color, int wheels, FuelType fuelType)
    {
        Vehicle? vehicle = CreateVehicleType(vehicleType, color, wheels, fuelType);

        if (vehicle is null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

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
        var NrOfSeed = garageCapacity - garageCount;

        if (NrOfSeed == 0)
        {
            return false;
        }

        var seedList = GenerateVehicles();

        if (NrOfSeed < 10)
        {
            for (int i = 0; i < NrOfSeed; i++)
            {
                (ParkResult parked, Vehicle? parkedVehicle) = _garage.Park(seedList[i]);
                if (parked != ParkResult.Success)
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
                if (parked != ParkResult.Success)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Helper methods

    private List<Vehicle> GenerateVehicles()
    {
        List<Vehicle> seed = new List<Vehicle>();

        var car = new Car(registrationNumber: _garage.GenerateRegistrationNumber(), color: "Red", wheels: 4, fueltype: FuelType.Hybrid, numberOfDoors: 4, type: CarType.Suv);
        var car1 = new Car(_garage.GenerateRegistrationNumber(), color: "Blue", wheels: 4, fueltype: FuelType.Electric, numberOfDoors: 2, type: CarType.SportsCar);
        var mc = new Motorcycle(_garage.GenerateRegistrationNumber(), color: "Green", wheels: 2, fueltype: FuelType.Gasoline, type: MotorcycleType.Sport, engineDisplacement: 110);
        var mc1 = new Motorcycle(_garage.GenerateRegistrationNumber(), color: "Black", wheels: 2, fueltype: FuelType.Hybrid, type: MotorcycleType.Cruiser, engineDisplacement: 120);
        var bus = new Bus(_garage.GenerateRegistrationNumber(), color: "Yellow", wheels: 4, fueltype: FuelType.Diesel, numberOfSeats: 20, isDoubleDecker: false);
        var bus1 = new Bus(_garage.GenerateRegistrationNumber(), color: "Red", wheels: 4, fueltype: FuelType.Gasoline, numberOfSeats: 18, isDoubleDecker: false);
        var boat = new Boat(_garage.GenerateRegistrationNumber(), color: "White", wheels: 0, fueltype: FuelType.None, type: BoatType.Sailboat, length: 12);
        var boat1 = new Boat(_garage.GenerateRegistrationNumber(), color: "Orange", wheels: 0, fueltype: FuelType.Gasoline, type: BoatType.Motorboat, length: 8);
        var plane = new Airplane(_garage.GenerateRegistrationNumber(), color: "Black", wheels: 2, fueltype: FuelType.Gasoline, engines: 2, wingspan: 20);
        var plane1 = new Airplane(_garage.GenerateRegistrationNumber(), color: "White", wheels: 4, fueltype: FuelType.Hybrid, engines: 4, wingspan: 32);

        seed.AddRange(car, mc, bus, boat, plane, car1, mc1, bus1, boat1, plane1);

        return seed;
    }

    private Vehicle? CreateVehicleType(int typeVehicle, string color, int wheels, FuelType fuelType)
    {

            Vehicle? vehicle = typeVehicle switch
            {
                1 => new Car(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, 4, CarType.Suv),
                2 => new Motorcycle(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, MotorcycleType.Chopper, 150),
                3 => new Bus(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, 22, false),
                4 => new Boat(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, BoatType.FishingBoat, 14),
                5 => new Airplane(_garage.GenerateRegistrationNumber(), color, wheels, fuelType, 2, 16),
                _ => null
            };

        return vehicle;
    }

    private string? GetVehicleType(string? choice)
    {
        string? vehicleType = choice switch
        {
            "1" => "Car",
            "2" => "Motorcycle",
            "3" => "Bus",
            "4" => "Boat",
            "5" => "Airplane",
            _ => null
        };

        return vehicleType;
    }


}
