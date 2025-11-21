using Ovning5_Garage_1_ConsoleApp.Enums;

namespace Ovning5_Garage_1_ConsoleApp.Vehicles;

public abstract class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; protected set; }
    public int Wheels { get; protected set; }
    public FuelType FuelType { get; protected set; }

    private static readonly Random _random = new();
    private static readonly HashSet<string> _usedRegistrationNumbers = new();

    protected Vehicle(string color, int wheels, FuelType fuelType)
    {
        RegistrationNumber = GetRegNr();
        Color = color;
        Wheels = wheels;
        FuelType = fuelType;
    }

    private static string GetRegNr()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVW";

        // calling generate random reg number method and checking that it is unique.
        while (true)
        {
            var regNr = GenerateRandomRegNr(alphabet);

            if (_usedRegistrationNumbers.Add(regNr))
                return regNr; // HashSet.Add will return false if it already exist and the loop will continue.
        }
    }

    // Generateing random reg number
    private static string GenerateRandomRegNr(string alphabet)
    {
        var randomRegNr = "";

        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                // generating a random letter from alphabet
                var index = _random.Next(alphabet.Length);
                randomRegNr += alphabet[index];
            }
            else
            {
                // generating a random number between 0-9
                randomRegNr += $"{_random.Next(0, 10)}";
            }
        }

        return randomRegNr;

    }
}
