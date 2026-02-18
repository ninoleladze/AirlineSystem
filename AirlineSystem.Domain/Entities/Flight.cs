using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public string FlightNumber { get; set; } = string.Empty;
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; } = FlightStatus.Scheduled;
        public int AvailableSeats { get; set; }

        public decimal BasePrice { get; set; }
        public string PriceCurrency { get; set; } = "USD";

        // coordinates for departure and arrival airportsssss
        public Coordinate? DepartureCoordinate { get; set; }
        public Coordinate? ArrivalCoordinate { get; set; }

        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; } = null!;


        //one to one relationship with fliight manifest
        public FlightManifest? FlightManifest { get; set; }

        //one to many relationshipp with tickets
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

        //many to many relationship with crew members via flightassigment

        public List<FlightAssignment> FlightAssignments { get; set; } = new List<FlightAssignment>();
        public override string ToString() =>
           $"[{Id}] {FlightNumber} | {DepartureAirport} -> {ArrivalAirport}" +
           $" | {DepartureTime:g} | PRICE: {BasePrice} {PriceCurrency}" +
           $" | Status: {Status} | Seats: {AvailableSeats}";





    }
}
