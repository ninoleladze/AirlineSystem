using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;

namespace AirlineSystem.Domain.Entities
{
    public class CrewMember : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public CrewRole Role { get; set; }
        public bool IsAvailable { get; set; } = true;

        // One-to-One: User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // One-to-Many: FlightAssignments
        public List<FlightAssignment> FlightAssignments { get; set; } = new();

        public override string ToString() =>
            $"[{Id}] {Name} | Role: {Role} | License: {LicenseNumber} | Available: {IsAvailable}" +
            $" | User: {User?.Username ?? "N/A"}";
    }
}