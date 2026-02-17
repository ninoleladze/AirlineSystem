using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.ValueObjects
{
    public class Money
    {

        public Decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public Money() { }
        public Money(decimal amount,string currency = "USD")
        {
            Amount = amount;
            Currency = currency;
        }
        public override string ToString() => $"{Amount:F2} {Currency}";
    }
}
