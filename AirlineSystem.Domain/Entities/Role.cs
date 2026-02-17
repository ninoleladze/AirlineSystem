using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Nav prop many-to-many with User
        public List<UserRole> UserRoles { get; set; } = new ();
        public override string ToString() => $"{Id} - {Name}"; 
    }
}
