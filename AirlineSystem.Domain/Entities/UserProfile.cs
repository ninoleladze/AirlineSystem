using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;

        public override string ToString() => $"{FirstName}{LastName} | Phone: {PhoneNumber} | Passport: {PassportNumber}";
    }
}
