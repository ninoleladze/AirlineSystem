using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class FlightMenu : IMenu
    {
        private FlightService FlightService = new FlightService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Flight Management");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. Create Flight           [Admin]");
                Console.WriteLine("3. Update Flight Status    [Admin]");
                Console.WriteLine("4. Delete Flight           [Admin]");
                Console.WriteLine("5. View Flight Manifest    [Admin]");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": FlightService.GetAllFlights(); break;
                        case "2": FlightService.CreateFlight(); break;
                        case "3": FlightService.UpdateFlightStatus(); break;
                        case "4": FlightService.DeleteFlight(); break;
                        case "5": FlightService.ViewFlightManifest(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}