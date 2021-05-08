using ManPowerSupplyAPI.DatabaseManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Models
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EMail { get; set; }

        public DateTime? LastLoggedIn { get; set; }
        public string Token { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public List<UserOrganization> Organizations { get; set; }

        public AuthenticateResponse(UserEntity user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            MobileNumber = user.MobileNumber;
            EMail = user.EMail;
            LastLoggedIn = user.LastLoggedIn;
            Organizations = (from orgMap in user.UserOrganizationMappings
                             select new UserOrganization
                             {
                                 Id = orgMap.OrganizationId,
                                 Name = orgMap.Organization.Name,
                                 Address = orgMap.Organization.Address
                             }).ToList();

            Token = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
