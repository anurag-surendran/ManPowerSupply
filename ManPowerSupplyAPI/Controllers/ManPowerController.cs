using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManPowerSupplyAPI.DatabaseManagement;
using ManPowerSupplyAPI.DatabaseManagement.Entities;
using ManPowerSupplyAPI.Helpers;
using ManPowerSupplyAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManPowerSupplyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManPowerController : ControllerBase
    {
        private readonly ManPowerDbContext _context;

        public ManPowerController(ManPowerDbContext context)
        {
            _context = context;
        }

        #region Customer
        [HttpGet("Customers")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            var result = await (from x in _context.Customers
                                where x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                                orderby x.IsDeleted ascending, x.Name ascending 
                                select x).ToListAsync();
            return Ok(CustomerMapper(result));
        }

        [HttpPost("Customers")]
        public async Task<ActionResult<CustomerModel>> CreateCustomer([FromBody] CreateCustomerRequestModel requestModel)
        {

            if ((from x in _context.Customers where x.Mobile == requestModel.Mobile select x).Any())
                throw new Exception($"Mobile Number : {requestModel.Mobile} is already registered");

            var customerEntity = new CustomerEntity()
            {
                Name = requestModel.Name,
                Mobile = requestModel.Mobile,
                AlternateMobile = requestModel.AlternateMobile,
                Address = requestModel.Address
            };

            _context.Customers.Add(customerEntity);
            await _context.SaveChangesAsync();
            return Ok(CustomerMapper(customerEntity));
        }

        [HttpPut("Customers/CustomerId/{customerId}")]
        public async Task<ActionResult<CustomerModel>> UpdateCustomer(long customerId, CreateCustomerRequestModel requestModel)
        {
            var customerEntityQuery = from x in _context.Customers where x.Id == customerId select x;
            if (!customerEntityQuery.Any())
                throw new Exception($"Customer Id : {customerId} is not exists");

            if ((from x in _context.Customers where x.Mobile == requestModel.Mobile && x.Id != customerId select x).Any())
                throw new Exception($"Mobile Number : {requestModel.Mobile} is already registered");

            var customerEntity = customerEntityQuery.FirstOrDefault();

            customerEntity.Name = requestModel.Name;
            customerEntity.Mobile = requestModel.Mobile;
            customerEntity.AlternateMobile = requestModel.AlternateMobile;
            customerEntity.Address = requestModel.Address;

            _context.Customers.Update(customerEntity);
            await _context.SaveChangesAsync();

            return Ok(CustomerMapper(customerEntity));
        }

        [HttpDelete("Customers/CustomerId/{customerId}")]
        public async Task<ActionResult<CustomerModel>> DeleteCustomer(long customerId)
        {
            var customerEntityQuery = from x in _context.Customers where x.Id == customerId select x;
            if (!customerEntityQuery.Any())
                throw new Exception($"Customer Id : {customerId} is not exists");

            var customerEntity = customerEntityQuery.FirstOrDefault();
            customerEntity.IsDeleted = true;
            _context.Customers.Update(customerEntity);
            await _context.SaveChangesAsync();

            return Ok(CustomerMapper(customerEntity));
        }

        [HttpPut("Customers/CustomerId/{customerId}/Activate")]
        public async Task<ActionResult<CustomerModel>> ActivateCustomer(long customerId)
        {
            var customerEntityQuery = from x in _context.Customers where x.Id == customerId select x;
            if (!customerEntityQuery.Any())
                throw new Exception($"Customer Id : {customerId} is not exists");

            var customerEntity = customerEntityQuery.FirstOrDefault();
            customerEntity.IsDeleted = false;
            _context.Customers.Update(customerEntity);
            await _context.SaveChangesAsync();

            return Ok(CustomerMapper(customerEntity));
        }

        private IEnumerable<CustomerModel> CustomerMapper(IEnumerable<CustomerEntity> customerEntities)
        {
            var customerModels = from cus in customerEntities
                                 let receivable = (from x in _context.Attendances where x.CustomerId == cus.Id && !x.IsDeleted select x).
                                                      Sum(x => x.CustomerTA + x.CustomerPay + x.Rent)
                                 let received = (from x in _context.CustomerReceipts where x.CustomerId == cus.Id && !x.IsDeleted select x).
                                                      Sum(x => x.PaidAmount)
                                 select new CustomerModel
                                 {
                                     Id = cus.Id,
                                     Name = cus.Name,
                                     Mobile = cus.Mobile,
                                     Address = cus.Address,
                                     AlternateMobile = cus.AlternateMobile,
                                     BalanceAmount = receivable - received,
                                     IsDeleted = cus.IsDeleted,
                                     LastUpdatedBy = cus.LastUpdatedBy,
                                     LastUpdatedDate = cus.LastUpdatedDate
                                 };
            return customerModels;
        }

        private CustomerModel CustomerMapper(CustomerEntity customerEntity)
        {
            return CustomerMapper(new List<CustomerEntity>() { customerEntity }).FirstOrDefault();
        }
        #endregion


        #region Emoloyee Skill
        [HttpGet("EmployeeSkills")]
        public async Task<ActionResult<IEnumerable<EmployeeSkillEntity>>> GetAllSkills()
        {
            var result = await (from x in _context.EmployeeSkills orderby x.Name select x).ToListAsync();
            return Ok(result);
        }

        [HttpPost("EmployeeSkills")]
        public async Task<ActionResult<EmployeeSkillEntity>> CreateSkill(CreateSkillsRequestModel requestModel)
        {
            if ((from x in _context.EmployeeSkills where x.Name == requestModel.Name select x).Any())
                throw new Exception($"Skill: {requestModel.Name} is already registered");

            if ((from x in _context.EmployeeSkills where x.Code == requestModel.Code select x).Any())
                throw new Exception($"Skill Code : {requestModel.Code} is already registered");

            var skillEntity = new EmployeeSkillEntity()
            {
                Name = requestModel.Name,
                Code = requestModel.Code,
                Description = requestModel.Description
            };

            _context.EmployeeSkills.Add(skillEntity);
            await _context.SaveChangesAsync();

            return Ok(skillEntity);
        }

        [HttpPut("EmployeeSkills/{skillId}")]
        public async Task<ActionResult<EmployeeSkillEntity>> UpdateSkill(long skillId, CreateSkillsRequestModel requestModel)
        {
            if ((from x in _context.EmployeeSkills where x.Name == requestModel.Name && x.Id != skillId select x).Any())
                throw new Exception($"Skill: {requestModel.Name} is already registered");

            if ((from x in _context.EmployeeSkills where x.Code == requestModel.Code && x.Id != skillId select x).Any())
                throw new Exception($"Skill Code: {requestModel.Code} is already registered");

            var skillEntityQuery = from x in _context.EmployeeSkills where x.Id == skillId select x;
            if (!skillEntityQuery.Any())
                throw new Exception($"Skill Id : {skillId} is not exists");

            var skillEntity = skillEntityQuery.FirstOrDefault();

            skillEntity.Name = requestModel.Name;
            skillEntity.Code = requestModel.Code;
            skillEntity.Description = requestModel.Description;

            _context.EmployeeSkills.Update(skillEntity);

            await _context.SaveChangesAsync();

            return skillEntity;

        }
        #endregion


        #region Employee

        [HttpGet("Employees")]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetAllEmployees()
        {
            var emplyees = await (from x in _context.Employees orderby x.Name select x).ToListAsync();
            return Ok(EmployeeMapper(emplyees));
        }

        [HttpPost("Employees")]
        public async Task<ActionResult<EmployeeModel>> CreateEmployee(CreateEmployeeRequestModel requestModel)
        {
            if ((from x in _context.Employees where x.Mobile == requestModel.Mobile select x).Any())
                throw new Exception($"Mobile Number : {requestModel.Mobile} is already registered");

            var employeeEntity = new EmployeeEntity()
            {
                Name = requestModel.Name,
                Mobile = requestModel.Mobile,
                AlternateMobile = requestModel.AlternateMobile,
                Location = requestModel.Location,
                IdentityDetails = requestModel.IdentityDetails,
                Address = requestModel.Address
            };

            _context.Employees.Add(employeeEntity);

            await _context.SaveChangesAsync();

            var skillMapperEntities = new List<EmployeeSkillMapperEntity>();

            requestModel.Skills.ForEach(x =>
            {
                skillMapperEntities.Add(new EmployeeSkillMapperEntity()
                {
                    EmployeeId = employeeEntity.Id,
                    EmployessSkillId = x.Id
                });
            });

            _context.EmployeeSkillMappers.AddRange(skillMapperEntities);

            await _context.SaveChangesAsync();

            var result = EmployeeMapper(employeeEntity);

            return Ok(result);
        }

        [HttpPut("Employees/{employeeId}")]
        public async Task<ActionResult<EmployeeModel>> UpdateEmployee(long employeeId, CreateEmployeeRequestModel requestModel)
        {
            if ((from x in _context.Employees where x.Mobile == requestModel.Mobile && x.Id != employeeId select x).Any())
                throw new Exception($"Mobile Number : {requestModel.Mobile} is already registered");

            var employeeEntityQuery = from x in _context.Employees where x.Id == employeeId select x;
            if (!employeeEntityQuery.Any())
                throw new Exception($"Employee Id : {employeeId} is not exists");

            var employeeEntity = employeeEntityQuery.FirstOrDefault();

            employeeEntity.Name = requestModel.Name;
            employeeEntity.Mobile = requestModel.Mobile;
            employeeEntity.AlternateMobile = requestModel.AlternateMobile;
            employeeEntity.Location = requestModel.Location;
            employeeEntity.IdentityDetails = requestModel.IdentityDetails;
            employeeEntity.Address = requestModel.Address;

            _context.Employees.Update(employeeEntity);

            await _context.SaveChangesAsync();

            var allSkills = from x in _context.EmployeeSkillMappers where x.EmployeeId == employeeId select x;

            await allSkills.ForEachAsync(x => x.IsDeleted = true);

            _context.EmployeeSkillMappers.UpdateRange(allSkills);

            await _context.SaveChangesAsync();

            var updateSkills = new List<EmployeeSkillMapperEntity>();
            var newSkills = new List<EmployeeSkillMapperEntity>();

            requestModel.Skills.ForEach(skill =>
            {
                var mapperQuery = (from x in _context.EmployeeSkillMappers where x.EmployessSkillId == skill.Id && x.EmployeeId == employeeId select x);
                if (mapperQuery.Any())
                {
                    var mapperEntity = mapperQuery.FirstOrDefault();
                    mapperEntity.IsDeleted = false;
                    updateSkills.Add(mapperEntity);
                }
                else
                {
                    newSkills.Add(new EmployeeSkillMapperEntity()
                    {
                        EmployeeId = employeeId,
                        EmployessSkillId = skill.Id
                    });
                }
            });

            _context.AddRange(newSkills);
            _context.UpdateRange(updateSkills);

            await _context.SaveChangesAsync();

            var result = EmployeeMapper(employeeEntity);
            return result;
        }

        private IEnumerable<EmployeeModel> EmployeeMapper(IEnumerable<EmployeeEntity> employeeEntities)
        {
            var employeeModel = from emp in employeeEntities
                                let skills = from map in _context.EmployeeSkillMappers
                                             join skill in _context.EmployeeSkills on map.EmployessSkillId equals skill.Id
                                             where map.EmployeeId == emp.Id && !map.IsDeleted
                                             select skill
                                let payable = (from x in _context.Attendances where x.EmployeeId == emp.Id && !x.IsDeleted select x).
                                                      Sum(x => x.EmployeePay)
                                let paid = (from x in _context.EmployeePayments where x.EmployeeId == emp.Id && !x.IsDeleted select x).Sum(x => x.Amount)
                                select new EmployeeModel
                                {
                                    Id = emp.Id,
                                    Name = emp.Name,
                                    Mobile = emp.Mobile,
                                    Location = emp.Location,
                                    AlternateMobile = emp.AlternateMobile,
                                    Address = emp.Address,
                                    Description = emp.Description,
                                    IdentityDetails = emp.IdentityDetails,
                                    IsVerified = emp.IsVerified,
                                    LastUpdatedDate = emp.LastUpdatedDate,
                                    SkillsAsPlainText = string.Join(", ", skills.Select(x => x.Code).ToList()),
                                    BalanceAmount = payable - paid,
                                    Skills = (from x in skills
                                              select new EmployeeSkillsModel
                                              {
                                                  Code = x.Code,
                                                  Id = x.Id,
                                                  Name = x.Name
                                              }).ToList()
                                };
            return employeeModel;
        }

        private EmployeeModel EmployeeMapper(EmployeeEntity employeeEntity)
        {
            return EmployeeMapper(new List<EmployeeEntity>() { employeeEntity }).FirstOrDefault();
        }

        #endregion


        #region Attendance

        [HttpGet("Attendance/Date/{date}")]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> GetAttendance(DateTime date)
        {
            var attendanceEntities = from attendance in _context.Attendances
                                     join emp in _context.Employees on attendance.EmployeeId equals emp.Id
                                     where attendance.Date == date && !attendance.IsDeleted
                                     orderby emp.Name ascending
                                     select attendance;

            if (attendanceEntities.Any())
                return Ok(AttendanceMapper(attendanceEntities.ToList()));

            return new List<AttendanceModel>();
        }

        [HttpPost("Attendance")]
        public async Task<ActionResult<AttendanceModel>> AddAttendance(CreateAttendanceRequestModel requestModel)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var paymentEntity = await AddPaymentForCompanyTA(requestModel);

                var attendanceEntity = new AttendanceEntity()
                {
                    Date = requestModel.Date,
                    EmployeeId = requestModel.EmployeeId,
                    CustomerId = requestModel.CustomerId,
                    AttendanceStatus = requestModel.AttendanceStatus,
                    NextDayCustomerId = requestModel.NextDayCustomerId,
                    CustomerPay = requestModel.CustomerPay,
                    Rent = requestModel.Rent,
                    CustomerTA = requestModel.CustomerTA,
                    CompanyTA = requestModel.CompanyTA,
                    EmployeePay = requestModel.EmployeePay,
                    Remarks = requestModel.Remarks,
                    ReceiptAndPaymentId = paymentEntity.Id
                };

                _context.Attendances.Add(attendanceEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(AttendanceMapper(attendanceEntity));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> AddPaymentForCompanyTA(CreateAttendanceRequestModel requestModel)
        {
            var employee = await (from x in _context.Employees where x.Id == requestModel.EmployeeId select x).FirstOrDefaultAsync();

            var paymentEntity = new ReceiptAndPaymentEntity()
            {
                AccountHeadId = 1,
                Date = requestModel.Date,
                TransactionType = TransactionTypes.Payment,
                Amount = requestModel.CompanyTA,
                Particular = employee.Name,
                Description = requestModel.Remarks,
            };

            _context.ReceiptAndPayments.Add(paymentEntity);
            await _context.SaveChangesAsync();

            return paymentEntity;
        }

        [HttpPut("Attendance/{attendanceId}")]
        public async Task<ActionResult<AttendanceModel>> UpdateAttendance(long attendanceId, CreateAttendanceRequestModel requestModel)
        {
            var attendanceEntityQuery = from x in _context.Attendances where x.Id == attendanceId && !x.IsDeleted select x;
            if (!attendanceEntityQuery.Any())
                throw new Exception("Sorry! Attendance details not found");

            using var transaction = _context.Database.BeginTransaction();
            try
            {

                var attendanceEntity = attendanceEntityQuery.FirstOrDefault();

                attendanceEntity.EmployeeId = requestModel.EmployeeId;
                attendanceEntity.CustomerId = requestModel.CustomerId;
                attendanceEntity.AttendanceStatus = requestModel.AttendanceStatus;
                attendanceEntity.NextDayCustomerId = requestModel.NextDayCustomerId;
                attendanceEntity.CustomerPay = requestModel.CustomerPay;
                attendanceEntity.Rent = requestModel.Rent;
                attendanceEntity.CustomerTA = requestModel.CustomerTA;
                attendanceEntity.CompanyTA = requestModel.CompanyTA;
                attendanceEntity.EmployeePay = requestModel.EmployeePay;
                attendanceEntity.Remarks = requestModel.Remarks;
                var paymentEntity = await AddOrUpdatePaymentForCompanyTA(attendanceEntity, requestModel);
                attendanceEntity.ReceiptAndPaymentId = paymentEntity.Id;

                _context.Attendances.Update(attendanceEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(AttendanceMapper(attendanceEntity));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }

        }

        private async Task<ReceiptAndPaymentEntity> AddOrUpdatePaymentForCompanyTA(AttendanceEntity attendanceEntity, CreateAttendanceRequestModel requestModel)
        {
            if (attendanceEntity.ReceiptAndPaymentId == null)
                return await AddPaymentForCompanyTA(requestModel);

            var employee = await (from x in _context.Employees where x.Id == requestModel.EmployeeId select x).FirstOrDefaultAsync();
            var paymentEntity = await (from x in _context.ReceiptAndPayments where x.Id == attendanceEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            paymentEntity.Date = requestModel.Date;
            paymentEntity.Amount = requestModel.CompanyTA;
            paymentEntity.Particular = employee.Name;
            paymentEntity.Description = requestModel.Remarks;

            _context.ReceiptAndPayments.Update(paymentEntity);
            await _context.SaveChangesAsync();

            return paymentEntity;
        }

        [HttpDelete("Attendance/{attendanceId}")]
        public async Task<ActionResult<AttendanceModel>> DeleteAttendance(long attendanceId)
        {
            var attendanceEntityQuery = from x in _context.Attendances where x.Id == attendanceId select x;
            if (!attendanceEntityQuery.Any())
                throw new Exception("Sorry! Attendance details not found");

            using var transaction = _context.Database.BeginTransaction();
            try
            {

                var attendanceEntity = attendanceEntityQuery.FirstOrDefault();
                attendanceEntity.IsDeleted = true;
                attendanceEntity.CustomerPay = 0;
                attendanceEntity.CustomerTA = 0;
                attendanceEntity.Rent = 0;
                attendanceEntity.CompanyTA = 0;
                attendanceEntity.EmployeePay = 0;

                await DeletePaymentForCompanyTA(attendanceEntity);

                _context.Attendances.Update(attendanceEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(AttendanceMapper(attendanceEntity));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> DeletePaymentForCompanyTA(AttendanceEntity attendanceEntity)
        {
            if (attendanceEntity.ReceiptAndPaymentId == null)
                return null;


            var paymentEntity = await (from x in _context.ReceiptAndPayments where x.Id == attendanceEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            paymentEntity.Amount = 0;
            paymentEntity.IsDeleted = true;

            _context.ReceiptAndPayments.Update(paymentEntity);
            await _context.SaveChangesAsync();

            return paymentEntity;
        }

        [HttpPost("Attendance/Transfer")]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> TransferAttendance([FromBody] DateTime date)
        {
            var attendanceEntities = from x in _context.Attendances where x.Date == date.AddDays(1) && !x.IsDeleted select x;

            if (attendanceEntities.Any())
                throw new Exception("Already Transfered");

            attendanceEntities = from x in _context.Attendances where x.Date == date && !x.IsDeleted select x;

            await attendanceEntities.ForEachAsync(x =>
            {
                x.Id = 0;
                x.CustomerTA = x.CustomerId == x.NextDayCustomerId ? x.CustomerTA : 0;
                x.CompanyTA = x.CustomerId == x.NextDayCustomerId ? x.CompanyTA : 0;
                x.Date = x.Date.AddDays(1);
                x.CustomerId = x.NextDayCustomerId;
                x.AttendanceStatus = null;
                x.Rent = 0;
                x.ReceiptAndPaymentId = null;
            });

            _context.Attendances.AddRange(attendanceEntities);
            await _context.SaveChangesAsync();

            return Ok(AttendanceMapper(attendanceEntities.ToList()));
        }

        private IEnumerable<AttendanceModel> AttendanceMapper(IEnumerable<AttendanceEntity> attendanceEntities)
        {
            var attendanceModel = from attendance in attendanceEntities
                                  let employee = (from x in _context.Employees where x.Id == attendance.EmployeeId select x).FirstOrDefault()
                                  let skills = (from map in _context.EmployeeSkillMappers
                                                join skill in _context.EmployeeSkills on map.EmployessSkillId equals skill.Id
                                                where map.EmployeeId == attendance.EmployeeId && !map.IsDeleted
                                                select skill).ToList()
                                  let customer = (from x in _context.Customers where x.Id == attendance.CustomerId select x).FirstOrDefault()
                                  let nextDayCustomer = (from x in _context.Customers where x.Id == attendance.NextDayCustomerId select x).FirstOrDefault()
                                  select new AttendanceModel
                                  {
                                      Id = attendance.Id,
                                      Date = attendance.Date,
                                      EmployeeId = employee.Id,
                                      EmployeeName = employee.Name,
                                      CustomerId = customer.Id,
                                      CustomerName = customer.Name,
                                      AttendanceStatus = attendance.AttendanceStatus,
                                      NextDayCustomerId = nextDayCustomer.Id,
                                      NextDayCustomerName = nextDayCustomer.Name,
                                      CustomerPay = attendance.CustomerPay,
                                      Rent = attendance.Rent,
                                      CustomerTA = attendance.CustomerTA,
                                      CompanyTA = attendance.CompanyTA,
                                      EmployeePay = attendance.EmployeePay,
                                      Remarks = attendance.Remarks,
                                      Skill = string.Join(", ", skills.Select(x => x.Name).ToList()),
                                      SkillCode = string.Join(", ", skills.Select(x => x.Code).ToList()),
                                      IsDeleted = attendance.IsDeleted,
                                      LastUpdatedBy = attendance.LastUpdatedBy
                                  };
            return attendanceModel;
        }

        private AttendanceModel AttendanceMapper(AttendanceEntity attendanceEntity)
        {
            return AttendanceMapper(new List<AttendanceEntity>() { attendanceEntity }).FirstOrDefault();
        }
        #endregion


        #region Customer Receipt

        [HttpGet("Receipt/Customer")]
        public async Task<ActionResult<IEnumerable<CustomerReceiptModel>>> GetCustomerReceipts([FromQuery] CustomerReceiptResponseRequestModel requestModel)
        {
            var customerReceiptEntities = (from receipt in _context.CustomerReceipts
                                           where receipt.Date.Date >= requestModel.FromDate.Date
                                           && receipt.Date.Date <= requestModel.ToDate.Date
                                           && (requestModel.CustomerId == null || receipt.CustomerId == requestModel.CustomerId)
                                           select receipt).ToList();

            return Ok(CustomerReceiptMapper(customerReceiptEntities));
        }

        [HttpPost("Receipt/Customer")]
        public async Task<ActionResult<CustomerReceiptModel>> CreateCustomerReceipt([FromBody] CustomerReceiptRequestModel requestModel)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var receiptAndPaymentEntity = await AddCustomerReceiptToAccount(requestModel);
                var customerReceiptEntity = new CustomerReceiptEntity()
                {
                    CustomerId = requestModel.CustomerId,
                    Date = requestModel.Date,
                    PaidAmount = requestModel.PaidAmount,
                    CashCollectedBy = requestModel.CashCollectedBy,
                    Remarks = requestModel.Remarks,
                    ReceiptAndPaymentId = receiptAndPaymentEntity.Id
                };

                _context.CustomerReceipts.Add(customerReceiptEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(CustomerReceiptMapper(customerReceiptEntity));
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }

        }

        private async Task<ReceiptAndPaymentEntity> AddCustomerReceiptToAccount(CustomerReceiptRequestModel requestModel)
        {
            var customerName = (from x in _context.Customers where x.Id == requestModel.CustomerId select x.Name).FirstOrDefault();

            var receiptAndPaymentEntity = new ReceiptAndPaymentEntity()
            {
                Date = requestModel.Date,
                TransactionType = TransactionTypes.Receipt,
                AccountHeadId = 2,
                Particular = customerName,
                Amount = requestModel.PaidAmount,
                Description = requestModel.Remarks
            };

            _context.ReceiptAndPayments.Add(receiptAndPaymentEntity);
            await _context.SaveChangesAsync();

            return receiptAndPaymentEntity;
        }

        [HttpPut("Receipt/Customer/ReceiptId/{receiptId}")]
        public async Task<ActionResult<CustomerReceiptModel>> UpdateCustomerReceipt(long receiptId, [FromBody] CustomerReceiptRequestModel requestModel)
        {
            var customerReceiptEntityQuery = (from x in _context.CustomerReceipts where x.Id == receiptId select x);
            if (!customerReceiptEntityQuery.Any())
                throw new Exception($"Customer Receipt Id : {receiptId} not found.");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var customerReceiptEntity = customerReceiptEntityQuery.FirstOrDefault();

                customerReceiptEntity.CustomerId = requestModel.CustomerId;
                customerReceiptEntity.Date = requestModel.Date;
                customerReceiptEntity.PaidAmount = requestModel.PaidAmount;
                customerReceiptEntity.CashCollectedBy = requestModel.CashCollectedBy;
                customerReceiptEntity.Remarks = requestModel.Remarks;

                var receiptEntity = await AddOrUpdateCustomerReceiptToAccount(customerReceiptEntity, requestModel);
                customerReceiptEntity.ReceiptAndPaymentId = receiptEntity.Id;

                _context.CustomerReceipts.Update(customerReceiptEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(CustomerReceiptMapper(customerReceiptEntity));
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> AddOrUpdateCustomerReceiptToAccount(CustomerReceiptEntity customerReceiptEntity, CustomerReceiptRequestModel requestModel)
        {
            if (customerReceiptEntity.ReceiptAndPaymentId == null)
                return await AddCustomerReceiptToAccount(requestModel);

            var customerName = await (from x in _context.Customers where x.Id == requestModel.CustomerId select x.Name).FirstOrDefaultAsync();
            var receiptEntity = await (from x in _context.ReceiptAndPayments where x.Id == customerReceiptEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            receiptEntity.Date = requestModel.Date;
            receiptEntity.Amount = requestModel.PaidAmount;
            receiptEntity.Particular = customerName;
            receiptEntity.Description = requestModel.Remarks;

            _context.ReceiptAndPayments.Update(receiptEntity);
            await _context.SaveChangesAsync();

            return receiptEntity;
        }

        [HttpDelete("Receipt/Customer/ReceiptId/{receiptId}")]
        public async Task<ActionResult<CustomerReceiptModel>> DeleteCustomerReceipt(long receiptId)
        {
            var customerReceiptEntityQuery = (from x in _context.CustomerReceipts where x.Id == receiptId select x);
            if (!customerReceiptEntityQuery.Any())
                throw new Exception($"Customer Receipt Id : {receiptId} not found.");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var customerReceiptEntity = customerReceiptEntityQuery.FirstOrDefault();

                customerReceiptEntity.IsDeleted = true;
                customerReceiptEntity.PaidAmount = 0;

                await DeleteCustomerReceiptToAccount(customerReceiptEntity);

                _context.CustomerReceipts.Update(customerReceiptEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(CustomerReceiptMapper(customerReceiptEntity));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> DeleteCustomerReceiptToAccount(CustomerReceiptEntity customerReceiptEntity)
        {
            if (customerReceiptEntity.ReceiptAndPaymentId == null)
                return null;

            var receiptEntity = await (from x in _context.ReceiptAndPayments where x.Id == customerReceiptEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            receiptEntity.Amount = 0;
            receiptEntity.IsDeleted = true;

            _context.ReceiptAndPayments.Update(receiptEntity);
            await _context.SaveChangesAsync();

            return receiptEntity;
        }

        [HttpGet("Receipt/Customer/CollectionPersons")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCollectionPersons()
        {
            var collectionPersons = await (from x in _context.CustomerReceipts
                                           group x by x.CashCollectedBy into grp
                                           select grp.Key).ToListAsync();
            return Ok(collectionPersons);
        }

        private IEnumerable<CustomerReceiptModel> CustomerReceiptMapper(IEnumerable<CustomerReceiptEntity> customerReceiptEntities)
        {
            var customerReceiptModels = from receipt in customerReceiptEntities
                                        join cus in _context.Customers on receipt.CustomerId equals cus.Id
                                        orderby receipt.IsDeleted ascending, receipt.Date ascending, receipt.Id ascending
                                        select new CustomerReceiptModel
                                        {
                                            Id = receipt.Id,
                                            Date = receipt.Date,
                                            CustomerId = receipt.CustomerId,
                                            CustomerName = cus.Name,
                                            PaidAmount = receipt.PaidAmount,
                                            CashCollectedBy = receipt.CashCollectedBy,
                                            Remarks = receipt.Remarks,
                                            IsDeleted = receipt.IsDeleted,
                                            LastUpdatedBy = receipt.LastUpdatedBy,
                                            LastUpdatedDate = receipt.LastUpdatedDate
                                        };
            return customerReceiptModels;
        }

        private CustomerReceiptModel CustomerReceiptMapper(CustomerReceiptEntity customerReceiptEntity)
        {
            return CustomerReceiptMapper(new List<CustomerReceiptEntity>() { customerReceiptEntity }).FirstOrDefault();
        }

        #endregion


        #region Employee Payment

        [HttpGet("Payment/Employee")]
        public async Task<ActionResult<IEnumerable<EmployeePaymentModel>>> GetEmployeePayments([FromQuery] EmployeePaymentResponseRequestModel requestModel)
        {
            var employeePaymentEntities = (from x in _context.EmployeePayments
                                           where x.Date.Date >= requestModel.FromDate.Date
                                           && x.Date.Date <= requestModel.ToDate.Date
                                           && (requestModel.EmployeeId == null || x.EmployeeId == requestModel.EmployeeId)
                                           select x).ToList();
            return Ok(EmployeePaymentMapper(employeePaymentEntities));

        }

        [HttpPost("Payment/Employee")]
        public async Task<ActionResult<EmployeePaymentModel>> CreateEmployeePayment([FromBody] EmployeePaymentRequestModel requestModel)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var receiptAndPaymentEntity = await AddEmployeePaymentToAccount(requestModel);

                var employeePaymentEntity = new EmployeePaymentEntity()
                {
                    Date = requestModel.Date,
                    EmployeeId = requestModel.EmployeeId,
                    PaymentTypeId = requestModel.PaymentTypeId,
                    Amount = requestModel.Amount,
                    Remarks = requestModel.Remarks,
                    ReceiptAndPaymentId = receiptAndPaymentEntity.Id
                };

                _context.EmployeePayments.Add(employeePaymentEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(EmployeePaymentMapper(employeePaymentEntity));
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> AddEmployeePaymentToAccount(EmployeePaymentRequestModel requestModel)
        {
            var employeeName = await (from x in _context.Employees where x.Id == requestModel.EmployeeId select x.Name).FirstOrDefaultAsync();

            var receiptAndPaymentEntity = new ReceiptAndPaymentEntity()
            {
                Date = requestModel.Date,
                TransactionType = TransactionTypes.Payment,
                AccountHeadId = 3,
                Particular = employeeName,
                Amount = requestModel.Amount,
                Description = requestModel.Remarks
            };

            _context.ReceiptAndPayments.Add(receiptAndPaymentEntity);
            await _context.SaveChangesAsync();

            return receiptAndPaymentEntity;
        }

        [HttpPut("Payment/Employee/PaymentId/{paymentId}")]
        public async Task<ActionResult<EmployeePaymentModel>> UpdateEmployeePayment(long paymentId, [FromBody] EmployeePaymentRequestModel requestModel)
        {
            var employeeReceiptEntityQuery = (from x in _context.EmployeePayments where x.Id == paymentId select x);
            if (!employeeReceiptEntityQuery.Any())
                throw new Exception($"Employee Payment Id : {paymentId} not found.");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var employeePaymentEntity = employeeReceiptEntityQuery.FirstOrDefault();

                employeePaymentEntity.Date = requestModel.Date;
                employeePaymentEntity.EmployeeId = requestModel.EmployeeId;
                employeePaymentEntity.PaymentTypeId = requestModel.PaymentTypeId;
                employeePaymentEntity.Amount = requestModel.Amount;
                employeePaymentEntity.Remarks = requestModel.Remarks;

                var paymentEntity = await AddOrUpdateEmployeePaymentToAccount(employeePaymentEntity, requestModel);
                employeePaymentEntity.ReceiptAndPaymentId = paymentEntity.Id;

                _context.EmployeePayments.Update(employeePaymentEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(EmployeePaymentMapper(employeePaymentEntity));
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> AddOrUpdateEmployeePaymentToAccount(EmployeePaymentEntity employeePaymentEntity, EmployeePaymentRequestModel requestModel)
        {
            if (employeePaymentEntity.ReceiptAndPaymentId == null)
                return await AddEmployeePaymentToAccount(requestModel);

            var employeeName = await (from x in _context.Employees where x.Id == requestModel.EmployeeId select x.Name).FirstOrDefaultAsync();
            var paymentEntity = await (from x in _context.ReceiptAndPayments where x.Id == employeePaymentEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            paymentEntity.Date = requestModel.Date;
            paymentEntity.Amount = requestModel.Amount;
            paymentEntity.Particular = employeeName;
            paymentEntity.Description = requestModel.Remarks;

            _context.ReceiptAndPayments.Update(paymentEntity);
            await _context.SaveChangesAsync();

            return paymentEntity;
        }

        [HttpDelete("Payment/Employee/PaymentId/{paymentId}")]
        public async Task<ActionResult<EmployeePaymentModel>> DeleteEmployeePayment(long paymentId)
        {
            var employeeReceiptEntityQuery = (from x in _context.EmployeePayments where x.Id == paymentId select x);
            if (!employeeReceiptEntityQuery.Any())
                throw new Exception($"Employee Payment Id : {paymentId} not found.");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var employeePaymentEntity = employeeReceiptEntityQuery.FirstOrDefault();

                employeePaymentEntity.IsDeleted = true;
                employeePaymentEntity.Amount = 0;

                await DeleteEmployeePaymentToAccount(employeePaymentEntity);

                _context.EmployeePayments.Update(employeePaymentEntity);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(EmployeePaymentMapper(employeePaymentEntity));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Something went wrong. Please try again later.");
            }
        }

        private async Task<ReceiptAndPaymentEntity> DeleteEmployeePaymentToAccount(EmployeePaymentEntity employeePaymentEntity)
        {
            if (employeePaymentEntity.ReceiptAndPaymentId == null)
                return null;

            var paymentEntity = await (from x in _context.ReceiptAndPayments where x.Id == employeePaymentEntity.ReceiptAndPaymentId select x).FirstOrDefaultAsync();

            paymentEntity.Amount = 0;
            paymentEntity.IsDeleted = true;

            _context.ReceiptAndPayments.Update(paymentEntity);
            await _context.SaveChangesAsync();

            return paymentEntity;
        }

        private IEnumerable<EmployeePaymentModel> EmployeePaymentMapper(IEnumerable<EmployeePaymentEntity> employeePaymentEntities)
        {
            var employeePaymentModels = from payment in employeePaymentEntities
                                        join emp in _context.Employees on payment.EmployeeId equals emp.Id
                                        join type in _context.EmployeePaymentTypes on payment.PaymentTypeId equals type.Id
                                        orderby payment.IsDeleted ascending, payment.Date ascending
                                        select new EmployeePaymentModel
                                        {
                                            Id = payment.Id,
                                            Date = payment.Date,
                                            EmployeeId = emp.Id,
                                            EmployeeName = emp.Name,
                                            PaymentTypeId = type.Id,
                                            PaymentTypeName = type.Name,
                                            Amount = payment.Amount,
                                            Remarks = payment.Remarks,
                                            IsDeleted = payment.IsDeleted,
                                            LastUpdatedBy = payment.LastUpdatedBy,
                                            LastUpdatedDate = payment.LastUpdatedDate
                                        };
            return employeePaymentModels;
        }

        private EmployeePaymentModel EmployeePaymentMapper(EmployeePaymentEntity employeePaymentEntity)
        {
            return EmployeePaymentMapper(new List<EmployeePaymentEntity>() { employeePaymentEntity }).FirstOrDefault();
        }

        #endregion


        #region Employee Payment Type
        [HttpGet("EmployeePaymentTypes")]
        public async Task<ActionResult<EmployeePaymentTypeEntity>> GetAllEmployeePaymentTypes()
        {
            var employeePaymentTypes = from x in _context.EmployeePaymentTypes
                                       where !x.IsDeleted
                                       select x;
            return Ok(employeePaymentTypes);
        }
        #endregion


        #region Reports

        #region Customer Ledger

        [HttpGet("Reports/CustomerLedger")]
        public async Task<ActionResult<CustomerLedgerModel>> GetCustomerLedger([FromQuery] CustomerLedgerRequestModel requestModel)
        {
            var attendanceParticulars = GetCustomerAttendanceParticulars(requestModel);
            var paymentParticulars = GetCustomerReceivedParticulars(requestModel);

            var particulars = attendanceParticulars.Union(paymentParticulars).ToList().OrderBy(x => x.Date).ToList();

            long openingBalance = GetCustomerLedgerOpeningBalance(requestModel);
            var balance = openingBalance;
            long closingBalance = 0;

            for (int i = 1; i < particulars.Count; i++)
            {
                balance += particulars[i - 1].Amount;
                particulars[i - 1].Balance = balance;

                if (i == particulars.Count - 1)
                {
                    balance += particulars[i].Amount;
                    particulars[i].Balance = balance;
                    closingBalance = balance;
                }
            }

            var customerLedgerModel = new CustomerLedgerModel()
            {
                OpeningBalance = openingBalance,
                Particulars = particulars,
                ClosingBalance = closingBalance
            };

            return Ok(customerLedgerModel);

        }

        private List<CustomerLedgerParticular> GetCustomerAttendanceParticulars(CustomerLedgerRequestModel requestModel)
        {
            var attendanceParticulars = (from attendance in _context.Attendances
                                         join cus in _context.Customers on attendance.CustomerId equals cus.Id
                                         join emp in _context.Employees on attendance.EmployeeId equals emp.Id
                                         let totalPay = attendance.CustomerPay + attendance.CustomerTA + attendance.Rent
                                         where !attendance.IsDeleted
                                         && attendance.Date.Date >= requestModel.FromDate.Date
                                         && attendance.Date.Date <= requestModel.ToDate.Date
                                         && cus.Id == requestModel.CustomerId
                                         select new CustomerLedgerParticular
                                         {
                                             Date = attendance.Date,
                                             Particular = $"Work - {emp.Name}",
                                             CustomerPay = attendance.CustomerPay,
                                             CustomerTA = attendance.CustomerTA,
                                             Rent = attendance.Rent,
                                             TotalPay = totalPay,
                                             Received = 0,
                                             Amount = totalPay,
                                             Balance = 0,
                                             Type = "Attendance"
                                         }).ToList();
            return attendanceParticulars;
        }

        private List<CustomerLedgerParticular> GetCustomerReceivedParticulars(CustomerLedgerRequestModel requestModel)
        {
            var paymentParticulars = (from receipt in _context.CustomerReceipts
                                      join cus in _context.Customers on receipt.CustomerId equals cus.Id
                                      where !receipt.IsDeleted
                                         && receipt.Date.Date >= requestModel.FromDate.Date
                                         && receipt.Date.Date <= requestModel.ToDate.Date
                                         && cus.Id == requestModel.CustomerId
                                      select new CustomerLedgerParticular
                                      {
                                          Date = receipt.Date,
                                          Particular = "Amount Received",
                                          CustomerPay = 0,
                                          CustomerTA = 0,
                                          Rent = 0,
                                          TotalPay = 0,
                                          Received = receipt.PaidAmount,
                                          Amount = -receipt.PaidAmount,
                                          Balance = 0,
                                          Type = "Receipt"
                                      }).ToList();
            return paymentParticulars;
        }

        private long GetCustomerLedgerOpeningBalance(CustomerLedgerRequestModel requestModel)
        {
            var attendanceTotal = (from attendance in _context.Attendances
                                   join cus in _context.Customers on attendance.CustomerId equals cus.Id
                                   let totalPay = attendance.CustomerPay + attendance.CustomerTA + attendance.Rent
                                   where !attendance.IsDeleted
                                   && attendance.Date.Date < requestModel.FromDate.Date
                                   && cus.Id == requestModel.CustomerId
                                   select totalPay).Sum();

            var receiptTotal = (from receipt in _context.CustomerReceipts
                                join cus in _context.Customers on receipt.CustomerId equals cus.Id
                                where !receipt.IsDeleted
                                   && receipt.Date.Date < requestModel.FromDate.Date
                                   && cus.Id == requestModel.CustomerId
                                select receipt.PaidAmount).Sum();

            var openingBalance = attendanceTotal - receiptTotal;

            return openingBalance;
        }

        #endregion

        #region Employee Ledger
        [HttpGet("Reports/EmployeeLedger")]
        public async Task<ActionResult<EmployeeLedgerModel>> GetEmployeeLedger([FromQuery] EmployeeLedgerRequestModel requestModel)
        {
            var attendanceParticulars = GetEmployeeAttendanceParticulars(requestModel);
            var paymentParticulars = GetEmployeePaymentParticulars(requestModel);

            var particulars = attendanceParticulars.Union(paymentParticulars).ToList().OrderBy(x => x.Date).ToList();

            long openingBalance = GetEmployeeLedgerOpeningBalance(requestModel);
            var balance = openingBalance;
            long closingBalance = 0;

            for (int i = 1; i < particulars.Count; i++)
            {
                balance += particulars[i - 1].Amount;
                particulars[i - 1].Balance = balance;

                if (i == particulars.Count - 1)
                {
                    balance += particulars[i].Amount;
                    particulars[i].Balance = balance;
                    closingBalance = balance;
                }
            }

            var customerLedgerModel = new EmployeeLedgerModel()
            {
                OpeningBalance = openingBalance,
                Particulars = particulars,
                ClosingBalance = closingBalance
            };

            return Ok(customerLedgerModel);
        }

        private List<EmployeeLedgerParticular> GetEmployeeAttendanceParticulars(EmployeeLedgerRequestModel requestModel)
        {
            var attendanceParticulars = (from attendance in _context.Attendances
                                         join cus in _context.Customers on attendance.CustomerId equals cus.Id
                                         join emp in _context.Employees on attendance.EmployeeId equals emp.Id
                                         where !attendance.IsDeleted
                                         && attendance.Date.Date >= requestModel.FromDate.Date
                                         && attendance.Date.Date <= requestModel.ToDate.Date
                                         && emp.Id == requestModel.EmployeeId
                                         select new EmployeeLedgerParticular
                                         {
                                             Date = attendance.Date,
                                             Particular = $"Work - {cus.Name}",
                                             EmployeePay = attendance.EmployeePay,
                                             Payment = 0,
                                             Amount = attendance.EmployeePay,
                                             Balance = 0,
                                             Type = "Attendance"
                                         }).ToList();
            return attendanceParticulars;
        }

        private List<EmployeeLedgerParticular> GetEmployeePaymentParticulars(EmployeeLedgerRequestModel requestModel)
        {
            var paymentParticulars = (from payment in _context.EmployeePayments
                                      join emp in _context.Employees on payment.EmployeeId equals emp.Id
                                      join type in _context.EmployeePaymentTypes on payment.PaymentTypeId equals type.Id
                                      where !payment.IsDeleted
                                         && payment.Date.Date >= requestModel.FromDate.Date
                                         && payment.Date.Date <= requestModel.ToDate.Date
                                         && emp.Id == requestModel.EmployeeId
                                      select new EmployeeLedgerParticular
                                      {
                                          Date = payment.Date,
                                          Particular = $"Payment - {type.Name}",
                                          EmployeePay = 0,
                                          Payment = payment.Amount,
                                          Amount = -payment.Amount,
                                          Balance = 0,
                                          Type = "paid"
                                      }).ToList();
            return paymentParticulars;
        }

        private long GetEmployeeLedgerOpeningBalance(EmployeeLedgerRequestModel requestModel)
        {
            var attendanceTotal = (from attendance in _context.Attendances
                                   join emp in _context.Employees on attendance.EmployeeId equals emp.Id
                                   where !attendance.IsDeleted
                                   && attendance.Date.Date < requestModel.FromDate.Date
                                   && emp.Id == requestModel.EmployeeId
                                   select attendance.EmployeePay).Sum();

            var receiptTotal = (from payment in _context.EmployeePayments
                                join emp in _context.Employees on payment.EmployeeId equals emp.Id
                                where !payment.IsDeleted
                                   && payment.Date.Date < requestModel.FromDate.Date
                                   && emp.Id == requestModel.EmployeeId
                                select payment.Amount).Sum();

            var openingBalance = attendanceTotal - receiptTotal;

            return openingBalance;
        }
        #endregion

        #region Income And Expenditure Report
        [HttpGet("Reports/IncomeAndExpenditure")]
        public async Task<ActionResult<IncomeAndExpenditureReportModel>> GetIncomeAndExpenditureReport([FromQuery] ReceiptAndPaymentRequestModel requestModel)
        {
            var particulars = await (from x in _context.ReceiptAndPayments
                                     join accHead in _context.AccountHeads on x.AccountHeadId equals accHead.Id
                                     where x.Date.Date >= requestModel.FromDate.Date
                                                   && x.Date.Date <= requestModel.ToDate.Date
                                                   && !x.IsDeleted
                                                   && (requestModel.AccountHeadId == null || x.AccountHeadId == requestModel.AccountHeadId)
                                                   && (requestModel.TransactionType == null || x.TransactionType == requestModel.TransactionType)
                                     orderby x.Date ascending
                                     select new IncomeAndExpenditureReportParticular
                                     {
                                         Date = x.Date,
                                         AccountHeadId = x.AccountHeadId,
                                         AccountHeadName = accHead.Name,
                                         Particular = x.Particular,
                                         TransactionType = x.TransactionType.ToString(),
                                         ReceivedAmount = (x.TransactionType == TransactionTypes.Receipt ? x.Amount : 0),
                                         PaidAmount = (x.TransactionType == TransactionTypes.Payment ? x.Amount : 0),
                                         Amount = (x.TransactionType == TransactionTypes.Receipt ? x.Amount : -x.Amount),
                                         Balance = 0
                                     }).ToListAsync();

            long openingBalance = GetIAndEOpeningBalance(requestModel);
            var balance = openingBalance;
            long closingBalance = 0;

            for (int i = 1; i < particulars.Count; i++)
            {
                balance += particulars[i - 1].Amount;
                particulars[i - 1].Balance = balance;

                if (i == particulars.Count - 1)
                {
                    balance += particulars[i].Amount;
                    particulars[i].Balance = balance;
                    closingBalance = balance;
                }
            }

            var incomeAndExpenditureReportModel = new IncomeAndExpenditureReportModel()
            {
                OpeningBalance = openingBalance,
                ClosingBalance = closingBalance,
                Particulars = particulars
            };

            return Ok(incomeAndExpenditureReportModel);
        }

        private long GetIAndEOpeningBalance(ReceiptAndPaymentRequestModel requestModel)
        {
            var amounts = (from x in _context.ReceiptAndPayments
                           where x.Date.Date < requestModel.FromDate.Date
                                  && !x.IsDeleted
                                  && (requestModel.AccountHeadId == null || x.AccountHeadId == requestModel.AccountHeadId)
                                  && (requestModel.TransactionType == null || x.TransactionType == requestModel.TransactionType)
                           select x.TransactionType == TransactionTypes.Receipt ? x.Amount : -x.Amount).ToList();

            var openingBalance = amounts.Sum();
            return openingBalance;
        }
        #endregion

        #endregion


        #region Account Heads

        [HttpGet("AccountHeads")]
        public async Task<ActionResult<IEnumerable<AccountHeaderModel>>> GetAllAccountHeads([FromQuery] AccountHeaderResponseRequestModel requestModel)
        {
            var accountHeadEntities = (from head in _context.AccountHeads
                                       join grp in _context.AccountGroups on head.AccountGroupId equals grp.Id
                                       where
                                       (requestModel.AccountType == null || grp.AccountType == requestModel.AccountType)
                                       && (requestModel.GroupId == null || grp.Id == requestModel.GroupId)
                                       || head.IsDualAccount
                                       select head).ToList();

            if (requestModel.Restricted)
                accountHeadEntities = (from x in accountHeadEntities where x.Id > 3 select x).ToList();

            return Ok(AccoutHeadMapper(accountHeadEntities));
        }

        [HttpPost("AccountHeads")]
        public async Task<ActionResult<AccountHeaderModel>> CreateAccountHead(AccountHeaderCreateRequestModel requestModel)
        {
            if ((from x in _context.AccountHeads where x.AccountGroupId == requestModel.AccountGroupId && x.Name == requestModel.Name && !x.IsDeleted select x).Any())
            {
                throw new Exception("Account Name is already exists under the same group");
            }

            var accountHeadEntity = new AccountHeadEntity()
            {
                Name = requestModel.Name,
                AccountGroupId = requestModel.AccountGroupId,
                Description = requestModel.Description
            };

            _context.AccountHeads.Add(accountHeadEntity);
            await _context.SaveChangesAsync();

            return Ok(AccoutHeadMapper(accountHeadEntity));
        }

        [HttpPut("AccountHeads/AccountHeadId/{accountHeadId}")]
        public async Task<ActionResult<AccountHeaderModel>> UpdateAccountHead(long accountHeadId, AccountHeaderCreateRequestModel requestModel)
        {
            if ((from x in _context.AccountHeads where x.Id != accountHeadId && x.AccountGroupId == requestModel.AccountGroupId && x.Name == requestModel.Name && !x.IsDeleted select x).Any())
            {
                throw new Exception("Account Name is already exists under the same group");
            }

            var accountHeadQuery = from x in _context.AccountHeads where x.Id == accountHeadId select x;
            if (!accountHeadQuery.Any())
                throw new Exception($"Sorry! Account head Id {accountHeadId} not found");

            var accountHeadEntity = accountHeadQuery.FirstOrDefault();
            accountHeadEntity.Name = requestModel.Name;
            accountHeadEntity.AccountGroupId = requestModel.AccountGroupId;
            accountHeadEntity.Description = requestModel.Description;

            _context.AccountHeads.Update(accountHeadEntity);

            await _context.SaveChangesAsync();

            return Ok(AccoutHeadMapper(accountHeadEntity));
        }

        [HttpDelete("AccountHeads/AccountHeadId/{accountHeadId}")]
        public async Task<ActionResult<AccountHeaderModel>> DeleteAccountHead(long accountHeadId)
        {
            var accountHeadQuery = from x in _context.AccountHeads where x.Id == accountHeadId select x;
            if (!accountHeadQuery.Any())
                throw new Exception($"Sorry! Account head Id {accountHeadId} not found");

            var accountHeadEntity = accountHeadQuery.FirstOrDefault();
            accountHeadEntity.IsDeleted = true;

            _context.AccountHeads.Update(accountHeadEntity);

            await _context.SaveChangesAsync();

            return Ok(AccoutHeadMapper(accountHeadEntity));
        }

        private IEnumerable<AccountHeaderModel> AccoutHeadMapper(IEnumerable<AccountHeadEntity> accountHeadEntities)
        {
            var accountHeads = (from head in accountHeadEntities
                                join grp in _context.AccountGroups on head.AccountGroupId equals grp.Id
                                select new AccountHeaderModel
                                {
                                    Id = head.Id,
                                    Name = head.Name,
                                    Description = head.Description,
                                    GroupId = grp.Id,
                                    GroupName = grp.Name,
                                    AccountType = grp.AccountType.ToString()
                                }).ToList();
            return accountHeads;
        }

        private AccountHeaderModel AccoutHeadMapper(AccountHeadEntity accountHeadEntity)
        {
            return AccoutHeadMapper(new List<AccountHeadEntity>() { accountHeadEntity }).FirstOrDefault();
        }


        #endregion


        #region Receipt And Payments

        [HttpGet("ReceiptAndPayments")]
        public async Task<ActionResult<IEnumerable<ReceiptAndPaymentModel>>> GetAllReceiptAndPayments([FromQuery] ReceiptAndPaymentRequestModel requestModel)
        {
            var particularsEntities = await (from x in _context.ReceiptAndPayments
                                             where x.Date.Date >= requestModel.FromDate.Date
                                             && x.Date.Date <= requestModel.ToDate.Date
                                             && x.AccountHeadId > 3
                                             && !x.IsDeleted
                                             && (requestModel.AccountHeadId == null || x.AccountHeadId == requestModel.AccountHeadId)
                                             && (requestModel.TransactionType == null || x.TransactionType == requestModel.TransactionType)
                                             orderby x.Date ascending
                                             select x).ToListAsync();

            var particulars = ReceiptAndPaymentMapper(particularsEntities).ToList();

            long openingBalance = 0;
            long closingBalance = 0;

            if (requestModel.TransactionType != null && requestModel.AccountHeadId != null)
            {
                openingBalance = GetAccountHeadOpeningBalance(requestModel);

                long balance = openingBalance;
                for (int i = 1; i < particulars.Count; i++)
                {
                    balance += particulars[i - 1].Amount;

                    if (i == particulars.Count - 1)
                    {
                        balance += particulars[i].Amount;
                        closingBalance = balance;
                    }
                }
            }

            var receiptAndPaymentModel = new ReceiptAndPaymentModel()
            {
                OpeningBalance = openingBalance,
                ClosingBalance = closingBalance,
                Particulars = particulars
            };

            return Ok(receiptAndPaymentModel);
        }

        [HttpPost("ReceiptAndPayments")]
        public async Task<ActionResult<ReceiptAndPaymentModelParticulars>> CreateReceiptAndPamentTransaction([FromBody] CreateReceiptAndPaymentRequestModel requestModel)
        {
            var rAndPEntity = new ReceiptAndPaymentEntity()
            {
                Date = requestModel.Date,
                TransactionType = requestModel.TransactionType,
                AccountHeadId = requestModel.AccountHeadId,
                Particular = requestModel.Particular,
                Amount = requestModel.Amount,
                Description = requestModel.Description
            };

            _context.ReceiptAndPayments.Add(rAndPEntity);
            await _context.SaveChangesAsync();

            return ReceiptAndPaymentMapper(rAndPEntity);
        }

        [HttpDelete("ReceiptAndPayments/TransactionId/{transactionId}")]
        public async Task<ActionResult<ReceiptAndPaymentModelParticulars>> DeleteReceiptAndPayment(long transactionId)
        {
            var transactionQuery = from x in _context.ReceiptAndPayments where x.Id == transactionId select x;
            if (!transactionQuery.Any())
                throw new Exception($"Sorry! Account head Id {transactionId} not found");

            var transactionEntity = transactionQuery.FirstOrDefault();
            transactionEntity.IsDeleted = true;
            transactionEntity.Amount = 0;

            _context.ReceiptAndPayments.Update(transactionEntity);
            await _context.SaveChangesAsync();

            return ReceiptAndPaymentMapper(transactionEntity);
        }

        [HttpPut("ReceiptAndPayments/TransactionId/{transactionId}")]
        public async Task<ActionResult<ReceiptAndPaymentModelParticulars>> UpdateReceiptAndPayment(long transactionId, [FromBody] CreateReceiptAndPaymentRequestModel requestModel)
        {
            var transactionQuery = from x in _context.ReceiptAndPayments where x.Id == transactionId select x;
            if (!transactionQuery.Any())
                throw new Exception($"Sorry! Account head Id {transactionId} not found");

            var transactionEntity = transactionQuery.FirstOrDefault();
            transactionEntity.Date = requestModel.Date;
            transactionEntity.TransactionType = requestModel.TransactionType;
            transactionEntity.AccountHeadId = requestModel.AccountHeadId;
            transactionEntity.Particular = requestModel.Particular;
            transactionEntity.Amount = requestModel.Amount;
            transactionEntity.Description = requestModel.Description;

            _context.ReceiptAndPayments.Update(transactionEntity);
            await _context.SaveChangesAsync();

            return ReceiptAndPaymentMapper(transactionEntity);
        }

        private long GetAccountHeadOpeningBalance(ReceiptAndPaymentRequestModel requestModel)
        {
            long openingBalance = (from x in _context.ReceiptAndPayments
                                   where x.Date.Date < requestModel.FromDate.Date
                                   && !x.IsDeleted
                                   && x.AccountHeadId == requestModel.AccountHeadId
                                   select x.Amount).Sum();
            return openingBalance;
        }

        private IEnumerable<ReceiptAndPaymentModelParticulars> ReceiptAndPaymentMapper(IEnumerable<ReceiptAndPaymentEntity> receiptAndPaymentEntities)
        {
            var receiptAndPaymentModels = (from rAndP in receiptAndPaymentEntities
                                           join account in _context.AccountHeads on rAndP.AccountHeadId equals account.Id
                                           select new ReceiptAndPaymentModelParticulars
                                           {
                                               Id = rAndP.Id,
                                               Date = rAndP.Date,
                                               AccountHeadId = account.Id,
                                               AccountHeadName = account.Name,
                                               TransactionType = rAndP.TransactionType.ToString(),
                                               Amount = rAndP.Amount,
                                               Particular = rAndP.Particular,
                                               Description = rAndP.Description,
                                               LastUpdatedDate = rAndP.LastUpdatedDate
                                           }).ToList();
            return receiptAndPaymentModels;
        }

        private ReceiptAndPaymentModelParticulars ReceiptAndPaymentMapper(ReceiptAndPaymentEntity receiptAndPaymentEntity)
        {
            return ReceiptAndPaymentMapper(new List<ReceiptAndPaymentEntity>() { receiptAndPaymentEntity }).FirstOrDefault();
        }

        #endregion

    }
}
