using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Models
{
    public class ValidatePasswordRequestModel
    {
        public long UserId { get; set; }

        public string Password { get; set; }
    }

    public class UpdatePasswordRequestModel
    {
        public long UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
