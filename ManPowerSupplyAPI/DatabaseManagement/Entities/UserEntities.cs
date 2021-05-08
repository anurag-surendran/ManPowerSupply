using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.DatabaseManagement.Entities
{
    public class UserEntity 
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EMail { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public int NoOfFailedAttempts { get; set; }

        public UserAccountStatus AccountStatus { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; }

        public ICollection<UserOrganizationMappingEntity> UserOrganizationMappings { get; set; }

        public ICollection<UserRoleMappingEntity> UserRoleMappings { get; set; }
    }

    public class RefreshTokenEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public UserEntity User { get; set; }

        public DateTime Expires { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }
    }

    public class UserOrganizationMappingEntity
    {
        public long Id { get; set; }

        public long OrganizationId { get; set; }
        public OrganizationEntity Organization { get; set; }

        public long UserId { get; set; }
        public UserEntity User { get; set; }
    }

    public class RoleEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserRoleMappingEntity> UserRoleMappings { get; set; }
    }

    public class UserRoleMappingEntity
    {
        public long Id { get; set; }

        public long RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public long UserId { get; set; }
        public UserEntity User { get; set; }
    }

    public enum UserAccountStatus
    {
        Active,
        DeActive,
        Suspended 
    }
}
