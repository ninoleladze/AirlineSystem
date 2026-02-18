using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IUserService
    {
        public void GetAllUsers();
        public void ViewMyProfile();
        public void UpdateMyProfile();
        public void ViewMyBalance();
        public void AddMoney();
        public void BanUser();

    }

}
