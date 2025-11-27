using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Services;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System.Collections;

namespace Ovning5_Garage_1_ConsoleApp;

// Generic garage class that can store any type of vehicle
public class Garage<T> : IGarage<T>, IEnumerable<T> where T : Vehicle
{
    private T?[] _vehicles; // Internal array to store vehicles with nullable slots for empty spaces
    private readonly RegistrationNumberGenerator _regNumberGenerator;
    public int Capacity => _vehicles.Length;
    public int Count { get; private set; } // // Tracks the current number of parked vehicles

    public Garage(int capacity, RegistrationNumberGenerator? regNumberGenerator = null)
    {
        if(capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _vehicles = new T[capacity];
        _regNumberGenerator = regNumberGenerator ?? new RegistrationNumberGenerator();
    }

    // Generates a unique registration number using the registration number generator
    public string GenerateRegistrationNumber() => _regNumberGenerator.Generate();

    // Returns vehicles that match the given predicate (filter condition)
    public IEnumerable<T> GetVehicles(Func<T, bool> predicate)
    {
        foreach(var vehicle in _vehicles)
        {
            if(vehicle is not null && predicate(vehicle))
            {
                yield return vehicle;
            }
        }
    }

    // Finds and returns a vehicle by its registration number
    public T? GetVehicleByRegNr(string regNr)
    {
        return _vehicles.FirstOrDefault(v => v is not null && v.RegistrationNumber.Equals(regNr, StringComparison.CurrentCultureIgnoreCase));
    }

    // Parks a vehicle in the first available slot and returns the result status
    public (ParkResult, T? parkedVehicle) Park(T vehicle)
    {
        if(vehicle == null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        int firstFreeIndex = -1;

        for (int i = 0; i < _vehicles.Length; i++)
        {
            var current = _vehicles[i];

            // Check if vehicle with same registration number is already parked
            if(current is not null && current.RegistrationNumber == vehicle.RegistrationNumber)
            {
                return (ParkResult.AlreadyInGarage, parkedVehicle: null);
            }

            // Remember the first empty slot found
            if(current is null && firstFreeIndex == -1)
            {
                firstFreeIndex = i;
            }
        }

        // Park the vehicle in the first available slot
        if(firstFreeIndex != -1)
        {
            _vehicles[firstFreeIndex] = vehicle;
            Count++;
            return (ParkResult.Success, parkedVehicle: _vehicles[firstFreeIndex]);
        }

        return (ParkResult.GarageIsFull, parkedVehicle:null);
    }

    // Removes a vehicle by registration number and releases the registration for reuse
    public bool Remove(string regNr)
    {
        if (string.IsNullOrWhiteSpace(regNr))
        {
            throw new ArgumentException(nameof(regNr), "Registration number is required");
        }

        for (int i = 0; i < _vehicles.Length; i++)
        {
            var v = _vehicles[i];
            if (v is not null && v.RegistrationNumber.Equals(regNr, StringComparison.CurrentCultureIgnoreCase))
            {
                // Release the registration number when car is removed from garage.
                _regNumberGenerator.Release(v.RegistrationNumber);
                _vehicles[i] = null;
                Count--;
                return true;
            }
        }

        return false;
    }

    // Returns a dictionary with vehicle type names as keys and their counts as values
    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _vehicles
            .Where(t => t != null)
            .GroupBy(t => t!.GetType().Name)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Iterator that yields only non-null vehicles when enumerating
    public IEnumerator<T> GetEnumerator()
    {
        foreach (T? v in _vehicles)
        {
            if (v is not null)
            {
                yield return v;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
