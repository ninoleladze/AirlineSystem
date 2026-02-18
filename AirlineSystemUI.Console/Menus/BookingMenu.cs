using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class BookingMenu : IMenu
    {
        private BookingService BookingService = new BookingService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Ticket Booking");
                Console.WriteLine("1. Book a Ticket");
                Console.WriteLine("2. Cancel a Ticket");
                Console.WriteLine("3. My Tickets");
                Console.WriteLine("4. Search by Reference");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": BookingService.BookTicket(); break;
                        case "2": BookingService.CancelTicket(); break;
                        case "3": BookingService.ShowMyTickets(); break;
                        case "4": BookingService.SearchByReference(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}