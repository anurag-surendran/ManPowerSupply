using ManPowerSupplyAPI.DatabaseManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.DatabaseManagement.EntityConfigurations
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
     where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property<bool>("IsDeleted")
               .IsRequired()
               .HasDefaultValue(false);

            builder.Property<DateTime>("LastUpdatedDate")
               .IsRequired()
               .HasDefaultValueSql("GETDATE()")
               .HasColumnType("datetime");

            builder.Property<string>("LastUpdatedBy")
               .IsRequired()
               .HasMaxLength(250);

            builder.Property<long>("OrganizationId")
                .IsRequired()
                .HasDefaultValueSql("1");
        }

    }
    public class CustomerEntityConfiguration : BaseEntityTypeConfiguration<CustomerEntity>
    {
        public override void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.ToTable("Customer");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(15);
            builder.HasIndex(x => x.Mobile).IsUnique();

            builder.Property(x => x.AlternateMobile).IsRequired(false).HasMaxLength(15);

            builder.Property(x => x.Address).IsRequired().HasMaxLength(500);

        }
    }

    public class EmployeeSkillEntityConfiguration : BaseEntityTypeConfiguration<EmployeeSkillEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeeSkillEntity> builder)
        {
            builder.ToTable("EmployeeSkill");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        }
    }

    public class EmployeeEntityConfiguration : BaseEntityTypeConfiguration<EmployeeEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.ToTable("Employee");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(15);
            builder.HasIndex(x => x.Mobile).IsUnique();

            builder.Property(x => x.AlternateMobile).IsRequired(false).HasMaxLength(15);

            builder.Property(x => x.Location).IsRequired(false).HasMaxLength(250);

            builder.Property(x => x.IdentityDetails).IsRequired(false).HasMaxLength(50);

            builder.Property(x => x.Address).IsRequired(false).HasMaxLength(500);

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        }
    }

    public class EmployeeSkillMapperEntityConfiguration : BaseEntityTypeConfiguration<EmployeeSkillMapperEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeeSkillMapperEntity> builder)
        {
            builder.ToTable("EmployeeSkillMapper");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeSkillMappers).HasForeignKey(x => x.EmployeeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.EmployeeSkill).WithMany(x => x.EmployeeSkillMappers).HasForeignKey(x => x.EmployessSkillId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class AttendanceEntityConfiguration : BaseEntityTypeConfiguration<AttendanceEntity>
    {
        public override void Configure(EntityTypeBuilder<AttendanceEntity> builder)
        {
            builder.ToTable("Attendance");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Date).IsRequired().HasColumnType("date");

            builder.HasOne(x => x.Employee).WithMany(x => x.Attendances).HasForeignKey(x => x.EmployeeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Customer).WithMany(x => x.Attendances).HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.AttendanceStatus).IsRequired(false);

            builder.HasOne(x => x.NextDayCustomer).WithMany(x => x.NextDayAttendances).HasForeignKey(x => x.NextDayCustomerId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<ReceiptAndPaymentEntity>(x => x.ReceiptAndPayment).WithOne(x => x.Attendace).HasForeignKey<AttendanceEntity>(x => x.ReceiptAndPaymentId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.CustomerPay).IsRequired().HasMaxLength(4);

            builder.Property(x => x.Rent).IsRequired().HasMaxLength(4);

            builder.Property(x => x.CustomerTA).IsRequired().HasMaxLength(4);

            builder.Property(x => x.CompanyTA).IsRequired().HasMaxLength(4);

            builder.Property(x => x.EmployeePay).IsRequired().HasMaxLength(4);

            builder.Property(x => x.Remarks).IsRequired(false).HasMaxLength(500);
        }
    }

    public class CustomerReceiptEntityConfiguration : BaseEntityTypeConfiguration<CustomerReceiptEntity>
    {
        public override void Configure(EntityTypeBuilder<CustomerReceiptEntity> builder)
        {
            builder.ToTable("CustomerPayment");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Customer).WithMany(x => x.CustomerReceipts).HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Date).IsRequired();

            builder.Property(x => x.PaidAmount).IsRequired();

            builder.Property(x => x.CashCollectedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Remarks).IsRequired(false).HasMaxLength(500);

            builder.HasOne<ReceiptAndPaymentEntity>(x => x.ReceiptAndPayment).WithOne(x => x.CustomerReceipt).HasForeignKey<CustomerReceiptEntity>(x => x.ReceiptAndPaymentId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class EmployeePaymentTypeEntityConfiguration : BaseEntityTypeConfiguration<EmployeePaymentTypeEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeePaymentTypeEntity> builder)
        {
            builder.ToTable("EmployeePaymentType");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        }
    }

    public class EmployeePaymentEntityConfiguration : BaseEntityTypeConfiguration<EmployeePaymentEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeePaymentEntity> builder)
        {
            builder.ToTable("EmployeePayment");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.Employee).WithMany(x => x.EmployeePayments).HasForeignKey(x => x.EmployeeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PaymentType).WithMany(x => x.EmployeePayments).HasForeignKey(x => x.PaymentTypeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Remarks).IsRequired(false).HasMaxLength(250);

            builder.HasOne<ReceiptAndPaymentEntity>(x => x.ReceiptAndPayment).WithOne(x => x.EmployeePayment).HasForeignKey<EmployeePaymentEntity>(x => x.ReceiptAndPaymentId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }


    public class AccountGroupEntityConfiguration : BaseEntityTypeConfiguration<AccountGroupEntity>
    {
        public override void Configure(EntityTypeBuilder<AccountGroupEntity> builder)
        {
            builder.ToTable("AccountGroup");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.AccountType).IsRequired();

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);
        }
    }

    public class AccountHeadEntityConfiguration : BaseEntityTypeConfiguration<AccountHeadEntity>
    {
        public override void Configure(EntityTypeBuilder<AccountHeadEntity> builder)
        {
            builder.ToTable("AccountHead");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.HasOne(x => x.AccountGroup).WithMany(x => x.AccountHeads).HasForeignKey(x => x.AccountGroupId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.IsDualAccount).IsRequired().HasDefaultValue(false);

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);

        }
    }

    public class ReceiptAndPaymentEntityConfiguration : BaseEntityTypeConfiguration<ReceiptAndPaymentEntity>
    {
        public override void Configure(EntityTypeBuilder<ReceiptAndPaymentEntity> builder)
        {
            builder.ToTable("ReceiptAndPayment");

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.AccountHead).WithMany(x => x.ReceiptAndPayments).HasForeignKey(x => x.AccountHeadId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.TransactionType).IsRequired();

            builder.Property(x => x.Amount).IsRequired().HasMaxLength(18);

            builder.Property(x => x.Particular).IsRequired(false).HasMaxLength(250);

            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);

        }
    }
}
