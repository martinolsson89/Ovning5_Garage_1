using Ovning5_Garage_1_ConsoleApp;
using Ovning5_Garage_1_ConsoleApp.Enums;
using Ovning5_Garage_1_ConsoleApp.Vehicles;

var car = new Car(color: "red", wheels: 4, fueltype: FuelType.Gasoline, type: CarType.Sedan, numberOfDoors: 4);
var boat = new Boat(color: "white", wheels: 0, fueltype: FuelType.Gasoline, type: BoatType.Motorboat, length: 12);
var bus = new Bus(color: "yellow", wheels: 4, fueltype: FuelType.Diesel, numberOfSeats: 22, isDoubleDecker: false);
var motorcycle = new Motorcycle(color: "green", wheels: 2, fueltype: FuelType.Electric, type: MotorcycleType.Sport, engineDisplacement: 110);

var garage = new Garage<Vehicle>(4);

garage.Park(car);
garage.Park(boat);
garage.Park(bus);
garage.Park(motorcycle);

foreach(var item in garage)
{
    Console.WriteLine(item);
}


