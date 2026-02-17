using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Promotion : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPercent { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int MaxUses { get; set; }
        public int CurrentUses { get; set; }

        //many to many tickets
        public List<TicketPromotion> TicketPromotions { get; set; } = new();

        public bool IsValid() =>
            IsActive &&
            ExpiryDate >= DateTime.Now &&
            (MaxUses == 0 || CurrentUses < MaxUses);

        public override string ToString() =>
            $"[{Id}] {Code} | {DiscountPercent}% off | Expires: {ExpiryDate:d}" +
            $"| Uses: {CurrentUses}/{(MaxUses == 0 ? "unlimited" : MaxUses.ToString())} " +
            $"| Active: {IsActive}";


    }
}
