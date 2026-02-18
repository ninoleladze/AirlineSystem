using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class AdminMenu : IMenu
    {
        private AdminService AdminService = new AdminService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Admin Panel");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. Revenue Report");
                Console.WriteLine("3. Assign Role to User");
                Console.WriteLine("4. Remove Role from User");
                Console.WriteLine("5. Update Maintenance Record");
                Console.WriteLine("6. View Logs");
                Console.WriteLine("7. Clear Logs");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": AdminService.ViewAllBookings(); break;
                        case "2": AdminService.RevenueReport(); break;
                        case "3": AdminService.AssignRole(); break;
                        case "4": AdminService.RemoveRole(); break;
                        case "5": AdminService.UpdateMaintenanceRecord(); break;
                        case "6": LogService.ViewLogs(); break;
                        case "7": LogService.ClearLogs(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}