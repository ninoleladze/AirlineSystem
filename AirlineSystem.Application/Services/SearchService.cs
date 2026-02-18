using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class SearchService: ISearchService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();

        public void SearchFlights()
        {
            AuthService.CheckLoggedIn();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Search Flights ===");
            Console.WriteLine("Departure airport (Enter to skip):"); string dep = Console.ReadLine()!.ToLower();
            Console.WriteLine("Arrival airport (Enter to skip):"); string arr = Console.ReadLine()!.ToLower();
            Console.WriteLine("Date yyyy-MM-dd (Enter to skip):"); string dateInput = Console.ReadLine()!;
            Console.WriteLine("Status 0=Scheduled 1=Boarding 2=Departed 3=Arrived 4=Cancelled (Enter to skip):");
            string statusInput = Console.ReadLine()!;
            Console.ResetColor();

            var query = DC.Flights.Include(f => f.Aircraft).AsQueryable();
            if (!string.IsNullOrWhiteSpace(dep))
                query = query.Where(f => f.DepartureAirport.ToLower().Contains(dep));
            if (!string.IsNullOrWhiteSpace(arr))
                query = query.Where(f => f.ArrivalAirport.ToLower().Contains(arr));
            if (DateTime.TryParse(dateInput, out DateTime date))
                query = query.Where(f => f.DepartureTime.Date == date.Date);
            if (int.TryParse(statusInput, out int sv) && Enum.IsDefined(typeof(FlightStatus), sv))
                query = query.Where(f => f.Status == (FlightStatus)sv);

            var results = query.OrderBy(f => f.DepartureTime).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {results.Count} Flight(s) Found ===");
            foreach (var f in results) Console.WriteLine(f);
            Console.ResetColor();
        }

        public void FilterTicketsByDate()
        {
            AuthService.CheckLoggedIn();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Filter My Tickets by Date ===");
            Console.WriteLine("From date (yyyy-MM-dd):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime from)) throw new Exception("Invalid from date.");
            Console.WriteLine("To date (yyyy-MM-dd):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime to) || to < from)
                throw new Exception("To date must be after from date.");
            Console.ResetColor();

            var tickets = DC.Tickets.Include(t => t.Flight)
                .Where(t => t.UserId == AuthService.LoggedInUser!.Id &&
                             t.CreatedAt.Date >= from.Date && t.CreatedAt.Date <= to.Date)
                .OrderByDescending(t => t.CreatedAt).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {tickets.Count} Ticket(s) Found ===");
            foreach (var t in tickets) Console.WriteLine($"{t} | Flight: {t.Flight?.FlightNumber}");
            Console.ResetColor();
        }

        public void SearchCrew()
        {
            AuthService.CheckLoggedIn();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Search Crew ===");
            Console.WriteLine("Role 0=Captain 1=CoPilot 2=Purser 3=Steward (Enter to skip):");
            string roleInput = Console.ReadLine()!;
            Console.WriteLine("Available only? y/n (Enter to skip):");
            string availInput = Console.ReadLine()!.ToLower();
            Console.WriteLine("Name contains (Enter to skip):");
            string nameInput = Console.ReadLine()!.ToLower();
            Console.ResetColor();

            var query = DC.CrewMembers.AsQueryable();
            if (int.TryParse(roleInput, out int rv) && Enum.IsDefined(typeof(CrewRole), rv))
                query = query.Where(c => c.Role == (CrewRole)rv);
            if (availInput == "y") query = query.Where(c => c.IsAvailable);
            if (!string.IsNullOrWhiteSpace(nameInput))
                query = query.Where(c => c.Name.ToLower().Contains(nameInput));

            var results = query.OrderBy(c => c.Name).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {results.Count} Crew Member(s) Found ===");
            foreach (var c in results) Console.WriteLine(c);
            Console.ResetColor();
        }

        public void SearchAllTickets()
        {
            AuthService.CheckAdmin();
            Console.WriteLine("Search by passenger name or booking reference:");
            string term = Console.ReadLine()!.ToLower();
            var tickets = DC.Tickets.Include(t => t.Flight).Include(t => t.User)
                .Where(t => t.PassengerName.ToLower().Contains(term) ||
                             t.BookingReference.ToLower().Contains(term)).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {tickets.Count} Result(s) ===");
            foreach (var t in tickets)
                Console.WriteLine($"{t} | User: {t.User.Username} | Flight: {t.Flight?.FlightNumber}");
            Console.ResetColor();
        }
    }
}