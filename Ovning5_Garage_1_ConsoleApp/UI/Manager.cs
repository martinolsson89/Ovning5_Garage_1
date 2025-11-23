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
        _handler.SeedGarage(); // Remove later

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

        _ui.ShowMessage("\nVehicles in garage:");
        foreach (var v in vehicles)
        {
            _ui.ShowMessage(v.ToString());
        }
    }

    private void GetVehicleTypeCount()
    {
        var counts = _handler.GetVehicleTypeCount();

        _ui.ShowMessage("\nVehicles types in garage:");
        foreach (var kvp in counts)
        {
            _ui.ShowMessage($"{kvp.Key}: {kvp.Value}");
        }

    }
}
