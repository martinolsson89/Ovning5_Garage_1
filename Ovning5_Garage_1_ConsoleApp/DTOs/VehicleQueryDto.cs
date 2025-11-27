
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.DTOs;

public class VehicleQueryDto
{
    // Base
    public VehicleType? VehicleType { get; set; }
    public string? Color { get; set; }
    public int? Wheels { get; set; }
    public FuelType? FuelType { get; set; }

    // Car
    public int? NumberOfDoors { get; set; }
    public CarType? CarType { get; set; }

    // Motorcycle
    public MotorcycleType? MotorcycleType { get; set; }
    public int? EngineDisplacement { get; set; }

    // Bus
    public int? NumberOfSeats { get; set; }
    public bool? IsDoubleDecker { get; set; }

    // Boat
    public BoatType? BoatType { get; set; }
    public int? Length { get; set; }

    // Airplane
    public int? Engines { get; set; }
    public int? Wingspan { get; set; }
}
