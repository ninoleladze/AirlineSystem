using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Events
{
    public class FlightDepartedEvent
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartedAt { get; set; }

        public FlightDepartedEvent(int flightId, string flightNumber )
        {
            FlightId = flightId;
            FlightNumber = flightNumber;
            DepartedAt = DateTime.UtcNow;
        }

    }
    
        
    
}
