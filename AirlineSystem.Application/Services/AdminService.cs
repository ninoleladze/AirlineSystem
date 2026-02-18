using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class AdminService : IAdminService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();

        public void AssignRole()
        {
            AuthService.CheckAdmin();
            var users = DC.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== All Users ===");
            foreach (var u in users)
            {
                string roles = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name));
                Console.WriteLine($"{u} | Roles: {roles}");
            }
            Console.ResetColor();

            Console.WriteLine("Enter user ID:");
            int userId = int.Parse(Console.ReadLine()!);
            if (DC.Users.Find(userId) == null) throw new Exception("User not found.");

            var roleList = DC.Roles.ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Roles ===");
            foreach (var r in roleList) Console.WriteLine(r);
            Console.ResetColor();

            Console.WriteLine("Enter role ID:");
            int roleId = int.Parse(Console.ReadLine()!);
            if (!DC.Roles.Any(r => r.Id == roleId)) throw new Exception("Role not found.");
            if (DC.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == roleId))
                throw new Exception("User already has this role.");

            DC.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Role assigned successfully.");
            Console.ResetColor();
        }

        public void RemoveRole()
        {
            AuthService.CheckAdmin();
            Console.WriteLine("Enter user ID:");
            int userId = int.Parse(Console.ReadLine()!);
            var userRoles = DC.UserRoles.Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId).ToList();
            if (userRoles.Count == 0) throw new Exception("This user has no roles.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var ur in userRoles) Console.WriteLine($"[{ur.RoleId}] {ur.Role.Name}");
            Console.ResetColor();
            Console.WriteLine("Enter role ID to remove:");
            int roleId = int.Parse(Console.ReadLine()!);
            var entry = userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (entry == null) throw new Exception("User does not have that role.");
            DC.UserRoles.Remove(entry);
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Role removed successfully.");
            Console.ResetColor();
        }

        public void ViewAllBookings()
        {
            AuthService.CheckAdmin();
            var tickets = DC.Tickets
                .Include(t => t.Flight).Include(t => t.User).Include(t => t.Payments)
                .OrderByDescending(t => t.CreatedAt).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== All Bookings ===");
            if (tickets.Count == 0) { Console.WriteLine("No bookings found."); Console.ResetColor(); return; }
            foreach (var t in tickets)
            {
                string payStatus = t.Payments.Any(p => p.Status == PaymentStatus.Completed) ? "PAID" : "UNPAID";
                Console.WriteLine($"{t} | User: {t.User.Username} | Flight: {t.Flight?.FlightNumber} | {payStatus}");
            }
            Console.ResetColor();
        }

        public void RevenueReport()
        {
            AuthService.CheckAdmin();
            var flights = DC.Flights.Include(f => f.Tickets).ThenInclude(t => t.Payments).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Revenue Report Per Flight ===");
            Console.WriteLine(new string('-', 60));
            decimal grandTotal = 0;
            foreach (var f in flights)
            {
                decimal revenue = f.Tickets.Where(t => !t.IsCanceled)
                    .Sum(t => t.Payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount));
                int paidCount = f.Tickets.Count(t => !t.IsCanceled &&
                    t.Payments.Any(p => p.Status == PaymentStatus.Completed));
                Console.WriteLine($"{f.FlightNumber} | Paid Tickets: {paidCount} | Revenue: {revenue:F2} USD");
                grandTotal += revenue;
            }
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"GRAND TOTAL: {grandTotal:F2} USD");
            Console.ResetColor();
        }

        public void UpdateMaintenanceRecord()
        {
            AuthService.CheckAdmin();
            var aircrafts = DC.Aircrafts.Include(a => a.MaintenanceRecord).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Aircrafts ===");
            foreach (var a in aircrafts)
            {
                Console.WriteLine(a);
                if (a.MaintenanceRecord != null) Console.WriteLine($"   {a.MaintenanceRecord}");
                else Console.WriteLine("   No maintenance record.");
            }
            Console.ResetColor();

            Console.WriteLine("Enter aircraft ID:");
            int id = int.Parse(Console.ReadLine()!);
            var aircraft = DC.Aircrafts.Include(a => a.MaintenanceRecord).FirstOrDefault(a => a.Id == id);
            if (aircraft == null) throw new Exception("Aircraft not found.");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Last Maintenance Date (yyyy-MM-dd):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime lastDate)) throw new Exception("Invalid date.");
            Console.WriteLine("Next Maintenance Date (yyyy-MM-dd):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime nextDate) || nextDate <= lastDate)
                throw new Exception("Next date must be after last date.");
            Console.WriteLine("Technician Name:");
            string tech = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(tech)) throw new Exception("Technician name cannot be empty.");
            Console.WriteLine("Notes (Enter to skip):");
            string notes = Console.ReadLine()!;
            Console.ResetColor();

            if (aircraft.MaintenanceRecord == null)
            {
                DC.MaintenanceRecords.Add(new MaintenanceRecord
                {
                    AircraftId = aircraft.Id,
                    LastMaintenanceDate = lastDate,
                    NextMaintenanceDate = nextDate,
                    TechnicianName = tech,
                    Notes = notes
                });
            }
            else
            {
                aircraft.MaintenanceRecord.LastMaintenanceDate = lastDate;
                aircraft.MaintenanceRecord.NextMaintenanceDate = nextDate;
                aircraft.MaintenanceRecord.TechnicianName = tech;
                if (!string.IsNullOrWhiteSpace(notes)) aircraft.MaintenanceRecord.Notes = notes;
            }
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Maintenance record updated.");
            Console.ResetColor();
        }
    }
}