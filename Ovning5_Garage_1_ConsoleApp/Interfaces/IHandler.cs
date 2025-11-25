
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IHandler
{
    bool SeedGarage();
    (ParkResult, Vehicle? parkedVehicle) ParkVehicle(string typeChoice, string color, int wheels, FuelType fuelType);
    bool RemoveVehicle(string registrationNumber);
    Dictionary<string, int> GetVehicleTypeCount();
    IEnumerable<Vehicle> GetAllVehicles();
    Vehicle? GetVehicleByRegNr(string regNr);
    IEnumerable<Vehicle> GetVehicles(string? color, int? wheels, FuelType? fuelType, string? vehicleType);
}