
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IHandler
{
    bool SeedGarage();
    (ParkResult, Vehicle? parkedVehicle) ParkVehicle(Vehicle vehicle);
    bool RemoveVehicle(string registrationNumber);
    Dictionary<string, int> GetVehicleTypeCount();
    IEnumerable<Vehicle> GetAllVehicles();
}