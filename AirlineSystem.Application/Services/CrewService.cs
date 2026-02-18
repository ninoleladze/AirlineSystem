using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using AirlineSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class CrewService : ICrewService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();
        private FlightService FlightService = new FlightService();

        public void AddCrewMember()
        {
            AuthService.CheckAdmin();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Add Crew Member ===");
            Console.WriteLine("Name:");
            string name = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name cannot be empty.");

            Console.WriteLine("License Number:");
            string license = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(license)) throw new Exception("License cannot be empty.");
            if (DC.CrewMembers.Any(c => c.LicenseNumber == license)) throw new Exception("License already exists.");

            Console.WriteLine("Role  0=Captain  1=CoPilot  2=Purser  3=Steward:");
            int roleInput = int.Parse(Console.ReadLine()!);
            if (!Enum.IsDefined(typeof(CrewRole), roleInput)) throw new Exception("Invalid role.");

            Console.WriteLine("Linked User ID (0 to skip):");
            int userId = int.Parse(Console.ReadLine()!);
            if (userId != 0 && DC.Users.Find(userId) == null) throw new Exception("User not found.");
            Console.ResetColor();

            DC.CrewMembers.Add(new CrewMember
            {
                Name = name,
                LicenseNumber = license,
                Role = (CrewRole)roleInput,
                UserId = userId != 0 ? userId : 1,
                IsAvailable = true 
            });
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Crew member added successfully.");
            Console.ResetColor();
        }

        public void AssignCrewToFlight()
        {
            AuthService.CheckAdmin();
            FlightService.GetAllFlights();
            Console.WriteLine("Enter flight ID:");
            int flightId = int.Parse(Console.ReadLine()!);
            var flight = DC.Flights.Find(flightId);
            if (flight == null) throw new Exception("Flight not found.");
            if (flight.Status == FlightStatus.Departed || flight.Status == FlightStatus.Cancelled)
                throw new Exception("Cannot assign crew to a departed or cancelled flight.");

            GetAllCrewMembers();
            Console.WriteLine("Enter crew member ID:");
            int crewId = int.Parse(Console.ReadLine()!);

            var crew = DC.CrewMembers.Include(c => c.User).FirstOrDefault(c => c.Id == crewId);
            if (crew == null) throw new Exception("Crew member not found.");
            if (DC.FlightAssignments.Any(fa => fa.FlightId == flightId && fa.CrewMemberId == crewId))
                throw new Exception("Crew member already assigned to this flight.");

            var lastFlight = DC.FlightAssignments
                .Include(fa => fa.Flight)
                .Where(fa => fa.CrewMemberId == crewId)
                .OrderByDescending(fa => fa.Flight.ArrivalTime)
                .FirstOrDefault();
            if (lastFlight != null)
            {
                double gap = (flight.DepartureTime - lastFlight.Flight.ArrivalTime).TotalHours;
                if (gap < 10)
                    throw new Exception($"Insufficient rest. {gap:F1}h gap (minimum 10h required).");
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Duty Hours:");
            double dutyHours = double.Parse(Console.ReadLine()!);
            if (dutyHours <= 0 || dutyHours > 14) throw new Exception("Duty hours must be between 1 and 14.");
            Console.WriteLine("Notes (Enter to skip):");
            string notes = Console.ReadLine()!;
            Console.ResetColor();

            DC.FlightAssignments.Add(new FlightAssignment
            {
                FlightId = flightId,
                CrewMemberId = crewId,
                DutyHours = dutyHours,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
            });

            crew.IsAvailable = false;
            DC.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Crew member {crew.Name} (User: {crew.User.Username}) assigned successfully and marked as unavailable.");
            Console.ResetColor();
        }
        public void RemoveCrewFromFlight()
        {
            AuthService.CheckAdmin();
            FlightService.GetAllFlights();
            Console.WriteLine("Enter flight ID:");
            int flightId = int.Parse(Console.ReadLine()!);
            var flight = DC.Flights.Find(flightId);
            if (flight == null) throw new Exception("Flight not found.");
            if (flight.Status != FlightStatus.Scheduled && flight.Status != FlightStatus.Boarding)
                throw new Exception("Can only remove crew from Scheduled or Boarding flights.");

            ShowCrewForFlight(flightId);
            Console.WriteLine("Enter assignment ID to remove:");
            int assignmentId = int.Parse(Console.ReadLine()!);
            var assignment = DC.FlightAssignments
                .Include(fa => fa.CrewMember)
                .FirstOrDefault(fa => fa.Id == assignmentId && fa.FlightId == flightId);
            if (assignment == null) throw new Exception("Assignment not found for that flight.");

            var crew = assignment.CrewMember;
            DC.FlightAssignments.Remove(assignment);

         
            var hasOtherAssignments = DC.FlightAssignments
                .Include(fa => fa.Flight)
                .Any(fa => fa.CrewMemberId == crew.Id &&
                           fa.FlightId != flightId &&
                           (fa.Flight.Status == FlightStatus.Scheduled ||
                            fa.Flight.Status == FlightStatus.Boarding));

            if (!hasOtherAssignments)
            {
                crew.IsAvailable = true;
            }

            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(hasOtherAssignments
                ? "Crew removed from flight (still assigned to other flights)."
                : "Crew removed from flight and marked as available.");
            Console.ResetColor();
        }

        public void GetAllCrewMembers()
        {
            AuthService.CheckLoggedIn();
            var list = DC.CrewMembers.Include(c => c.User).ToList();  
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Crew Members ===");
            foreach (var c in list) Console.WriteLine(c);
            Console.ResetColor();
        }

        public void ShowCrewForFlight(int flightId = 0)
        {
            AuthService.CheckLoggedIn();
            if (flightId == 0)
            {
                FlightService.GetAllFlights();
                Console.WriteLine("Enter flight ID:");
                flightId = int.Parse(Console.ReadLine()!);
            }
            var assignments = DC.FlightAssignments
                .Include(fa => fa.CrewMember)
                .Where(fa => fa.FlightId == flightId).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== Crew for Flight {flightId} ===");
            if (assignments.Count == 0) Console.WriteLine("No crew assigned.");
            foreach (var a in assignments) Console.WriteLine(a);
            Console.ResetColor();
        }
    }
}