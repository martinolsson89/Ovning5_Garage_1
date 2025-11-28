using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Services.VehiclesCreator;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Services;

public class VehicleFactory : IVehicleFactory
{
    // Generates unique registration numbers for created vehicles
    private readonly Func<string> _registrationNumberGenerator;

    private readonly Random _random;

    // Registered creators keyed by vehicle type
    private readonly Dictionary<VehicleType, VehicleCreator> _creators;

    public VehicleFactory(Func<string> registrationNumberGenerator, Random? random = null)
    {
        _registrationNumberGenerator = registrationNumberGenerator ??
            throw new ArgumentNullException(nameof(registrationNumberGenerator));
        _random = random ?? new Random();
        _creators = CreateVehicleCreators(_random);
    }

    // Create a vehicle from a DTO, delegating to the matching creator
    public Vehicle CreateFromDto(VehicleDto dto, bool allowRandomFallbacks = true)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var registrationNumber = _registrationNumberGenerator();

        if (!_creators.TryGetValue(dto.VehicleType, out var creator))
        {
            throw new ArgumentOutOfRangeException(nameof(dto.VehicleType));
        }
        return creator.Create(dto, registrationNumber, allowRandomFallbacks);
    }

    // Create a fully random vehicle (type and missing attributes)
    public Vehicle CreateRandomVehicle()
    {
        var vehicleType = GetRandomVehicleType();
        var dto = new VehicleDto
        {
            VehicleType = vehicleType,
            Color = string.Empty,
            FuelType = FuelType.None
        };

        return CreateFromDto(dto, allowRandomFallbacks: true);
    }

    // Register concrete creators for each vehicle type
    private Dictionary<VehicleType, VehicleCreator> CreateVehicleCreators(Random random)
    {
        VehicleCreator[] creators =
        {
            new CarCreator(random),
            new MotorcycleCreator(random),
            new BusCreator(random),
            new BoatCreator(random),
            new AirplaneCreator(random)
        };

        return creators.ToDictionary(c => c.Type, c => c);
    }

    // Select a random vehicle type
    private VehicleType GetRandomVehicleType()
    {
        var values = Enum.GetValues<VehicleType>();
        return values[_random.Next(values.Length)];
    }
}

