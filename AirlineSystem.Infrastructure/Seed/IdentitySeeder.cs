using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.ValueObjects;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Infrastructure.Seed
{
    internal class IdentitySeeder
    {
        private AirlineDbContext DC = new AirlineDbContext();

        public void Seed()
        {
            if (DC.Roles.Any()) return;

            var adminRole = new Role { Name = "Admin", Description = " Full system access " };

            var userRole = new Role { Name = "User", Description = " Limited access to own data " };

            var crewRole = new Role { Name = "Crew", Description = " Access to flight and schedule management " };

            DC.Roles.AddRange(adminRole, userRole, crewRole);

            var admin = new User
            {
                Username = "admin",
                Email = "admin@airline.com",
                Password = PasswordHash.Hash("Admin123"),
                Status = Domain.Enums.UserStatus.Active,
            };
            DC.Users.AddRange(admin);
            DC.SaveChanges();

            DC.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
            DC.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Identity seeding completed successfully.");
            Console.ResetColor();

        }
    }
}
