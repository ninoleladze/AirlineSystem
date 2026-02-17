using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class FlightManifest : BaseEntity
    {
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;

        public int PassengerCount { get; set; }
        public decimal FuelLoadKg { get; set; }
        public DateTime DepartureTimestamp { get; set; }
        public string PilotInCommand { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
       
        public override string ToString()=> $"Flight {FlightId} | Pax: {PassengerCount} | Fuel : {FuelLoadKg}kg"+
            $" | Departed: {DepartureTimestamp:g} | Pilot: {PilotInCommand}";

    }
}
