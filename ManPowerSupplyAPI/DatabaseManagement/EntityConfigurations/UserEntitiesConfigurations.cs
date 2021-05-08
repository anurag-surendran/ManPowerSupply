using ManPowerSupplyAPI.DatabaseManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.DatabaseManagement.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(250);

            builder.Property(x => x.UserName).IsRequired().HasMaxLength(250);
            builder.HasIndex(x => x.UserName).IsUnique();

            builder.Property(x => x.MobileNumber).IsRequired().HasMaxLength(20);

            builder.Property(x => x.EMail).IsRequired().HasMaxLength(250);

            builder.Property(x => x.LastLoggedIn).IsRequired(false);

            builder.Property(x => x.NoOfFailedAttempts).IsRequired();

            builder.Property(x => x.AccountStatus).IsRequired();

            builder.Property(x => x.Password).IsRequired().HasMaxLength(500);

            builder.Property(x => x.PasswordSalt).IsRequired().HasMaxLength(500);

            builder.Property(x => x.CreatedBy).IsRequired();

            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.UpdatedBy).IsRequired(false);

            builder.Property(x => x.UpdatedOn).IsRequired(false);

        }
    }

    public class OrganizationEntityConfiguration : IEntityTypeConfiguration<OrganizationEntity>
    {
       

        public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
        {
            builder.ToTable("Organization");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Address).IsRequired(false).HasMaxLength(1000);
        }
    }

    #region RefreshTokenEntityConfiguration
    public class RefreshTokenEntityConfiguration : BaseEntityTypeConfiguration<RefreshTokenEntity>
    {
        public override void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshToken");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Token).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Expires).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.CreatedByIp).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Revoked).IsRequired(false);

            builder.Property(x => x.RevokedByIp).IsRequired(false).HasMaxLength(250);

            builder.Property(x => x.ReplacedByToken).IsRequired(false).HasMaxLength(500);

            base.Configure(builder);
        }
    }
    #endregion

    #region UserOrganizationMappingEntityConfiguration
    public class UserOrganizationMappingEntityConfiguration : BaseEntityTypeConfiguration<UserOrganizationMappingEntity>
    {
        public override void Configure(EntityTypeBuilder<UserOrganizationMappingEntity> builder)
        {
            builder.ToTable("UserOrganizationMapping");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Organization).WithMany(x => x.UserOrganizationMappings).HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            
            builder.HasOne(x => x.User).WithMany(x => x.UserOrganizationMappings).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            base.Configure(builder);
        }
    }
    #endregion

    #region RoleEntity Configuration

    public class RoleEntityConfiguration : BaseEntityTypeConfiguration<RoleEntity>
    {
        public override void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Role");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);

            base.Configure(builder);
        }
    }

    #endregion

    #region UserRoleMappingEntity Configurations

    public class UserRoleMappingEntityConfiguration : BaseEntityTypeConfiguration<UserRoleMappingEntity>
    {
        public override void Configure(EntityTypeBuilder<UserRoleMappingEntity> builder)
        {
            builder.ToTable("UserRoleMapping");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.User).WithMany(x => x.UserRoleMappings).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            builder.HasOne(x => x.Role).WithMany(x => x.UserRoleMappings).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            base.Configure(builder);
        }
    }
    #endregion
}
