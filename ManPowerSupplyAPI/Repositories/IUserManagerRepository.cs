using ManPowerSupplyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Repositories
{
    public interface IUserManagerRepository
    {
        Task<IEnumerable<UserOrganization>> GetAllOrganization();

        Task<IEnumerable<UserRole>> GetAllRoles();

        Task<UserResponseModel> AddUser(UserRequestModel requestModel);

        Task<UserResponseModel> UpdateUser(UserRequestModel requestModel,long userId);

        Task<bool> ValidatePassword(ValidatePasswordRequestModel requestModel);

        Task<bool> UpdatePassword(UpdatePasswordRequestModel requestModel);       

    }
}
