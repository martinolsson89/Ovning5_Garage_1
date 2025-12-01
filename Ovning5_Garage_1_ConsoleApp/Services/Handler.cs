using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;



namespace Ovning5_Garage_1_ConsoleApp.Services;

public class Handler : IHandler
{
    private readonly IGarage<Vehicle> _garage;
    private readonly IVehicleFactory _vehicleFactory;

    public Handler(int capacity)
    {
        _garage = new Garage<Vehicle>(capacity);
        _vehicleFactory = new VehicleFactory(_garage.GenerateRegistrationNumber);
    }

    public int GetAvailableSpots()
    {
        return _garage.Capacity - _garage.Count;
    }

    public IEnumerable<Vehicle> GetAllVehicles() => _garage;

    public IEnumerable<Vehicle> GetVehicles(VehicleQueryDto query)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        // Filter matching vehicles
        var matches = _garage.Where(v => v.Matches(query));

        return matches;
    }

    public Vehicle? GetVehicleByRegNr(string regNr)
    {
        return _garage.GetVehicleByRegNr(regNr);
    }

    public Dictionary<string, int> GetVehicleTypeCount()
    {
        return _garage.GetVehicleTypeCount();
    }

    public (ParkResult, Vehicle? parkedVehicle) ParkVehicle(VehicleDto dto)
    {
        if(dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        // Create the appropriate vehicle type based on user choice
        Vehicle vehicle = _vehicleFactory.CreateFromDto(dto, false);

        if (vehicle is null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        return _garage.Park(vehicle);
    }

    public bool RemoveVehicle(string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
        {
            throw new ArgumentNullException(nameof(registrationNumber));
        }

        return _garage.Remove(registrationNumber);
    }

    public (bool Success, int SeededCount, int RequestedCount, int AvailableSpots) SeedGarage(int requestedCount)
    {
        var availableSpots = _garage.Capacity - _garage.Count;

        // Check if seeding is possible
        if (requestedCount <= 0 || availableSpots <= 0)
        {
            return (false, 0, requestedCount, availableSpots);
        }

        // Limit the number of vehicles to the available space
        var toCreate = Math.Min(requestedCount, availableSpots);

        var vehiclesToSeed = GenerateVehicles(toCreate);
        var seeded = 0;

        // Attempt to park each generated vehicle
        foreach (var v in vehiclesToSeed)
        {
            (ParkResult parked, Vehicle? _) = _garage.Park(v);
            if (parked == ParkResult.Success)
            {
                seeded++;
            }
        }

        return (seeded > 0, seeded, requestedCount, availableSpots);
    }

    #region Helper methods

    private IEnumerable<Vehicle> GenerateVehicles(int count)
    {
        // Lazy generate vehicles one at a time
        for (int i = 0; i < count; i++)
        {
            yield return _vehicleFactory.CreateRandomVehicle();
        }
    }

    #endregion
}
