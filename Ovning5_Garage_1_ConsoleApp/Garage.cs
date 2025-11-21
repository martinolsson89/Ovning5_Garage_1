using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System.Collections;
using System.Numerics;

namespace Ovning5_Garage_1_ConsoleApp;

public class Garage<T> : IEnumerable<T> where T : Vehicle
{
    private T[] _vehicles;

    public int Capacity => _vehicles.Length;

    public Garage(int capacity)
    {
        if(capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }


        _vehicles = new T[capacity];
    }

    public (bool parked, T? parkedVehicle) Park(T vehicle)
    {
        if(vehicle == null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        for (int i = 0; i < _vehicles.Length; i++)
        {
            if (_vehicles[i] is null)
            {
                _vehicles[i] = vehicle;
                return (parked: true,parkedVehicle: _vehicles[i]);
            }
        }

        return (parked: false, parkedVehicle:null);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T v in _vehicles)
        {
            yield return v;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
