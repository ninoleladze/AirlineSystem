using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public string BookingReference { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public bool IsCanceled { get; set; } = false;

        //money value object for price
        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; } = "USD";

        //many to one relationship with flight
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;
        //one to one relationship with user
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        //one to many payment
        public List<Payment> Payments { get; set; } = new();
        
        //many to many Promotions  {via TicketPromotion}
        public List<TicketPromotion> TicketPromotions { get; set; } = new();


        public override string ToString() => $"[{Id}] Ref: {BookingReference} | Seat: {SeatNumber}" +
            $"| {PassengerName} | {PriceAmount} {PriceCurrency} | Cancelled:" +
            $"{IsCanceled}";

    }
}
