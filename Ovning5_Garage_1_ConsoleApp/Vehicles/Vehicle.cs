using Ovning5_Garage_1_ConsoleApp.DTOs;
using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public abstract class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; protected set; }
    public int Wheels { get; protected set; }
    public FuelType FuelType { get; protected set; }

    public abstract VehicleType VehicleType { get; }

    protected Vehicle(string registrationNumber, string color, int wheels, FuelType fuelType)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new ArgumentException("Registration number cannot be empty.", nameof(registrationNumber));
        if (wheels < 0)
            throw new ArgumentOutOfRangeException(nameof(wheels), "Number of wheels cannot be negative.");

        RegistrationNumber = registrationNumber;
        Color = color;
        Wheels = wheels;
        FuelType = fuelType;
    }

    public override string ToString()
    {
        return string.Format(
            "Reg: {0,-12}  Color: {1,-8}  Wheels: {2,-6}  FuelType: {3,-10}",
            RegistrationNumber,
            Color,
            Wheels,
            FuelType
        );
    }

    // Public entry point for matches
    public bool Matches(VehicleQueryDto query)
    {
        if (!MatchesCommon(query))
            return false;

        return MatchesSpecific(query);
    }

    // Common filters for all vehicles
    private bool MatchesCommon(VehicleQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.Color) &&
            !Color.Equals(query.Color, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (query.Wheels.HasValue && Wheels != query.Wheels.Value)
        {
            return false;
        }

        if (query.FuelType.HasValue && FuelType != query.FuelType.Value)
        {
            return false;
        }

        if (query.VehicleType is not null && query.VehicleType != VehicleType)
        {
            return false;
        }

        return true;
    }

    // Hook for subclasses
    protected virtual bool MatchesSpecific(VehicleQueryDto query) => true;

}
