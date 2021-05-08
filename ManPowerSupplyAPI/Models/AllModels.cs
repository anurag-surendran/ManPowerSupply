using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Models
{
    #region Customer
    public class CreateCustomerRequestModel
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }

        public string Address { get; set; }
    }

    public class CustomerModel : DatabaseManagement.Entities.BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }

        public string Address { get; set; }

        public long BalanceAmount { get; set; }
    }
    #endregion

    #region Employee
    public class CreateSkillsRequestModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class EmployeeModel
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

        public string SkillsAsPlainText { get; set; }

        public long BalanceAmount { get; set; }

        public List<EmployeeSkillsModel> Skills { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }

    public class EmployeeSkillsModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }

    public class CreateEmployeeRequestModel
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }

        public string Location { get; set; }

        public string IdentityDetails { get; set; }

        public string Address { get; set; }

        public List<EmployeeSkillsModel> Skills { get; set; }
    }

    #endregion

    #region Attendance
    public class AttendanceModel
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        public bool? AttendanceStatus { get; set; }

        public long NextDayCustomerId { get; set; }
        public string NextDayCustomerName { get; set; }

        public int CustomerPay { get; set; }
        public int Rent { get; set; }
        public int CustomerTA { get; set; }
        public int CompanyTA { get; set; }
        public int EmployeePay { get; set; }

        public string Skill { get; set; }

        public string SkillCode { get; set; }

        public string Remarks { get; set; }

        public bool IsDeleted { get; set; }

        public string LastUpdatedBy { get; set; }
    }

    public class CreateAttendanceRequestModel
    {
        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }

        public long CustomerId { get; set; }

        public bool? AttendanceStatus { get; set; }

        public long NextDayCustomerId { get; set; }

        public int CustomerPay { get; set; }
        public int Rent { get; set; }
        public int CustomerTA { get; set; }
        public int CompanyTA { get; set; }
        public int EmployeePay { get; set; }

        public string Remarks { get; set; }
    }

    #endregion

    #region Customer Receipt

    public class CustomerReceiptRequestModel
    {
        public long CustomerId { get; set; }

        public DateTime Date { get; set; }

        public long PaidAmount { get; set; }

        public string CashCollectedBy { get; set; }

        public string Remarks { get; set; }
    }

    public class CustomerReceiptResponseRequestModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long? CustomerId { get; set; }
    }

    public class CustomerReceiptModel : DatabaseManagement.Entities.BaseEntity
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        public DateTime Date { get; set; }

        public long PaidAmount { get; set; }

        public string CashCollectedBy { get; set; }

        public string Remarks { get; set; }
    }

    #endregion

    #region Employee Payment

    public class EmployeePaymentModel : DatabaseManagement.Entities.BaseEntity
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public long PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }

        public long Amount { get; set; }

        public string Remarks { get; set; }
    }
    public class EmployeePaymentRequestModel
    {
        public DateTime Date { get; set; }

        public long EmployeeId { get; set; }

        public long PaymentTypeId { get; set; }

        public long Amount { get; set; }

        public string Remarks { get; set; }
    }
    public class EmployeePaymentResponseRequestModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long? EmployeeId { get; set; }
    }
    #endregion

    #region Reports

    public class CustomerLedgerModel
    {
        public long OpeningBalance { get; set; }

        public List<CustomerLedgerParticular> Particulars { get; set; }

        public long ClosingBalance { get; set; }
    }

    public class CustomerLedgerParticular
    {
        public DateTime Date { get; set; }

        public string Particular { get; set; }

        public long CustomerPay { get; set; }

        public long CustomerTA { get; set; }

        public long Rent { get; set; }

        public long TotalPay { get; set; }

        public long Received { get; set; }

        public long Amount { get; set; }

        public long Balance { get; set; }

        public string Type { get; set; }
    }

    public class CustomerLedgerRequestModel
    {
        public long CustomerId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class EmployeeLedgerModel
    {
        public long OpeningBalance { get; set; }

        public long ClosingBalance { get; set; }

        public List<EmployeeLedgerParticular> Particulars { get; set; }
    }

    public class EmployeeLedgerParticular
    {
        public DateTime Date { get; set; }

        public string Particular { get; set; }

        public long EmployeePay { get; set; }

        public long Payment { get; set; }

        public long Amount { get; set; }

        public long Balance { get; set; }

        public string Type { get; set; }
    }

    public class EmployeeLedgerRequestModel
    {
        public long EmployeeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    #endregion

    #region Account Manager

    public class AccountHeaderModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public string AccountType { get; set; }

        public string Description { get; set; }

    }

    public class AccountHeaderResponseRequestModel
    {
        public DatabaseManagement.Entities.AccountTypes? AccountType { get; set; }

        public long? GroupId { get; set; }

        public bool Restricted { get; set; }
    }


    public class AccountHeaderCreateRequestModel
    {
        public string Name { get; set; }

        public long AccountGroupId { get; set; }

        public string Description { get; set; }
    }

    #endregion

    #region Receipt And Payments

    public class ReceiptAndPaymentModel
    {
        public long OpeningBalance { get; set; }

        public long ClosingBalance { get; set; }

        public List<ReceiptAndPaymentModelParticulars> Particulars { get; set; }
    }
    public class ReceiptAndPaymentModelParticulars
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long AccountHeadId { get; set; }
        public string AccountHeadName { get; set; }

        public string TransactionType { get; set; }

        public long Amount { get; set; }

        public string Particular { get; set; }

        public string Description { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }

    public class ReceiptAndPaymentRequestModel
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public long? AccountHeadId { get; set; }

        public DatabaseManagement.Entities.TransactionTypes? TransactionType { get; set; }
    }

    public class CreateReceiptAndPaymentRequestModel
    {

        public DateTime Date { get; set; }

        public long AccountHeadId { get; set; }

        public DatabaseManagement.Entities.TransactionTypes TransactionType { get; set; }

        public long Amount { get; set; }

        public string Particular { get; set; }

        public string Description { get; set; }
    }

    public class IncomeAndExpenditureReportModel
    {
        public long OpeningBalance { get; set; }

        public long ClosingBalance { get; set; }

        public List<IncomeAndExpenditureReportParticular> Particulars { get; set; }
    }

    public class IncomeAndExpenditureReportParticular
    {
        public DateTime Date { get; set; }

        public long AccountHeadId { get; set; }
        public string AccountHeadName { get; set; }

        public string Particular { get; set; }

        public string TransactionType { get; set; }

        public long? ReceivedAmount { get; set; }

        public long? PaidAmount { get; set; }

        public long Amount { get; set; }

        public long Balance { get; set; }
    }

    #endregion
}
