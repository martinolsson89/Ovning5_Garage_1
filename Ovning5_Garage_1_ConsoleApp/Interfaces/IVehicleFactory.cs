
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IVehicleFactory
{
    Vehicle CreateFromDto(VehicleDto dto, bool allowRandomFallbacks = true);
    Vehicle CreateRandomVehicle();
}
