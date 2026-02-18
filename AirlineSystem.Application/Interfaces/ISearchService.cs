using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface ISearchService
    {
        public void SearchFlights();
        public void FilterTicketsByDate();
        public void SearchCrew();
        public void SearchAllTickets();
    }
}
