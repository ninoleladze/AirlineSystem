using AirlineSystem.AirlineSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class CrewMember : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public CrewRole Role { get; set; }
        public bool IsAvailable { get; set; } = true;

        //one to one to user 
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        //many to many relationship with flights via flight assignment
        public List<FlightAssignment> FlightAssignments { get; set; } = new List<FlightAssignment>();
    }
}
