using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface ICrewService
    {
        public void AddCrewMember();
        public void AssignCrewToFlight();
        public void RemoveCrewFromFlight();
        public void GetAllCrewMembers();
        public void ShowCrewForFlight(int flightId = 0);
    }
}
