using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.DatabaseManagement.Entities
{
    public class OrganizationEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<UserOrganizationMappingEntity> UserOrganizationMappings { get; set; }
    }


    public class BaseEntity
    {
        public bool IsDeleted { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public long OrganizationId { get; set; }
    }

    public class CustomerEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }

        public string Address { get; set; }
        public ICollection<AttendanceEntity> Attendances { get; set; }
        public ICollection<AttendanceEntity> NextDayAttendances { get; set; }
        public ICollection<CustomerReceiptEntity> CustomerReceipts { get; set; }
    }

    public class EmployeeSkillEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public ICollection<EmployeeSkillMapperEntity> EmployeeSkillMappers { get; set; }
    }

    public class EmployeeEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }

        public string Location { get; set; }

        public string IdentityDetails { get; set; }

        public string Address { get; set; }

        public bool? IsVerified { get; set; }

        public string Description { get; set; }


        public ICollection<EmployeeSkillMapperEntity> EmployeeSkillMappers { get; set; }
        public ICollection<AttendanceEntity> Attendances { get; set; }
        public ICollection<EmployeePaymentEntity> EmployeePayments { get; set; }
    }

    public class EmployeeSkillMapperEntity : BaseEntity
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }

        public long EmployessSkillId { get; set; }
        public EmployeeSkillEntity EmployeeSkill { get; set; }
    }



    public class AttendanceEntity : BaseEntity
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }

        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }

        public bool? AttendanceStatus { get; set; }

        public long NextDayCustomerId { get; set; }
        public CustomerEntity NextDayCustomer { get; set; }

        public long? ReceiptAndPaymentId { get; set; }
        public ReceiptAndPaymentEntity ReceiptAndPayment { get; set; }

        public int CustomerPay { get; set; }
        public int Rent { get; set; }
        public int CustomerTA { get; set; }
        public int CompanyTA { get; set; }
        public int EmployeePay { get; set; }

        public string Remarks { get; set; }
    }

    public class CustomerReceiptEntity : BaseEntity
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }

        public DateTime Date { get; set; }

        public long PaidAmount { get; set; }

        public string CashCollectedBy { get; set; }

        public string Remarks { get; set; }

        public long? ReceiptAndPaymentId { get; set; }
        public ReceiptAndPaymentEntity ReceiptAndPayment { get; set; }
    }

    public class EmployeePaymentTypeEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<EmployeePaymentEntity> EmployeePayments { get; set; }
    }

    public class EmployeePaymentEntity : BaseEntity
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }

        public long PaymentTypeId { get; set; }
        public EmployeePaymentTypeEntity PaymentType { get; set; }

        public long Amount { get; set; }

        public string Remarks { get; set; }

        public long? ReceiptAndPaymentId { get; set; }
        public ReceiptAndPaymentEntity ReceiptAndPayment { get; set; }
    }

    public enum AccountTypes
    {
        Asset = 1,
        Liability = 2,
        Income = 3,
        Expenditure = 4
    }

    public enum TransactionTypes
    {
        Receipt = 1,
        Payment = 2
    }

    public class AccountGroupEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public AccountTypes AccountType { get; set; }

        public string Description { get; set; }


        public ICollection<AccountHeadEntity> AccountHeads { get; set; }
    }

    public class AccountHeadEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long AccountGroupId { get; set; }
        public AccountGroupEntity AccountGroup { get; set; }

        public bool IsDualAccount { get; set; }

        public string Description { get; set; }


        public ICollection<ReceiptAndPaymentEntity> ReceiptAndPayments { get; set; }
    }

    public class ReceiptAndPaymentEntity : BaseEntity
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long AccountHeadId { get; set; }
        public AccountHeadEntity AccountHead { get; set; }

        public TransactionTypes TransactionType { get; set; }

        public long Amount { get; set; }

        public string Particular { get; set; }

        public string Description { get; set; }


        public AttendanceEntity Attendace { get; set; }
        public CustomerReceiptEntity CustomerReceipt { get; set; }
        public EmployeePaymentEntity EmployeePayment { get; set; }


    }
}
