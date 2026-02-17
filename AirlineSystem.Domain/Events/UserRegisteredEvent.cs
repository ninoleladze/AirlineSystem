using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Events
{
  
    public class UserRegisteredEvent
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public UserRegisteredEvent(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
            RegisteredAt = DateTime.UtcNow;
        }
    }
}
