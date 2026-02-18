using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Domain.Events;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class FlightService : IFlightService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();


        public void CreateFlight()
        {
            AuthService.CheckAdmin();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Create Flight ===");

            Console.WriteLine("Flight Number:");
            string flightNumber = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(flightNumber)) throw new Exception("Flight number cannot be empty.");
            if (DC.Flights.Any(f => f.FlightNumber == flightNumber)) throw new Exception("Flight number already exists.");

            Console.WriteLine("Departure Airport:");
            string dep = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(dep)) throw new Exception("Departure airport cannot be empty.");

            Console.WriteLine("Arrival Airport:");
            string arr = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(arr)) throw new Exception("Arrival airport cannot be empty.");

            Console.WriteLine("Departure Time (yyyy-MM-dd HH:mm):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime depTime))
                throw new Exception("Invalid departure time.");

            Console.WriteLine("Arrival Time (yyyy-MM-dd HH:mm):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime arrTime))
                throw new Exception("Invalid arrival time.");
            if (arrTime <= depTime) throw new Exception("Arrival must be after departure.");

            Console.WriteLine("Base Ticket Price:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                throw new Exception("Invalid price.");

            ShowAllAircrafts();
            Console.WriteLine("Aircraft ID:");
            int aircraftId = int.Parse(Console.ReadLine()!);
            var aircraft = DC.Aircrafts.Find(aircraftId);
            if (aircraft == null) throw new Exception("Aircraft not found.");

            bool conflict = DC.Flights.Any(f =>
                f.AircraftId == aircraftId && f.Status != FlightStatus.Cancelled &&
                ((depTime >= f.DepartureTime && depTime < f.ArrivalTime) ||
                 (arrTime > f.DepartureTime && arrTime <= f.ArrivalTime)));
            if (conflict) throw new Exception("Aircraft already scheduled during that time window.");
            Console.ResetColor();

            var flight = new Flight
            {
                FlightNumber = flightNumber,
                DepartureAirport = dep,
                ArrivalAirport = arr,
                DepartureTime = depTime,
                ArrivalTime = arrTime,
                Status = FlightStatus.Scheduled,
                AircraftId = aircraftId,
                AvailableSeats = aircraft.Capacity,
                BasePrice = price,      
                PriceCurrency = "USD"        
            };
            DC.Flights.Add(flight);
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight created successfully.");
            Console.ResetColor();
        }

        public void GetAllFlights()
        {
            AuthService.CheckLoggedIn();
            var flights = DC.Flights.Include(f => f.Aircraft).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== All Flights ===");
            Console.WriteLine(new string('-', 100));
            foreach (var f in flights)
            {
                Console.WriteLine(f);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"   Price: {f.BasePrice} {f.PriceCurrency} | Aircraft: {f.Aircraft.Model}");
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            Console.WriteLine(new string('-', 100));
            Console.ResetColor();
        }

        public void UpdateFlightStatus()
        {
            AuthService.CheckAdmin();
            GetAllFlights();
            Console.WriteLine("Enter flight ID:");
            int id = int.Parse(Console.ReadLine()!);
            var flight = DC.Flights.Find(id);
            if (flight == null) throw new Exception("Flight not found.");
            if (flight.Status == FlightStatus.Cancelled) throw new Exception("Cannot update a cancelled flight.");
            if (flight.Status == FlightStatus.Arrived) throw new Exception("Cannot update an arrived flight.");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("New Status  0=Scheduled  1=Boarding  2=Departed  3=Arrived  4=Cancelled:");
            int s = int.Parse(Console.ReadLine()!);
            if (!Enum.IsDefined(typeof(FlightStatus), s)) throw new Exception("Invalid status.");
            Console.ResetColor();

            flight.Status = (FlightStatus)s;

            if (flight.Status == FlightStatus.Departed)
            {
                var captain = DC.FlightAssignments
                    .Include(fa => fa.CrewMember)
                    .FirstOrDefault(fa => fa.FlightId == flight.Id &&
                                          fa.CrewMember.Role == CrewRole.Captain);
                if (captain == null) throw new Exception("Cannot depart: no Captain assigned.");

                DC.FlightManifests.Add(new FlightManifest
                {
                    FlightId = flight.Id,
                    PassengerCount = DC.Tickets.Count(t => t.FlightId == flight.Id && !t.IsCanceled),
                    FuelLoadKg = 15000m,
                    DepartureTimestamp = DateTime.UtcNow,
                    PilotInCommand = captain.CrewMember.Name,
                    Notes = "Auto-generated on departure"
                });
                var evt = new FlightDepartedEvent(flight.Id, flight.FlightNumber);
                LogService.LogInfo($"Flight departed: {flight.FlightNumber}");
            }

            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight status updated successfully.");
            Console.ResetColor();
        }

        public void DeleteFlight()
        {
            AuthService.CheckAdmin();
            GetAllFlights();
            Console.WriteLine("Enter flight ID to delete:");
            int id = int.Parse(Console.ReadLine()!);
            var flight = DC.Flights.Find(id);
            if (flight == null) throw new Exception("Flight not found.");
            if (flight.Status == FlightStatus.Departed) throw new Exception("Cannot delete a departed flight.");
            DC.Flights.Remove(flight);
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight deleted successfully.");
            Console.ResetColor();
        }

        public void ViewFlightManifest()
        {
            AuthService.CheckAdmin();
            GetAllFlights();
            Console.WriteLine("Enter flight ID:");
            int id = int.Parse(Console.ReadLine()!);
            var manifest = DC.FlightManifests.FirstOrDefault(m => m.FlightId == id);
            if (manifest == null) throw new Exception("No manifest found (flight may not have departed).");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Flight Manifest ===");
            Console.WriteLine(manifest);
            Console.ResetColor();
        }

        public void ShowAllAircrafts()
        {
            var list = DC.Aircrafts.ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Aircrafts ===");
            foreach (var a in list) Console.WriteLine(a);
            Console.ResetColor();
        }
    }
}
