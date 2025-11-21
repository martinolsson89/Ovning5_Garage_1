// See https://aka.ms/new-console-template for more information


using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

var car = new Car(color: "red", wheels: 4, fueltype: FuelType.Gasoline, type: CarType.Sedan, numberOfDoors: 4);
var car1 = new Car(color: "blue", wheels: 4, fueltype: FuelType.Diesel, type: CarType.Suv, numberOfDoors: 4);
var car2 = new Car(color: "orange", wheels: 4, fueltype: FuelType.Gasoline, type: CarType.Van, numberOfDoors: 6);
var car3 = new Car(color: "yellow", wheels: 4, fueltype: FuelType.Electric, type: CarType.SportsCar, numberOfDoors: 2);

Console.WriteLine($"{car.RegistrationNumber},  {car.Color}, {car.Wheels}, {car.FuelType}, {car.Type}, {car.NumberOfDoors}");
Console.WriteLine($"{car1.RegistrationNumber},  {car1.Color}, {car1.Wheels}, {car1.FuelType}, {car1.Type}, {car1.NumberOfDoors}");
Console.WriteLine($"{car2.RegistrationNumber},  {car2.Color}, {car2.Wheels}, {car2.FuelType}, {car2.Type}, {car2.NumberOfDoors}");
Console.WriteLine($"{car3.RegistrationNumber},  {car3.Color}, {car3.Wheels}, {car3.FuelType}, {car3.Type}, {car3.NumberOfDoors}");
