using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Infrastructure.Seed;
using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Menus;
using AirlineSystem.Infrastructure.Seed;
namespace AirlineSystem.ConsoleUI
{
    internal class AppHost
    {
        public void Run()
        {
            LogService.LogInfo("Application started.");
            new IdentitySeeder().Seed();
            new DataSeeder().Seed();
            new MainMenu().Show();
            LogService.LogInfo("Application exited cleanly.");
        }
    }
}
