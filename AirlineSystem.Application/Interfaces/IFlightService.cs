using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IFlightService
    {

        public void CreateFlight();
        public void GetAllFlights();
        public void UpdateFlightStatus();
        public void DeleteFlight();
        public void ViewFlightManifest();
        public void ShowAllAircrafts();
    }
}
