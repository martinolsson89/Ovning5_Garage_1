
namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IUI
{
    int AskForCapacity();
    void ShowMessage(string message);
    void ShowMenu();
    void ShowVehiclesSubMenu();
    void ShowAddVehicleSubMenu();
    void ShowFuelTypeMenu();
    string ReadInput(string prompt);
    string ReadUserInput(string prompt);
    int ReadInt (string prompt);
    int ReadIntInRange(string prompt, int min, int max);
    public bool ReadYesNo(string prompt);
}
