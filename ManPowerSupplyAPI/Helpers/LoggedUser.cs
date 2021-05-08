using ManPowerSupplyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Helpers
{
    public sealed class LoggedUser
    {
        private static LoggedUser _instance = null;

        public static LoggedUser Instance
        {
            get {
                if (_instance == null)
                    _instance = new LoggedUser();
                return _instance;
            }
        }

        private LoggedUser()
        {

        }


        public UserResponseModel CurrentUser { get; set; }

        public long CurrentOrganization { get; set; }
    }
}
