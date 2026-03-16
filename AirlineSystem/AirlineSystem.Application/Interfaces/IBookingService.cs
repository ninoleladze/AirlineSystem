using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IBookingService
    {

        public void BookTicket();
        public void CancelTicket();
        public void ShowMyTickets();
        public void SearchByReference();
    }
}
