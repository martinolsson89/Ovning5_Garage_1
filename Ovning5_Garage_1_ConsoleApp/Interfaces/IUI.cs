
namespace Ovning5_Garage_1_ConsoleApp.Interfaces;

public interface IUI
{
    void ShowMessage(string message);
    void ShowMenu();
    void ShowVehiclesSubMenu();
    string ReadInput(string prompt);
}
