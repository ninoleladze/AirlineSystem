using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class SearchMenu : IMenu
    {
        private SearchService SearchService = new SearchService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Search & Filter");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Filter My Tickets by Date");
                Console.WriteLine("3. Search Crew Members");
                Console.WriteLine("4. Search All Tickets      [Admin]");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": SearchService.SearchFlights(); break;
                        case "2": SearchService.FilterTicketsByDate(); break;
                        case "3": SearchService.SearchCrew(); break;
                        case "4": SearchService.SearchAllTickets(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}