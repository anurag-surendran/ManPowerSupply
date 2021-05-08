using ManPowerSupplyAPI.DatabaseManagement;
using ManPowerSupplyAPI.Helpers;
using ManPowerSupplyAPI.Models;
using ManPowerSupplyAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Services
{
    public class DashboardService : IDashboardRepository
    {
        private readonly ManPowerDbContext _context;

        public DashboardService(ManPowerDbContext context)
        {
            _context = context;
        }

        async Task<long> IDashboardRepository.GetCurrentAmount()
        {
            var amounts = await (from x in _context.ReceiptAndPayments
                                 where !x.IsDeleted
                                   && x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                                 group x by x.TransactionType into grp
                                 select grp.ToList().Sum(x => x.Amount)).ToListAsync();

            var result = amounts[0] - amounts[1];
            return result;
        }

        async Task<long> IDashboardRepository.GetCustomerBalance()
        {
            var receivable = (from x in _context.Attendances
                              where !x.IsDeleted
                                    && x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                              select x
                             ).Sum(x => x.CustomerTA + x.CustomerPay + x.Rent);

            var received = (from x in _context.CustomerReceipts
                            where !x.IsDeleted
                                    && x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                            select x
                           ).Sum(x => x.PaidAmount);
            var result = receivable - received;
            return await Task.Run(() =>
            {
                return result;
            });
        }

        async Task<IEnumerable<DashboardAttendanceModel>> IDashboardRepository.GetConsolidatedAttendance(bool isCurrentMonth)
        {
            var fromDate = DateTime.Now;
            var toDate = DateTime.Now.AddDays(-1);
            if (isCurrentMonth)
                fromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
            else
                fromDate = fromDate.AddMonths(-1);

            var attendances = (from attendance in _context.Attendances
                               where !attendance.IsDeleted
                                  && attendance.OrganizationId == LoggedUser.Instance.CurrentOrganization
                                  && attendance.AttendanceStatus == true
                                  && attendance.Date >= fromDate
                                  && attendance.Date <= toDate
                               select attendance).ToList();

            var result = (from attendance in attendances
                          group attendance by attendance.Date into grp
                          select new DashboardAttendanceModel
                          {
                              Date = grp.Key,
                              Amount = grp.ToList().Sum(x => x.CustomerPay + x.CustomerTA + x.Rent),
                              AttendanceCount = grp.ToList().Count
                          }).ToList();
            return result;
        }

        async Task<long> IDashboardRepository.GetEmployeeBalance()
        {
            var payable = (from x in _context.Attendances
                           where !x.IsDeleted
                                && x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                           select x).Sum(x => x.EmployeePay);

            var paid = (from x in _context.EmployeePayments
                        where !x.IsDeleted
                            && x.OrganizationId == LoggedUser.Instance.CurrentOrganization
                        select x).Sum(x => x.Amount);
            var result = payable - paid;

            return await Task.Run(() =>
            {
                return result;
            });
        }

        Task<long> IDashboardRepository.GetTotalPayments()
        {
            throw new NotImplementedException();
        }

        Task<long> IDashboardRepository.GetTotalReceipts()
        {
            throw new NotImplementedException();
        }
    }
}
