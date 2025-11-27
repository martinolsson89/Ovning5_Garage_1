
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IHandler
{
    int GetAvailableSpots();
    (bool Success, int SeededCount, int RequestedCount, int AvailableSpots) SeedGarage(int requestedCount);
    (ParkResult, Vehicle? parkedVehicle) ParkVehicle(VehicleDto dto);
    bool RemoveVehicle(string registrationNumber);
    Dictionary<string, int> GetVehicleTypeCount();
    IEnumerable<Vehicle> GetAllVehicles();
    Vehicle? GetVehicleByRegNr(string regNr);
    IEnumerable<Vehicle> GetVehicles(VehicleType? vehicleType, string? color, int? wheels, FuelType? fuelType);
    IEnumerable<Vehicle> GetVehicles(VehicleQueryDto query);
}