using Ovning5_Garage_1_ConsoleApp.Interfaces;

namespace Ovning5_Garage_1_ConsoleApp.UI;

public class Manager
{
    private IUI _ui;
    private IHandler _handler;

    public Manager(IUI ui, IHandler handler)
    {
        _ui = ui;
        _handler = handler;
    }

    public void Run()
    {
        //_handler.SeedGarage();

        bool running = true;
        while (running)
        {
            _ui.ShowMenu();
            var choice = _ui.ReadInput("Select menu option: ");

            switch (choice)
            {
                case "1":
                    VehiclesSubMenu();
                    break;

                case "2":
                    SeedVehicles();
                    break;

                case "3":

                    break;

                case "0":
                    running = false;
                    break;

                default:
                    _ui.ShowMessage("Invalid choice, try again.");
                    break;
            }
        }
    }

    private void SeedVehicles()
    {
        var IsSeedSuccessful = _handler.SeedGarage();
        if (!IsSeedSuccessful)
        {
            _ui.ShowMessage("\nSeed was not successful, garage maybe full");
        }
        else
        {
            _ui.ShowMessage("\nSeed was successfull, garage is now populated with vehicles");
        }
    }

    private void VehiclesSubMenu()
    {
        while (true)
        {
            _ui.ShowVehiclesSubMenu();
            var choice = _ui.ReadInput("Select menu option: ");

            switch (choice)
            {
                case "1":
                    GetVehicles();
                    return;

                case "2":
                    GetVehicleTypeCount();
                    return;

                default:
                    _ui.ShowMessage("Invalid choice, try again.");
                    break;
            }

        }
    }

    private void GetVehicles()
    {
        var vehicles = _handler.GetAllVehicles();

        if(vehicles.Count() == 0)
        {
            _ui.ShowMessage("\nGarage is empty");
        }
        else
        {
            _ui.ShowMessage("\nVehicles in garage:");
            foreach (var v in vehicles)
            {
                _ui.ShowMessage(v.ToString());
            }
        }
    }

    private void GetVehicleTypeCount()
    {
        var counts = _handler.GetVehicleTypeCount();

        if(counts.Count() == 0)
        {
            _ui.ShowMessage("\nGarage is empty");
        }
        else
        {
            _ui.ShowMessage("\nVehicles types in garage:");
            foreach (var kvp in counts)
            {
                _ui.ShowMessage($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
