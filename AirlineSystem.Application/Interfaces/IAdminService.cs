using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IAdminService
    {

        public void AssignRole();
        public void RemoveRole();
        public void ViewAllBookings();
        public void RevenueReport();
        public void UpdateMaintenanceRecord();

    }
}
