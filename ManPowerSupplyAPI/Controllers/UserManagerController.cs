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
    public class UserManagerController : ControllerBase
    {
        private readonly IUserManagerRepository _repository;

        public UserManagerController(IUserManagerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<UserOrganization>>> GetAllOrganization()
        {
            var result = await _repository.GetAllOrganization();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAllRoles()
        {
            var result = await _repository.GetAllRoles();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserResponseModel>> AddUser(UserRequestModel requestModel)
        {
            var result = await _repository.AddUser(requestModel);
            return Ok(result);
        }

        [HttpPut("[action]/{userId}")]
        public async Task<ActionResult<UserResponseModel>> UpdateUser(UserRequestModel requestModel, long userId)
        {
            var result = await _repository.UpdateUser(requestModel, userId);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ValidatePassword(ValidatePasswordRequestModel requestModel)
        {
            var result = await _repository.ValidatePassword(requestModel);
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<bool>> UpdatePassword(UpdatePasswordRequestModel requestModel)
        {
            var result = await _repository.UpdatePassword(requestModel);
            return Ok(result);
        }

    }
}
