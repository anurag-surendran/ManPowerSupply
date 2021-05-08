using ManPowerSupplyAPI.Models;
using ManPowerSupplyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repository;

        public DashboardController(IDashboardRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<long>> GetCustomerBalance()
        {
            var result = await _repository.GetCustomerBalance();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<long>> GetEmployeeBalance()
        {
            var result = await _repository.GetEmployeeBalance();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<long>> GetCurrentAmount()
        {
            var result = await _repository.GetCurrentAmount();
            return Ok(result);
        }

        [HttpGet("[action]/{isCurrentMonth?}")]
        public async Task<ActionResult<IEnumerable<DashboardAttendanceModel>>> GetConsolidatedAttendance(bool isCurrentMonth)
        {
            var result = await _repository.GetConsolidatedAttendance(isCurrentMonth);
            return Ok(result);
        }

    }
}
