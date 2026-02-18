using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class AuthMenu : IMenu
    {
        private AuthService AuthService = new AuthService();
        private UserService UserService = new UserService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Authentication & Profile");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Logout");
                Console.WriteLine("4. View My Balance");
                Console.WriteLine("5. Add Money to Wallet");
                Console.WriteLine("6. View My Profile");
                Console.WriteLine("7. Update My Profile");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": AuthService.Register(); break;
                        case "2": AuthService.Login(); break;
                        case "3": AuthService.Logout(); break;
                        case "4": UserService.ViewMyBalance(); break;
                        case "5": UserService.AddMoney(); break;
                        case "6": UserService.ViewMyProfile(); break;
                        case "7": UserService.UpdateMyProfile(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}