using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System.Collections;

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

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T v in _vehicles)
        {
            yield return v;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
