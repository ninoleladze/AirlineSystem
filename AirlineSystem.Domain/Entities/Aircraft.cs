using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Aircraft : BaseEntity
    {
        public string Model { get; set; } = string.Empty;
        public string TailNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public int YearManufactured { get; set; }

        //one to one relationship with maintenance record
        public MaintenanceRecord? MaintenanceRecord { get; set; }

        //one to many relationship with flights
        public List<Flight> Flights { get; set; } = new List<Flight>();

        public override string ToString()=> $"[{Id}] {Model} | Tail: {TailNumber} | Capacity: {Capacity} | Year: {YearManufactured})";


    }
}
