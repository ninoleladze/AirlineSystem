using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class FlightAssignment : BaseEntity
    {
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;
        public int CrewMemberId { get; set; }
        public CrewMember CrewMember { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public double DutyHours { get; set; }
        public string? Notes { get; set; }
         public override string ToString()=> $"[{Id}]|Flight {FlightId} | Crew: {CrewMember?.Name} ({CrewMember?.Role}) | " +
             $"Assigned At: {AssignedAt:g} | Duty Hours: {DutyHours}h | Notes: {Notes}";

    }
}
