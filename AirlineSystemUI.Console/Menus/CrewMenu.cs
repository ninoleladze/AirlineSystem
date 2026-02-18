using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class CrewMenu : IMenu
    {
        private CrewService CrewService = new CrewService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Crew Management");
                Console.WriteLine("1. List All Crew Members");
                Console.WriteLine("2. Add Crew Member          [Admin]");
                Console.WriteLine("3. Assign Crew to Flight    [Admin]");
                Console.WriteLine("4. Remove Crew from Flight  [Admin]");
                Console.WriteLine("5. Show Crew for a Flight");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": CrewService.GetAllCrewMembers(); break;
                        case "2": CrewService.AddCrewMember(); break;
                        case "3": CrewService.AssignCrewToFlight(); break;
                        case "4": CrewService.RemoveCrewFromFlight(); break;
                        case "5": CrewService.ShowCrewForFlight(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}