using AirlineSystem.AirlineSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public decimal Balance { get; set; } = 1000m;  // starting balance
        public string Currency { get; set; } = "USD";
        //one-to-one relationship with UserProfile
        public UserProfile? UserProfile { get; set; }
        //one-to-many relationship with Ticket
        public List<Ticket> Tickets { get; set; } = new ();

        //many-to-many relationship with Role through UserRole
        public List<UserRole> UserRoles { get; set; } = new ();
        public override string ToString() => $"[{Id}] User: {Username} | {Email} | Status: {Status}";

    }
}
