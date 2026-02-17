using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class PaymentService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();

        public void PayForTicket()
        {
            AuthService.CheckLoggedIn();

            var unpaid = DC.Tickets
                .Include(t => t.Flight)
                .Include(t => t.Payments)
                .Where(t => t.UserId == AuthService.LoggedInUser!.Id && !t.IsCanceled)
                .ToList()
                .Where(t => !t.Payments.Any(p => p.Status == PaymentStatus.Completed))
                .ToList();

            if (unpaid.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No unpaid tickets found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Unpaid Tickets ===");
            foreach (var t in unpaid) Console.WriteLine($"{t} | Flight: {t.Flight?.FlightNumber}");
            Console.ResetColor();

            Console.WriteLine("Enter ticket ID to pay:");
            int ticketId = int.Parse(Console.ReadLine()!);
            var ticket = unpaid.FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket not found or already paid.");

            // ← CHECK USER BALANCE
            var user = DC.Users.Find(AuthService.LoggedInUser!.Id);
            if (user!.Balance < ticket.PriceAmount)
                throw new Exception($"Insufficient funds. Your balance: {user.Balance} {user.Currency} | Price: {ticket.PriceAmount} {ticket.PriceCurrency}");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Price: {ticket.PriceAmount} {ticket.PriceCurrency}");
            Console.WriteLine($"Your Balance: {user.Balance} {user.Currency}");
            Console.WriteLine("Payment Method  0=Cash  1=CreditCard  2=DebitCard  3=Online:");
            int method = int.Parse(Console.ReadLine()!);
            if (!Enum.IsDefined(typeof(PaymentMethod), method)) throw new Exception("Invalid payment method.");
            Console.ResetColor();

            // ← DEDUCT FROM BALANCE
            user.Balance -= ticket.PriceAmount;

            var payment = new Payment
            {
                TicketId = ticket.Id,
                Amount = ticket.PriceAmount,
                Currency = ticket.PriceCurrency,
                Method = (PaymentMethod)method,
                Status = PaymentStatus.Completed,
                PaidAt = DateTime.UtcNow,
                TransactionId = $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            };
            DC.Payments.Add(payment);
            DC.SaveChanges();

            LogService.LogInfo($"Payment completed: {payment.TransactionId} | User: {user.Username} | New balance: {user.Balance}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Payment successful! Transaction: {payment.TransactionId}");
            Console.WriteLine($"Remaining Balance: {user.Balance} {user.Currency}");
            Console.ResetColor();
        }

        public void RefundPayment()
        {
            AuthService.CheckLoggedIn();
            var paid = DC.Payments
                .Include(p => p.Ticket).ThenInclude(t => t.Flight)
                .Where(p => p.Ticket.UserId == AuthService.LoggedInUser!.Id &&
                             p.Status == PaymentStatus.Completed).ToList();

            if (paid.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No completed payments to refund.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Your Payments ===");
            foreach (var pay in paid) Console.WriteLine(pay);
            Console.ResetColor();

            Console.WriteLine("Enter payment ID to refund:");
            int paymentId = int.Parse(Console.ReadLine()!);
            var payment = paid.FirstOrDefault(p => p.Id == paymentId);
            if (payment == null) throw new Exception("Payment not found.");
            if (payment.Ticket.Flight?.Status == FlightStatus.Departed)
                throw new Exception("Cannot refund a payment for a departed flight.");

            // ← REFUND TO BALANCE
            var user = DC.Users.Find(AuthService.LoggedInUser!.Id);
            user!.Balance += payment.Amount;

            payment.Status = PaymentStatus.Refunded;
            payment.Notes = $"Refunded at {DateTime.UtcNow:g}";
            DC.SaveChanges();

            LogService.LogInfo($"Refund processed: {payment.TransactionId} | User: {user.Username} | New balance: {user.Balance}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Refund processed. Amount: {payment.Amount} {payment.Currency}");
            Console.WriteLine($"New Balance: {user.Balance} {user.Currency}");
            Console.ResetColor();
        }

        public void ViewMyPayments()
        {
            AuthService.CheckLoggedIn();
            var payments = DC.Payments
                .Include(p => p.Ticket)
                .Where(p => p.Ticket.UserId == AuthService.LoggedInUser!.Id)
                .OrderByDescending(p => p.CreatedAt).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== My Payments ===");
            if (payments.Count == 0) { Console.WriteLine("No payments found."); Console.ResetColor(); return; }
            foreach (var pay in payments) Console.WriteLine(pay);
            Console.ResetColor();
        }
    }
}