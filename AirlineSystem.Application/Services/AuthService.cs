using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Events;
using AirlineSystem.AirlineSystem.Domain.ValueObjects;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Services
{
    internal class AuthService : IAuthService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        public static User? LoggedInUser { get; private set; }

        public void Register()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Register ===");
            Console.WriteLine("Username:");
            string username = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username cannot be empty.");
            if (DC.Users.Any(u => u.Username == username)) throw new Exception("Username already taken.");

            Console.WriteLine("Email:");
            string email = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Email cannot be empty.");
            if (!email.Contains("@") || !email.Contains(".")) throw new Exception("Email is not valid.");
            if (DC.Users.Any(u => u.Email == email)) throw new Exception("Email already registered.");

            Console.WriteLine("Password (min 6 chars):");
            string password = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be empty.");
            if (password.Length < 6) throw new Exception("Password must be atleast 6 chars");

            Console.ResetColor();

            var user = new User
            {
                Username = username,
                Email = email,
                Password = PasswordHash.Hash(password),
                Status= Domain.Enums.UserStatus.Active
            };
            DC.Users.Add(user);
            DC.SaveChanges();

           var role = DC.Roles.FirstOrDefault(r=> r.Name == "User");
            if (role != null)
            {
                DC.UserRoles.Add(new UserRole{UserId = user.Id, RoleId = role.Id});
                DC.SaveChanges() ;

            }


            var evt = new UserRegisteredEvent(user.Id,user.Username,user.Email);
            LogService.LogInfo($"User registered: {user.Username}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Registration successful! Welcome, {user.Username}.");
            Console.ResetColor();





        }

        public void Login()
        {
            if (LoggedInUser != null) throw new Exception("Already logged in. Please logout first.");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Login ===");
            Console.WriteLine("Email:");
            string email = Console.ReadLine()!;

            Console.WriteLine("Password:");
            string password = Console.ReadLine()!;
            Console.ResetColor();
            string hashed = PasswordHash.Hash(password);

            var user = DC.Users.FirstOrDefault(u=>u.Email == email&& u.Password == hashed);

            if (user == null) throw new Exception("Invalid email or password.");
            if (user.Status == Domain.Enums.UserStatus.Banned) throw new Exception("Your account has been banned.");

            LoggedInUser = user;
            LogService.LogInfo($"User logged in: {user.Username}");
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine($"Login successful! Welcome back, {LoggedInUser.Username}");
            Console.ResetColor();



        }
        public void Logout()
        {
            if (LoggedInUser == null) throw new Exception("No user is currently logged in.");
            LogService.LogInfo($"User logged out: {LoggedInUser.Username}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Goodbye, {LoggedInUser.Username}!");
            Console.ResetColor();
            LoggedInUser = null;
        }
        public void CheckLoggedIn()
        {
            if(LoggedInUser == null) throw new Exception("You must be logged in to do this.");

        }
        public void CheckAdmin()
        {
            CheckLoggedIn();
            bool isAdmin = DC.UserRoles
                .Any(ur => ur.UserId == LoggedInUser!.Id && ur.Role.Name == "Admin");
            if (!isAdmin) throw new Exception("Admin access required.");
        }
    }
}
