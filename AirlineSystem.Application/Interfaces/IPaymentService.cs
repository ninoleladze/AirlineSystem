using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IPaymentService
    {
        public void PayForTicket();
        public void RefundPayment();
        public void ViewMyPayments();

    }
}
