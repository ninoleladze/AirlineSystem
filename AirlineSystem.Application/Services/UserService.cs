using AirlineSystem.AirlineSystem.Application.Interfaces;
using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class UserService : IUserService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();

        public void GetAllUsers()
        {
            AuthService.CheckAdmin();
            var users = DC.Users.Include(u => u.UserProfile).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== All Users ===");
            foreach (var u in users) Console.WriteLine(u);
            Console.ResetColor();
        }

        public void ViewMyProfile()
        {
            AuthService.CheckLoggedIn();
            var profile = DC.UserProfiles
                .FirstOrDefault(p => p.UserId == AuthService.LoggedInUser!.Id);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"User: {AuthService.LoggedInUser}");
            Console.WriteLine(profile != null ? $"Profile: {profile}" : "No profile set up yet.");
            Console.ResetColor();
        }

        public void UpdateMyProfile()
        {
            AuthService.CheckLoggedIn();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Update Profile ===");
            Console.WriteLine("First Name:"); string firstName = Console.ReadLine()!;
            Console.WriteLine("Last Name:"); string lastName = Console.ReadLine()!;
            Console.WriteLine("Phone Number:"); string phone = Console.ReadLine()!;
            Console.WriteLine("Passport Number:"); string passport = Console.ReadLine()!;
            Console.WriteLine("Nationality:"); string nationality = Console.ReadLine()!;
            Console.ResetColor();

            var profile = DC.UserProfiles
                .FirstOrDefault(p => p.UserId == AuthService.LoggedInUser!.Id);
            if (profile == null)
            {
                profile = new UserProfile { UserId = AuthService.LoggedInUser!.Id };
                DC.UserProfiles.Add(profile);
            }

            if (!string.IsNullOrWhiteSpace(firstName)) profile.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) profile.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phone)) profile.PhoneNumber = phone;
            if (!string.IsNullOrWhiteSpace(passport)) profile.PassportNumber = passport;
            if (!string.IsNullOrWhiteSpace(nationality)) profile.Nationality = nationality;

            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Profile updated successfully.");
            Console.ResetColor();
        }
        public void ViewMyBalance()
        {
            AuthService.CheckLoggedIn();
            var user = DC.Users.Find(AuthService.LoggedInUser!.Id);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== My Balance ===");
            Console.WriteLine($"{user!.Balance} {user.Currency}");
            Console.ResetColor();
        }

        public void AddMoney()
        {
            AuthService.CheckLoggedIn();
            var user = DC.Users.Find(AuthService.LoggedInUser!.Id);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Add Money to Wallet ===");
            Console.WriteLine($"Current Balance: {user!.Balance} {user.Currency}");
            Console.WriteLine("Amount to add:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                throw new Exception("Invalid amount.");
            Console.ResetColor();

            user.Balance += amount;
            DC.SaveChanges();

            LogService.LogInfo($"User {user.Username} added {amount} {user.Currency} to wallet");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Money added! New balance: {user.Balance} {user.Currency}");
            Console.ResetColor();
        }
        public void BanUser()
        {
            AuthService.CheckAdmin();
            GetAllUsers();
            Console.WriteLine("Enter user ID to ban:");
            int id = int.Parse(Console.ReadLine()!);
            var user = DC.Users.Find(id);
            if (user == null) throw new Exception("User not found.");
            if (user.Id == AuthService.LoggedInUser!.Id) throw new Exception("You cannot ban yourself.");
            user.Status = UserStatus.Banned;
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User banned successfully.");
            Console.ResetColor();
        }
    }
}
