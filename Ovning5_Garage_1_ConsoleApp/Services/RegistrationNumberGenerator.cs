namespace Ovning5_Garage_1_ConsoleApp.Services;

public class RegistrationNumberGenerator
{
    private readonly Random _random = new();
    private readonly HashSet<string> _usedRegistrationNumbers = new(); // Tracks all registration numbers currently in use to prevent duplicates.
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVW";


    // Generates a unique registration number that is not currently in use.
    public string Generate()
    {
        while (true)
        {
            var regNr = GenerateRandomRegNr();

            // Add returns true if the item was added
            if (_usedRegistrationNumbers.Add(regNr))
                return regNr;
        }
    }


    // Releases a registration number, making it available for reuse.
    public void Release(string regNr)
    {
        _usedRegistrationNumbers.Remove(regNr);
    }


    // Checks if a registration number is available (not currently in use).
    public bool IsAvailable(string regNr)
    {
        return !_usedRegistrationNumbers.Contains(regNr);
    }


    // Generates a random registration number without checking for uniqueness.
    // Format: 3 letters followed by 3 digits (e.g. ABC123)
    private string GenerateRandomRegNr()
    {
        var regNr = "";

        for (int i = 0; i < 6; i++)
        {
            // First 3 characters are letters
            if (i < 3)
            {
                var index = _random.Next(Alphabet.Length);
                regNr += Alphabet[index];
            }
            // Last 3 characters are digits
            else
            {
                regNr += _random.Next(0, 10);
            }
        }

        return regNr;
    }
}
