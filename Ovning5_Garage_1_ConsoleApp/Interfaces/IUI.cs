
namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IUI
{
    void ShowMessage(string message);
    void ShowMenu();
    void ShowVehiclesSubMenu();

    void ShowAddVehicleSubMenu();
    string ReadInput(string prompt);
    int ReadInt (string prompt);
}
