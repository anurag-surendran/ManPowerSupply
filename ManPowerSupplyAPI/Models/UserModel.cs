using ManPowerSupplyAPI.DatabaseManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Models
{
    public class UserResponseModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EMail { get; set; }

        public string AccountStatus { get; set; }

        public List<UserOrganization> Organizations { get; set; }

        public string Token { get; set; }

        
        public List<UserRole> Roles { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public UserResponseModel(UserEntity userEntity, string jwtToken = null, string refreshToken = null)
        {
            Id = userEntity.Id;
            FirstName = userEntity.FirstName;
            LastName = userEntity.LastName;
            UserName = userEntity.UserName;
            MobileNumber = userEntity.MobileNumber;
            EMail = userEntity.EMail;
            AccountStatus = userEntity.AccountStatus.ToString();
            Organizations = (from orgMap in userEntity.UserOrganizationMappings
                             select new UserOrganization
                             {
                                 Id = orgMap.OrganizationId,
                                 Name = orgMap.Organization.Name,
                                 Address = orgMap.Organization.Address
                             }).ToList();
            Roles = (from roleMap in userEntity.UserRoleMappings
                     select new UserRole
                     {
                         Id = roleMap.Id,
                         Name = roleMap.Role.Name
                     }).ToList();

            Token = jwtToken;
            RefreshToken = refreshToken;
        }
    }

    public class UserOrganization
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }

    public class UserRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EMail { get; set; }

        public string Password { get; set; }

        public List<UserOrganization> Organizations { get; set; }

        public List<UserRole> Roles { get; set; }
    }

    public class SetOrganizationRequestModel
    {
        public long OrganizationId { get; set; }
    }

    
}
