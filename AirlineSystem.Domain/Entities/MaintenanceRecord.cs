using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class MaintenanceRecord : BaseEntity
    {
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }= null!;
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime  NextMaintenanceDate { get; set; }

        public string TechnicianName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public override string ToString() => $"Last: {LastMaintenanceDate:d} | Next: {NextMaintenanceDate:d} | Tech: {TechnicianName}";



    }
}
