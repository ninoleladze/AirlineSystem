using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Application.Interfaces
{
    internal interface IAuthService
    {
        public void Register();
        public void Login();
        public void Logout();
        public void CheckLoggedIn();
        public void CheckAdmin();
    }
}
