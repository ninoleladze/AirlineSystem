using AirlineSystem.AirlineSystem.Application.Services;
using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.Application.Services
{
    internal class PromotionService
    {
        private AirlineDbContext DC = new AirlineDbContext();
        private AuthService AuthService = new AuthService();

        public void CreatePromotion()
        {
            AuthService.CheckAdmin();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Create Promotion ===");
            Console.WriteLine("Promo Code:");
            string code = Console.ReadLine()!.ToUpper();
            if (string.IsNullOrWhiteSpace(code)) throw new Exception("Code cannot be empty.");
            if (DC.Promotions.Any(pr => pr.Code == code)) throw new Exception("Promo code already exists.");

            Console.WriteLine("Description:");
            string desc = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(desc)) throw new Exception("Description cannot be empty.");

            Console.WriteLine("Discount Percent (e.g. 20 for 20% off):");
            if (!decimal.TryParse(Console.ReadLine(), out decimal discount) || discount <= 0 || discount > 100)
                throw new Exception("Discount must be between 1 and 100.");

            Console.WriteLine("Expiry Date (yyyy-MM-dd):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime expiry) || expiry <= DateTime.Now)
                throw new Exception("Expiry must be a future date.");

            Console.WriteLine("Max Uses (0 = unlimited):");
            int maxUses = int.Parse(Console.ReadLine()!);
            if (maxUses < 0) throw new Exception("Max uses cannot be negative.");
            Console.ResetColor();

            DC.Promotions.Add(new Promotion
            {
                Code = code,
                Description = desc,
                DiscountPercent = discount,
                ExpiryDate = expiry,
                MaxUses = maxUses,
                IsActive = true
            });
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Promotion created successfully.");
            Console.ResetColor();
        }

        public void ApplyPromoToTicket()
        {
            AuthService.CheckLoggedIn();
            Console.WriteLine("Enter promo code:");
            string code = Console.ReadLine()!.ToUpper();
            var promo = DC.Promotions.FirstOrDefault(pr => pr.Code == code);
            if (promo == null) throw new Exception("Promo code not found.");
            if (!promo.IsValid()) throw new Exception("Promo code is expired, inactive, or fully used.");

            Console.WriteLine("Enter ticket ID to apply promo to:");
            int ticketId = int.Parse(Console.ReadLine()!);
            var ticket = DC.Tickets.Include(t => t.TicketPromotions).FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket not found.");
            if (ticket.UserId != AuthService.LoggedInUser!.Id) throw new Exception("Not your ticket.");
            if (ticket.IsCanceled) throw new Exception("Cannot apply promo to a cancelled ticket.");
            if (ticket.TicketPromotions.Any(tp => tp.PromotionId == promo.Id))
                throw new Exception("Promo already applied to this ticket.");

            decimal savings = Math.Round(ticket.PriceAmount * (promo.DiscountPercent / 100m), 2);
            ticket.PriceAmount -= savings;
            promo.CurrentUses++;

            DC.TicketPromotions.Add(new TicketPromotion
            {
                TicketId = ticket.Id,
                PromotionId = promo.Id,
                Savings = savings
            });
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Promo applied! You saved {savings} {ticket.PriceCurrency}. New price: {ticket.PriceAmount} {ticket.PriceCurrency}");
            Console.ResetColor();
        }

        public void ListPromotions()
        {
            AuthService.CheckLoggedIn();
            var promos = DC.Promotions.ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Active Promotions ===");
            foreach (var pr in promos.Where(pr => pr.IsValid())) Console.WriteLine(pr);
            Console.ResetColor();
        }

        public void DeactivatePromotion()
        {
            AuthService.CheckAdmin();
            ListPromotions();
            Console.WriteLine("Enter promo ID to deactivate:");
            int id = int.Parse(Console.ReadLine()!);
            var promo = DC.Promotions.Find(id);
            if (promo == null) throw new Exception("Promotion not found.");
            promo.IsActive = false;
            DC.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Promotion deactivated.");
            Console.ResetColor();
        }
    }
}
