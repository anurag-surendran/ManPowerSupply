﻿// <auto-generated />
using System;
using ManPowerSupplyAPI.DatabaseManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ManPowerSupplyAPI.Migrations
{
    [DbContext(typeof(ManPowerDbContext))]
    [Migration("20210117095919_IncomeAndExpenditureReport")]
    partial class IncomeAndExpenditureReport
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountGroupEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AccountGroup");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AccountType = 3,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Other Income"
                        },
                        new
                        {
                            Id = 2L,
                            AccountType = 4,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Other Expense"
                        },
                        new
                        {
                            Id = 3L,
                            AccountType = 2,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Capital"
                        },
                        new
                        {
                            Id = 4L,
                            AccountType = 1,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bank"
                        });
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountHeadEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("AccountGroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDualAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("AccountGroupId");

                    b.ToTable("AccountHead");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AccountGroupId = 2L,
                            IsDeleted = false,
                            IsDualAccount = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Company TA"
                        },
                        new
                        {
                            Id = 2L,
                            AccountGroupId = 1L,
                            IsDeleted = false,
                            IsDualAccount = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cutomer Receipt"
                        },
                        new
                        {
                            Id = 3L,
                            AccountGroupId = 2L,
                            IsDeleted = false,
                            IsDualAccount = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Employee Payment"
                        });
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AttendanceEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<bool?>("AttendanceStatus")
                        .HasColumnType("bit");

                    b.Property<int>("CompanyTA")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<int>("CustomerPay")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<int>("CustomerTA")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<int>("EmployeePay")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("NextDayCustomerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReceiptAndPaymentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Rent")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("NextDayCustomerId");

                    b.HasIndex("ReceiptAndPaymentId")
                        .IsUnique()
                        .HasFilter("[ReceiptAndPaymentId] IS NOT NULL");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("AlternateMobile")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Mobile")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerReceiptEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("CashCollectedBy")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("PaidAmount")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReceiptAndPaymentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ReceiptAndPaymentId")
                        .IsUnique()
                        .HasFilter("[ReceiptAndPaymentId] IS NOT NULL");

                    b.ToTable("CustomerPayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("AlternateMobile")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("IdentityDetails")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Mobile")
                        .IsUnique();

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("PaymentTypeId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReceiptAndPaymentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("ReceiptAndPaymentId")
                        .IsUnique()
                        .HasFilter("[ReceiptAndPaymentId] IS NOT NULL");

                    b.ToTable("EmployeePayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentTypeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("EmployeePaymentType");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Employee Pay - Cash"
                        },
                        new
                        {
                            Id = 2L,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Employee Pay - Bank"
                        },
                        new
                        {
                            Id = 3L,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Advance Salary Payment"
                        },
                        new
                        {
                            Id = 4L,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mobile Recharge"
                        },
                        new
                        {
                            Id = 5L,
                            IsDeleted = false,
                            LastUpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Return Cutting"
                        });
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeSkillEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("EmployeeSkill");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeSkillMapperEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployessSkillId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployessSkillId");

                    b.ToTable("EmployeeSkillMapper");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("AccountHeadId")
                        .HasColumnType("bigint");

                    b.Property<long>("Amount")
                        .HasMaxLength(18)
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Particular")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountHeadId");

                    b.ToTable("ReceiptAndPayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountHeadEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountGroupEntity", "AccountGroup")
                        .WithMany("AccountHeads")
                        .HasForeignKey("AccountGroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AccountGroup");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AttendanceEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerEntity", "Customer")
                        .WithMany("Attendances")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeEntity", "Employee")
                        .WithMany("Attendances")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerEntity", "NextDayCustomer")
                        .WithMany("NextDayAttendances")
                        .HasForeignKey("NextDayCustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", "ReceiptAndPayment")
                        .WithOne("Attendace")
                        .HasForeignKey("ManPowerSupplyAPI.DatabaseManagement.Entities.AttendanceEntity", "ReceiptAndPaymentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("NextDayCustomer");

                    b.Navigation("ReceiptAndPayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerReceiptEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerEntity", "Customer")
                        .WithMany("CustomerReceipts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", "ReceiptAndPayment")
                        .WithOne("CustomerReceipt")
                        .HasForeignKey("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerReceiptEntity", "ReceiptAndPaymentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Customer");

                    b.Navigation("ReceiptAndPayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeEntity", "Employee")
                        .WithMany("EmployeePayments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentTypeEntity", "PaymentType")
                        .WithMany("EmployeePayments")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", "ReceiptAndPayment")
                        .WithOne("EmployeePayment")
                        .HasForeignKey("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentEntity", "ReceiptAndPaymentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Employee");

                    b.Navigation("PaymentType");

                    b.Navigation("ReceiptAndPayment");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeSkillMapperEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeEntity", "Employee")
                        .WithMany("EmployeeSkillMappers")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeSkillEntity", "EmployeeSkill")
                        .WithMany("EmployeeSkillMappers")
                        .HasForeignKey("EmployessSkillId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("EmployeeSkill");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", b =>
                {
                    b.HasOne("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountHeadEntity", "AccountHead")
                        .WithMany("ReceiptAndPayments")
                        .HasForeignKey("AccountHeadId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AccountHead");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountGroupEntity", b =>
                {
                    b.Navigation("AccountHeads");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.AccountHeadEntity", b =>
                {
                    b.Navigation("ReceiptAndPayments");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.CustomerEntity", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("CustomerReceipts");

                    b.Navigation("NextDayAttendances");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeEntity", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("EmployeePayments");

                    b.Navigation("EmployeeSkillMappers");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeePaymentTypeEntity", b =>
                {
                    b.Navigation("EmployeePayments");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.EmployeeSkillEntity", b =>
                {
                    b.Navigation("EmployeeSkillMappers");
                });

            modelBuilder.Entity("ManPowerSupplyAPI.DatabaseManagement.Entities.ReceiptAndPaymentEntity", b =>
                {
                    b.Navigation("Attendace");

                    b.Navigation("CustomerReceipt");

                    b.Navigation("EmployeePayment");
                });
#pragma warning restore 612, 618
        }
    }
}
