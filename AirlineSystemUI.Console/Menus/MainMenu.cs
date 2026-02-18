using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class MainMenu : IMenu
    {
        private AuthMenu AuthMenu = new AuthMenu();
        private FlightMenu FlightMenu = new FlightMenu();
        private CrewMenu CrewMenu = new CrewMenu();
        private BookingMenu BookingMenu = new BookingMenu();
        private PaymentMenu PaymentMenu = new PaymentMenu();
        private SearchMenu SearchMenu = new SearchMenu();
        private AdminMenu AdminMenu = new AdminMenu();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("AirlineSystem - Main Menu");
                Console.WriteLine("1. Authentication");
                Console.WriteLine("2. Flights");
                Console.WriteLine("3. Crew");
                Console.WriteLine("4. Booking");
                Console.WriteLine("5. Payments & Promotions");
                Console.WriteLine("6. Search & Filter");
                Console.WriteLine("7. Admin Panel            [Admin]");
                Console.WriteLine("0. Exit");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": AuthMenu.Show(); break;
                        case "2": FlightMenu.Show(); break;
                        case "3": CrewMenu.Show(); break;
                        case "4": BookingMenu.Show(); break;
                        case "5": PaymentMenu.Show(); break;
                        case "6": SearchMenu.Show(); break;
                        case "7": AdminMenu.Show(); break;
                        case "0": ConsoleWriter.PrintInfo("Goodbye!"); return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}