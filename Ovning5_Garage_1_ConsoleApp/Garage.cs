using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System.Collections;

namespace Ovning5_Garage_1_ConsoleApp;

public class Garage<T> : IEnumerable<T> where T : Vehicle
{
    private T?[] _vehicles;

    public int Capacity => _vehicles.Length;
    public int Count { get; private set; }

    public Garage(int capacity)
    {
        if(capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }


        _vehicles = new T[capacity];
    }

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

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _vehicles.FirstOrDefault(v => v?.RegistrationNumber.ToLower() == regNr.ToLower());
    }

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

            // Is the vehicle already parked in the garage ?
            if(current is not null && current.RegistrationNumber == vehicle.RegistrationNumber)
            {
                return (ParkResult.AlreadyInGarage, parkedVehicle: null);
            }

            // Save first free parking spot
            if(current is null && firstFreeIndex == -1)
            {
                firstFreeIndex = i;
            }
        }

        // Add vehicle to garage
        if(firstFreeIndex != -1)
        {
            _vehicles[firstFreeIndex] = vehicle;
            Count++;
            return (ParkResult.Success, parkedVehicle: _vehicles[firstFreeIndex]);
        }

        return (ParkResult.GarageIsFull, parkedVehicle:null);
    }

    public bool Remove(string regNr)
    {
        if (string.IsNullOrWhiteSpace(regNr))
        {
            throw new ArgumentException(nameof(regNr), "Registration number is required");
        }

        for (int i = 0; i < _vehicles.Length; i++)
        {
            var v = _vehicles[i];
            if (v is not null && v.RegistrationNumber.ToLower() == regNr.ToLower())
            {
                Vehicle.ReleaseRegistrationNumber(v.RegistrationNumber);
                _vehicles[i] = null;
                Count--;
                return true;
            }
        }

        return false;
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _vehicles
            .Where(t => t != null)
            .GroupBy(t => t!.GetType().Name)
            .ToDictionary(g => g.Key, g => g.Count());
    }



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
