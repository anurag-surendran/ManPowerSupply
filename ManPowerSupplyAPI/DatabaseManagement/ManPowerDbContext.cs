using ManPowerSupplyAPI.DatabaseManagement.Entities;
using ManPowerSupplyAPI.DatabaseManagement.EntityConfigurations;
using ManPowerSupplyAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.DatabaseManagement
{
    public class ManPowerDbContext : DbContext
    {
        public ManPowerDbContext()
        {

        }

        public ManPowerDbContext(DbContextOptions<ManPowerDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString = "Data Source=MTPL-2332;Initial Catalog=ManPowerDB;Persist Security Info=True;User ID=sa;Password=cmg434";
        //    Console.WriteLine("Loaded local connectionstring");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<EmployeeSkillEntity> EmployeeSkills { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<EmployeeSkillMapperEntity> EmployeeSkillMappers { get; set; }
        public DbSet<AttendanceEntity> Attendances { get; set; }
        public DbSet<CustomerReceiptEntity> CustomerReceipts { get; set; }
        public DbSet<EmployeePaymentTypeEntity> EmployeePaymentTypes { get; set; }
        public DbSet<EmployeePaymentEntity> EmployeePayments { get; set; }
        public DbSet<AccountGroupEntity> AccountGroups { get; set; }
        public DbSet<AccountHeadEntity> AccountHeads { get; set; }
        public DbSet<ReceiptAndPaymentEntity> ReceiptAndPayments { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<UserOrganizationMappingEntity> UserOrganizationMappings { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleMappingEntity> UserRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillMapperEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerReceiptEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeePaymentTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeePaymentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AccountGroupEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHeadEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptAndPaymentEntityConfiguration());

            modelBuilder.ApplyConfiguration(new OrganizationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserOrganizationMappingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleMappingEntityConfiguration());

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    MobileNumber = "9633676520",
                    EMail = "anuragks1103@gmail.com",
                    NoOfFailedAttempts = 0,
                    AccountStatus = UserAccountStatus.Active,
                    Password = "SuperAdmin",
                    PasswordSalt = "PasswordSalt",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow,

                });

            modelBuilder.Entity<OrganizationEntity>().HasData(
                new OrganizationEntity
                {
                    Id = 1,
                    Name = "Viswas Man Power Supply",
                    Address = ""
                });

            modelBuilder.Entity<EmployeePaymentTypeEntity>().HasData(
               new EmployeePaymentTypeEntity { Id = 1, Name = "Employee Pay - Cash" },
               new EmployeePaymentTypeEntity { Id = 2, Name = "Employee Pay - Bank" },
               new EmployeePaymentTypeEntity { Id = 3, Name = "Advance Salary Payment" },
               new EmployeePaymentTypeEntity { Id = 4, Name = "Mobile Recharge" },
               new EmployeePaymentTypeEntity { Id = 5, Name = "Return Cutting" });

            modelBuilder.Entity<AccountGroupEntity>().HasData(
               new AccountGroupEntity() { Id = 1, Name = "Other Income", AccountType = AccountTypes.Income },
               new AccountGroupEntity() { Id = 2, Name = "Other Expense", AccountType = AccountTypes.Expenditure },
               new AccountGroupEntity() { Id = 3, Name = "Capital", AccountType = AccountTypes.Liability },
               new AccountGroupEntity() { Id = 4, Name = "Bank", AccountType = AccountTypes.Asset }
               );

            modelBuilder.Entity<AccountHeadEntity>().HasData(
               new AccountHeadEntity() { Id = 1, Name = "Company TA", AccountGroupId = 2 },
               new AccountHeadEntity() { Id = 2, Name = "Cutomer Receipt", AccountGroupId = 1 },
               new AccountHeadEntity() { Id = 3, Name = "Employee Payment", AccountGroupId = 2 }
               );
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
           .Entries()
           .Where(e =>
                   e.State == EntityState.Added ||
                   e.State == EntityState.Modified
            ).ToList();

            entries.ForEach(E =>
            {
                try
                {
                    E.Property("LastUpdatedDate").CurrentValue = DateTime.Now;
                    E.Property("LastUpdatedBy").CurrentValue = LoggedUser.Instance.CurrentUser.UserName;
                    E.Property("OrganizationId").CurrentValue = 
                            Convert.ToInt64(E.Property("OrganizationId").CurrentValue) != 0 ? 
                                E.Property("OrganizationId").CurrentValue :
                                LoggedUser.Instance.CurrentOrganization;
                }
                catch (Exception)
                {
                }
            });

            return base.SaveChangesAsync(true);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
           .Entries()
           .Where(e =>
                   e.State == EntityState.Added ||
                   e.State == EntityState.Modified
            ).ToList();

            entries.ForEach(E =>
            {
                try
                {
                    E.Property("LastUpdatedDate").CurrentValue = DateTime.Now;
                    E.Property("LastUpdatedBy").CurrentValue = LoggedUser.Instance.CurrentUser.UserName;
                    E.Property("OrganizationId").CurrentValue = LoggedUser.Instance.CurrentOrganization;
                }
                catch (Exception)
                {
                }
            });

            return base.SaveChanges();
        }
    }
}
