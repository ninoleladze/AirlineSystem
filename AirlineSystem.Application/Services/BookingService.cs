using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Domain.Events;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class BookingService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();
        private FlightService FlightService = new FlightService();

        public void BookTicket()
        {
            AuthService.CheckLoggedIn();
            FlightService.GetAllFlights();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Book Ticket ===");
            Console.WriteLine("Enter flight ID:");
            int flightId = int.Parse(Console.ReadLine()!);
            var flight = DC.Flights.Find(flightId);
            if (flight == null) throw new Exception("Flight not found.");
            if (flight.Status != FlightStatus.Scheduled && flight.Status != FlightStatus.Boarding)
                throw new Exception("Booking not available for this flight status.");
            if (flight.AvailableSeats <= 0) throw new Exception("No seats available on this flight.");

            Console.WriteLine("Passenger Name:");
            string passengerName = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(passengerName)) throw new Exception("Passenger name cannot be empty.");

            Console.WriteLine("Seat Number (e.g. 12A):");
            string seatNumber = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(seatNumber)) throw new Exception("Seat number cannot be empty.");
            if (DC.Tickets.Any(t => t.FlightId == flightId && t.SeatNumber == seatNumber && !t.IsCanceled))
                throw new Exception($"Seat {seatNumber} is already taken.");

            // ← REMOVE THE PRICE PROMPT, USE FLIGHT PRICE INSTEAD
            Console.WriteLine($"Ticket Price: {flight.BasePrice} {flight.PriceCurrency}");
            Console.ResetColor();

            string bookingRef = $"BK-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

            var ticket = new Ticket
            {
                BookingReference = bookingRef,
                SeatNumber = seatNumber,
                PassengerName = passengerName,
                FlightId = flightId,
                UserId = AuthService.LoggedInUser!.Id,
                PriceAmount = flight.BasePrice,      // ← USE FLIGHT PRICE
                PriceCurrency = flight.PriceCurrency   // ← USE FLIGHT CURRENCY
            };
            flight.AvailableSeats--;
            DC.Tickets.Add(ticket);

            try
            {
                DC.SaveChanges();
                var evt = new TicketBookedEvent(ticket.Id, flightId, passengerName);
                LogService.LogInfo($"Ticket booked: {bookingRef} | Flight: {flightId}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Ticket booked! Reference: {bookingRef} | Seat: {seatNumber} | Price: {ticket.PriceAmount} {ticket.PriceCurrency}");
                Console.ResetColor();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Booking conflict detected. Please try again.");
            }
        }

        public void CancelTicket()
        {
            AuthService.CheckLoggedIn();
            ShowMyTickets();
            Console.WriteLine("Enter ticket ID to cancel:");
            int ticketId = int.Parse(Console.ReadLine()!);
            var ticket = DC.Tickets.Include(t => t.Flight).FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket not found.");
            if (ticket.UserId != AuthService.LoggedInUser!.Id) throw new Exception("You can only cancel your own tickets.");
            if (ticket.IsCanceled) throw new Exception("Ticket is already cancelled.");
            if (ticket.Flight.Status == FlightStatus.Departed ||
                ticket.Flight.Status == FlightStatus.Arrived)
                throw new Exception("Cannot cancel a ticket for a departed or arrived flight.");

            ticket.IsCanceled = true;
            ticket.Flight.AvailableSeats++;
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ticket cancelled successfully.");
            Console.ResetColor();
        }

        public void ShowMyTickets()
        {
            AuthService.CheckLoggedIn();
            var tickets = DC.Tickets.Include(t => t.Flight)
                .Where(t => t.UserId == AuthService.LoggedInUser!.Id).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== My Tickets ===");
            if (tickets.Count == 0) { Console.WriteLine("You have no tickets."); Console.ResetColor(); return; }
            foreach (var t in tickets) Console.WriteLine($"{t} | Flight: {t.Flight?.FlightNumber}");
            Console.ResetColor();
        }

        public void SearchByReference()
        {
            AuthService.CheckLoggedIn();
            Console.WriteLine("Enter booking reference:");
            string reference = Console.ReadLine()!;
            var ticket = DC.Tickets.Include(t => t.Flight).Include(t => t.User)
                .FirstOrDefault(t => t.BookingReference == reference);
            if (ticket == null) throw new Exception("Ticket not found.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Ticket Details ===");
            Console.WriteLine(ticket);
            Console.WriteLine($"Flight: {ticket.Flight}");
            Console.ResetColor();
        }
    }
}
