using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class TicketPromotion 
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; } = null!;

        public decimal Savings { get; set; }
        public DateTime AppliedAt { get; set; }= DateTime.UtcNow;
    }
}
