
namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IUI
{
    int AskForCapacity();
    void ShowMessage(string message);
    void ShowMenu();
    void ShowVehiclesSubMenu();
    void ShowAddVehicleSubMenu();
    string ReadInput(string prompt);
    int ReadInt (string prompt);
    string ReadUserInput(string prompt);
    int ReadIntInRange(string prompt, int min, int max);
    void ShowFuelTypeMenu();

}
