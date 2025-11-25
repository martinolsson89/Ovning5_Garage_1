using System.Collections.Generic;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IGarage<T> : IEnumerable<T> where T : Vehicle
{
    int Capacity { get; }
    int Count { get; }
    string GenerateRegistrationNumber();
    IEnumerable<T> GetVehicles(Func<T, bool> predicate);
    T? GetVehicleByRegNr(string regNr);
    (ParkResult, T? parkedVehicle) Park(T vehicle);
    bool Remove(string regNr);
    Dictionary<string, int> GetVehicleTypeCount();
}
