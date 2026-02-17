using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Events
{
 
    public class TicketBookedEvent
    {
        public int TicketId { get; set; }
        public int FlightId { get; set; }
        public string PassengerName { get; set; }
        public DateTime BookedAt { get; set; }

        public TicketBookedEvent(int ticketId, int flightId, string passengerName)
        {
            TicketId = ticketId;
            FlightId = flightId;
            PassengerName = passengerName;
            BookedAt = DateTime.UtcNow;
        }

    }
}
