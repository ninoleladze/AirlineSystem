using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IPromotionService
    {
        public void CreatePromotion();
        public void ApplyPromoToTicket();
        public void ListPromotions();
        public void DeactivatePromotion();

    }
}
