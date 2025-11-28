using Microsoft.VisualBasic.FileIO;
using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Interfaces;
using Ovning5_Garage_1_ConsoleApp.Vehicles;
using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;



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

        var results = _garage.GetVehicles(v => MatchesQuery(v, query));

        return results;
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
        Vehicle vehicle = _vehicleFactory.CreateFromDto(dto);

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

    private static bool MatchesQuery(Vehicle v, VehicleQueryDto query)
    {
        bool matches = true;

        if (!string.IsNullOrWhiteSpace(query.Color))
        {
            matches &= v.Color.Equals(query.Color, StringComparison.OrdinalIgnoreCase);
        }

        if (query.Wheels.HasValue)
        {
            matches &= v.Wheels == query.Wheels.Value;
        }

        if (query.FuelType.HasValue)
        {
            matches &= v.FuelType == query.FuelType.Value;
        }

        if (query.VehicleType is not null)
        {
            matches &= v.GetType().Name.Equals(query.VehicleType.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        // Type specific filters

        if (query.CarType.HasValue || query.NumberOfDoors is not null)
        {
            matches &= v is Car car
                && (!query.CarType.HasValue || car.Type == query.CarType.Value)
                && (query.NumberOfDoors is null || car.NumberOfDoors == query.NumberOfDoors.Value);
        }


        if (query.MotorcycleType.HasValue || query.EngineDisplacement is not null)
        {
            matches &= v is Motorcycle mc
                && (!query.MotorcycleType.HasValue || mc.Type == query.MotorcycleType.Value)
                && (query.EngineDisplacement is null || mc.EngineDisplacement == query.EngineDisplacement.Value);
        }

        if (query.NumberOfSeats is not null || query.IsDoubleDecker is not null)
        {
            matches &= v is Bus bus
                && (query.NumberOfSeats is null || bus.NumberOfSeats == query.NumberOfSeats.Value)
                && (query.IsDoubleDecker is null || bus.IsDoubleDecker == query.IsDoubleDecker.Value);
        }

        if (query.BoatType.HasValue || query.Length is not null)
        {
            matches &= v is Boat boat
                && (!query.BoatType.HasValue || boat.Type == query.BoatType.Value)
                && (query.Length is null || boat.Length == query.Length.Value);
        }

        if (query.Engines is not null || query.Wingspan is not null)
        {
            matches &= v is Airplane plane
                && (query.Engines is null || plane.Engines == query.Engines.Value)
                && (query.Wingspan is null || plane.Wingspan == query.Wingspan.Value);
        }

        return matches;
    }

    #endregion
}
