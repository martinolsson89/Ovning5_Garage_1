using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

namespace Ovning5_Garage_1_ConsoleApp.Tests;

public class GarageTests
{
    #region Constructor Tests
    [Fact]
    public void Constructor_Should_CreateGarage_When_CapacityIsValid()
    {
        // Arrange & Act
        var garage = new Garage<Vehicle>(capacity: 10);

        // Assert
        Assert.Equal(10, garage.Capacity);
        Assert.Equal(0, garage.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_Should_ThrowArgumentOutOfRangeException_When_CapacityIsInvalid(int invalidCapacity)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(invalidCapacity));
    }
    #endregion

    #region Park Tests
    [Fact]
    public void Park_Should_ReturnSuccess_When_SpaceAvailable()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);

        // Act
        var (result, parkedVehicle) = garage.Park(car);

        // Assert
        Assert.Equal(ParkResult.Success, result);
        Assert.NotNull(parkedVehicle);
        Assert.Equal(1, garage.Count);
    }
    [Fact]
    public void Park_Should_ThrowArgumentNullException_When_VehicleIsNull()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => garage.Park(null!));
    }

    [Fact]
    public void Park_Should_ReturnAlreadyInGarage_When_SameRegistrationNumberExists()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("ABC123", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        garage.Park(car1);

        // Act
        var (result, parkedVehicle) = garage.Park(car2);

        // Assert
        Assert.Equal(ParkResult.AlreadyInGarage, result);
        Assert.Null(parkedVehicle);
        Assert.Equal(1, garage.Count);
    }

    [Fact]
    public void Park_Should_ReturnGarageIsFull_When_NoSpaceAvailable()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 2);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        var car3 = new Car("GHI789", "Green", 4, FuelType.Diesel, 4, CarType.SportsCar);
        garage.Park(car1);
        garage.Park(car2);

        // Act
        var (result, parkedVehicle) = garage.Park(car3);

        // Assert
        Assert.Equal(ParkResult.GarageIsFull, result);
        Assert.Null(parkedVehicle);
        Assert.Equal(2, garage.Count);
    }

    [Fact]
    public void Park_Should_IncrementCount_When_Successful()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);

        // Act
        garage.Park(car1);
        garage.Park(car2);

        // Assert
        Assert.Equal(2, garage.Count);
    }

    #endregion

    #region Remove Tests
    [Fact]
    public void Remove_Should_ReturnTrue_When_VehicleExists()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.Remove("ABC123");

        // Assert
        Assert.True(result);
        Assert.Equal(0, garage.Count);
    }

    [Fact]
    public void Remove_Should_ReturnFalse_When_VehicleDoesNotExist()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.Remove("XYZ999");

        // Assert
        Assert.False(result);
        Assert.Equal(1, garage.Count);
    }

    [Fact]
    public void Remove_Should_BeCaseInsensitive()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.Remove("abc123");

        // Assert
        Assert.True(result);
        Assert.Equal(0, garage.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Remove_Should_ThrowArgumentException_When_RegistrationNumberIsInvalid(string? invalidRegNr)
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => garage.Remove(invalidRegNr!));
    }

    [Fact]
    public void Remove_Should_DecrementCount_When_Successful()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        garage.Park(car1);
        garage.Park(car2);

        // Act
        garage.Remove("ABC123");

        // Assert
        Assert.Equal(1, garage.Count);
    }
    #endregion

    #region GetVehicleByRegNr Tests

    [Fact]
    public void GetVehicleByRegNr_Should_ReturnVehicle_When_Exists()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.GetVehicleByRegNr("ABC123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ABC123", result.RegistrationNumber);
    }

    [Fact]
    public void GetVehicleByRegNr_Should_ReturnNull_When_VehicleDoesNotExist()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.GetVehicleByRegNr("XYZ999");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetVehicleByRegNr_Should_BeCaseInsensitive()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var result = garage.GetVehicleByRegNr("abc123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ABC123", result.RegistrationNumber);
    }

    #endregion

    #region GetVehicles Tests

    [Fact]
    public void GetVehicles_Should_ReturnMatchingVehicles_When_PredicateMatches()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        var motorcycle = new Motorcycle("GHI789", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);
        garage.Park(car1);
        garage.Park(car2);
        garage.Park(motorcycle);

        // Act
        var redVehicles = garage.GetVehicles(v => v.Color == "Red").ToList();

        // Assert
        Assert.Single(redVehicles);
        Assert.Equal("ABC123", redVehicles[0].RegistrationNumber);
    }

    [Fact]
    public void GetVehicles_Should_ReturnEmpty_When_NoMatches()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        garage.Park(car);

        // Act
        var yellowVehicles = garage.GetVehicles(v => v.Color == "Yellow").ToList();

        // Assert
        Assert.Empty(yellowVehicles);
    }

    [Fact]
    public void GetVehicles_Should_ReturnEmpty_When_GarageIsEmpty()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act
        var vehicles = garage.GetVehicles(v => true).ToList();

        // Assert
        Assert.Empty(vehicles);
    }

    [Fact]
    public void GetVehicles_Should_FilterByType()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var bus = new Bus("DEF456", "Yellow", 6, FuelType.Diesel, 50, false);
        var motorcycle = new Motorcycle("GHI789", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);
        garage.Park(car);
        garage.Park(bus);
        garage.Park(motorcycle);

        // Act
        var motorcycles = garage.GetVehicles(v => v is Motorcycle).ToList();

        // Assert
        Assert.Single(motorcycles);
        Assert.IsType<Motorcycle>(motorcycles[0]);
    }

    #endregion

    #region GetVehicleTypeCount Tests

    [Fact]
    public void GetVehicleTypeCount_Should_ReturnCorrectCounts_When_MultipleTypesExist()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 10);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        var motorcycle1 = new Motorcycle("GHI789", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);
        var motorcycle2 = new Motorcycle("JKL012", "White", 2, FuelType.Electric, MotorcycleType.Cruiser, 800);
        var bus = new Bus("MNO345", "Yellow", 6, FuelType.Diesel, 50, false);
        garage.Park(car1);
        garage.Park(car2);
        garage.Park(motorcycle1);
        garage.Park(motorcycle2);
        garage.Park(bus);

        // Act
        var typeCounts = garage.GetVehicleTypeCount();

        // Assert
        Assert.Equal(3, typeCounts.Count);
        Assert.Equal(2, typeCounts["Car"]);
        Assert.Equal(2, typeCounts["Motorcycle"]);
        Assert.Equal(1, typeCounts["Bus"]);
    }

    [Fact]
    public void GetVehicleTypeCount_Should_ReturnEmptyDictionary_When_GarageIsEmpty()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act
        var typeCounts = garage.GetVehicleTypeCount();

        // Assert
        Assert.Empty(typeCounts);
    }

    [Fact]
    public void GetVehicleTypeCount_Should_NotIncludeRemovedVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car1 = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var car2 = new Car("DEF456", "Blue", 4, FuelType.Electric, 2, CarType.Sedan);
        garage.Park(car1);
        garage.Park(car2);
        garage.Remove("ABC123");

        // Act
        var typeCounts = garage.GetVehicleTypeCount();

        // Assert
        Assert.Single(typeCounts);
        Assert.Equal(1, typeCounts["Car"]);
    }

    #endregion

    #region GenerateRegistrationNumber Tests

    [Fact]
    public void GenerateRegistrationNumber_Should_ReturnNonEmptyString()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act
        var regNumber = garage.GenerateRegistrationNumber();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(regNumber));
    }

    [Fact]
    public void GenerateRegistrationNumber_Should_ReturnUniqueNumbers()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 100);
        var regNumbers = new HashSet<string>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            regNumbers.Add(garage.GenerateRegistrationNumber());
        }

        // Assert
        Assert.Equal(100, regNumbers.Count);
    }

    #endregion

    #region GetEnumerator Tests

    [Fact]
    public void GetEnumerator_Should_IterateOverParkedVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var bus = new Bus("DEF456", "Yellow", 6, FuelType.Diesel, 50, false);
        var motorcycle = new Motorcycle("GHI789", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);
        garage.Park(car);
        garage.Park(bus);
        garage.Park(motorcycle);

        // Act
        var vehicles = garage.ToList();

        // Assert
        Assert.Equal(3, vehicles.Count);
        Assert.Contains(vehicles, v => v.RegistrationNumber == "ABC123");
        Assert.Contains(vehicles, v => v.RegistrationNumber == "DEF456");
        Assert.Contains(vehicles, v => v.RegistrationNumber == "GHI789");
    }

    [Fact]
    public void GetEnumerator_Should_ReturnEmpty_When_GarageIsEmpty()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);

        // Act
        var vehicles = garage.ToList();

        // Assert
        Assert.Empty(vehicles);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Integration_ParkAndRemoveMultipleVehicles_Should_MaintainCorrectState()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var bus = new Bus("DEF456", "Yellow", 6, FuelType.Diesel, 50, false);
        var motorcycle = new Motorcycle("GHI789", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);

        // Act & Assert
        garage.Park(car);
        garage.Park(bus);
        garage.Park(motorcycle);
        Assert.Equal(3, garage.Count);

        garage.Remove("DEF456");
        Assert.Equal(2, garage.Count);
        Assert.Null(garage.GetVehicleByRegNr("DEF456"));

        var boat = new Boat("XYZ999", "White", 0, FuelType.Diesel, BoatType.Sailboat, 15);
        garage.Park(boat);
        Assert.Equal(3, garage.Count);

        var allVehicles = garage.ToList();
        Assert.Equal(3, allVehicles.Count);
    }

    [Fact]
    public void Integration_AllVehicleTypes_Should_WorkCorrectly()
    {
        // Arrange
        var garage = new Garage<Vehicle>(capacity: 10);
        var car = new Car("CAR001", "Red", 4, FuelType.Gasoline, 4, CarType.Suv);
        var motorcycle = new Motorcycle("MOTO01", "Black", 2, FuelType.Gasoline, MotorcycleType.Sport, 600);
        var bus = new Bus("BUS001", "Yellow", 6, FuelType.Diesel, 50, false);
        var boat = new Boat("BOAT01", "White", 0, FuelType.Diesel, BoatType.Sailboat, 15);
        var airplane = new Airplane("AIR001", "Silver", 12, FuelType.Gasoline, 2, 40);

        // Act
        garage.Park(car);
        garage.Park(motorcycle);
        garage.Park(bus);
        garage.Park(boat);
        garage.Park(airplane);

        // Assert
        Assert.Equal(5, garage.Count);
        var typeCounts = garage.GetVehicleTypeCount();
        Assert.Equal(5, typeCounts.Count);
        Assert.Equal(1, typeCounts["Car"]);
        Assert.Equal(1, typeCounts["Motorcycle"]);
        Assert.Equal(1, typeCounts["Bus"]);
        Assert.Equal(1, typeCounts["Boat"]);
        Assert.Equal(1, typeCounts["Airplane"]);
    }

    #endregion
}
