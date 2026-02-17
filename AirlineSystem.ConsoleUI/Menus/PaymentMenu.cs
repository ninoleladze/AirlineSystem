using AirlineSystem.Application.Services;
using AirlineSystem.ConsoleUI.Utilities;

namespace AirlineSystem.ConsoleUI.Menus
{
    internal class PaymentMenu : IMenu
    {
        private PaymentService PaymentService = new PaymentService();
        private PromotionService PromotionService = new PromotionService();

        public void Show()
        {
            while (true)
            {
                ConsoleWriter.PrintTitle("Payments & Promotions");
                Console.WriteLine("1. Pay for a Ticket");
                Console.WriteLine("2. Refund a Payment");
                Console.WriteLine("3. My Payment History");
                Console.WriteLine("4. View Active Promotions");
                Console.WriteLine("5. Apply Promo Code to Ticket");
                Console.WriteLine("6. Create Promotion        [Admin]");
                Console.WriteLine("7. Deactivate Promotion    [Admin]");
                Console.WriteLine("0. Back");
                ConsoleWriter.PrintSeparator();
                Console.Write("Select: ");
                string choice = Console.ReadLine()!;
                try
                {
                    switch (choice)
                    {
                        case "1": PaymentService.PayForTicket(); break;
                        case "2": PaymentService.RefundPayment(); break;
                        case "3": PaymentService.ViewMyPayments(); break;
                        case "4": PromotionService.ListPromotions(); break;
                        case "5": PromotionService.ApplyPromoToTicket(); break;
                        case "6": PromotionService.CreatePromotion(); break;
                        case "7": PromotionService.DeactivatePromotion(); break;
                        case "0": return;
                        default: ConsoleWriter.PrintError("Invalid option."); break;
                    }
                }
                catch (Exception ex) { ConsoleWriter.PrintError(ex.Message); }
            }
        }
    }
}
