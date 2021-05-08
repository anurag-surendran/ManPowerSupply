using ManPowerSupplyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Repositories
{
    public interface IDashboardRepository
    {
        Task<long> GetCustomerBalance();

        Task<long> GetEmployeeBalance();

        Task<long> GetTotalPayments();

        Task<long> GetTotalReceipts();

        Task<long> GetCurrentAmount();

        Task<IEnumerable<DashboardAttendanceModel>> GetConsolidatedAttendance(bool isCurrentMonth);

    }
}
