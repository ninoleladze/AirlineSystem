using AirlineSystem.AirlineSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }= PaymentStatus.Pending;
        public DateTime? PaidAt { get; set; }
        public string? TransactionId { get; set; }
        public string? Notes { get; set; }

        public override string ToString() =>
            $"[{Id}] Ticket {TicketId} | {Amount} {Currency}" +
            $"| {Method} | {Status} | Paid: {PaidAt?.ToString("g") ?? "not yet"}";
      
    }
}
